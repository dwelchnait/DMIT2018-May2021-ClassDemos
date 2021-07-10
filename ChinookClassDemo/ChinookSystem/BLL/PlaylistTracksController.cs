using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using ChinookSystem.Entities;
using ChinookSystem.ViewModels;
using ChinookSystem.DAL;
using System.ComponentModel;
using FreeCode.Exceptions;
#endregion

namespace ChinookSystem.BLL
{
    public class PlaylistTracksController
    {
        //class level variable (data member) will hold multiple strings, 
        //  each representing an error message
        //this variable get created when the instance of the class is created.
        List<Exception> brokenRules = new List<Exception>();

        public List<UserPlaylistTrack> List_TracksForPlaylist(
            string playlistname, string username)
        {
            using (var context = new ChinookSystemContext())
            {
                var results = from x in context.PlaylistTracks
                              where x.Playlist.Name.Equals(playlistname)
                                 && x.Playlist.UserName.Equals(username)
                              orderby x.TrackNumber
                              select new UserPlaylistTrack
                              {
                                  TrackID = x.TrackId,
                                  TrackNumber = x.TrackNumber,
                                  TrackName = x.Track.Name,
                                  Milliseconds = x.Track.Milliseconds,
                                  UnitPrice = x.Track.UnitPrice
                              };
                

                return results.ToList();
            }
        }//eom
        public void Add_TrackToPLaylist(string playlistname, string username, int trackid, string song)
        {
            //method variables
            Playlist playlistExist = null;
            PlaylistTrack playlisttrackExist = null;
            int tracknumber = 0;
            using (var context = new ChinookSystemContext())
            {
                //code to go here
                //validation of data
                if (string.IsNullOrEmpty(playlistname))
                {
                    //there is a data error
                    //setting up an error message
                    brokenRules.Add(new BusinessRuleException<string>("Playlist name is missing. Unable to add track", "PlaylistName", "missing"));
                }

                if (string.IsNullOrEmpty(username))
                {
                    //there is a data error
                    //setting up an error message
                    brokenRules.Add(new BusinessRuleException<string>("User name is missing. Unable to add track", "User Name", "missing"));
                }


                //do I need to create a new playlist?
                //  query for an existing playlist
                playlistExist = (from x in context.Playlists
                                where x.Name.Equals(playlistname)
                                   && x.UserName.Equals(username)
                                select x).FirstOrDefault();

                //Is there a playlist?
                if (playlistExist == null)
                {
                    //no playlist
                    //create and set tracknumber to 1
                    playlistExist = new Playlist()
                    {
                        Name = playlistname,
                        UserName = username
                    };
                    //.Add simply stages your record to processing on the database
                    context.Playlists.Add(playlistExist);
                    tracknumber = 1;
                }
                else
                {
                    //set tracknumber to next highest
                    //query for the current highest tracknumber
                    tracknumber = context.PlaylistTracks
                                    .Where(x => x.Playlist.Name.Equals(playlistname)
                                    && x.Playlist.UserName.Equals(username))
                                    .Count();
                    tracknumber++;
                    //check the business rule: no duplicate tracks
                    playlisttrackExist = context.PlaylistTracks
                                            .Where(x => x.Playlist.Name.Equals(playlistname)
                                                && x.Playlist.UserName.Equals(username)
                                                && x.TrackId == trackid)
                                            .Select(x => x)
                                            .FirstOrDefault();
                    if (playlisttrackExist != null)
                    {
                        //duplicate
                        brokenRules.Add(new BusinessRuleException<string>("Track already exists on the playlist. Duplicates are not allowed", nameof(song), song));
                    }
                }

                //add the playlist track
                playlisttrackExist = new PlaylistTrack();
                //load the instance with data
                playlisttrackExist.TrackId = trackid;
                playlisttrackExist.TrackNumber = tracknumber;

                //what about the playlistid?
                //if this is a new playlist, what is the current value of PlaylistId in the
                //      playlist instance? --> it is 0 (numeric default)

                //to solve this problem, we will do an .Add via the navigational property
                //  of the PlayList entity
                //The processing will add the new Playlist then use the new identity value
                //  in adding the PlaListTrack record
                playlistExist.PlaylistTracks.Add(playlisttrackExist); //staged

                //can I commit?
                //are there any errors in this proces
                if(brokenRules.Count() > 0)
                {
                    //at least one error was discovered during the processing of the
                    //  transaction
                    //throw all errors in one batch
                    throw new BusinessRuleCollectionException("Add Playlist Track concerns:", brokenRules);
                }
                else
                {
                    //COMMIT THE TRANSACTION
                    //NOTE: there is ONE and ONLY ONE .SaveChanges() in a transaction
                    context.SaveChanges();
                }


            }
        }//eom
        public void MoveTrack(MoveTrackItem moveTrack)
        {
            Playlist playlistExist = null;
            PlaylistTrack playlisttrackExist = null;
            int numberoftracks = 0;
            using (var context = new ChinookSystemContext())
            {
                //code to go here 
                if (string.IsNullOrEmpty(moveTrack.PlaylistName))
                {
                    brokenRules.Add(new BusinessRuleException<string>("Playlist name is missing. Unable to move track.","Playlist Name", "missing"));
                }
                if (string.IsNullOrEmpty(moveTrack.UserName))
                {
                    brokenRules.Add(new BusinessRuleException<string>("User name is missing. Unable to move track.", "User Name", "missing"));
                }
                if (moveTrack.TrackID <= 0)
                {
                    brokenRules.Add(new BusinessRuleException<int>("Invalid track identifier. Unable to move track.", "Track identifier", moveTrack.TrackID));
                }
                if (moveTrack.TrackNumber <= 0)
                {
                    brokenRules.Add(new BusinessRuleException<int>("Invlaid track number. Unable to move track.", "Track Number", moveTrack.TrackNumber));
                }
                playlistExist = (from x in context.Playlists
                                 where x.Name.Equals(moveTrack.PlaylistName)
                                    && x.UserName.Equals(moveTrack.UserName)
                                 select x).FirstOrDefault();

                //Is there a playlist?
                if (playlistExist == null)
                {
                    brokenRules.Add(new BusinessRuleException<string>("Playlist no longer exists. Unable to move track.", "Playlist Name", moveTrack.PlaylistName));
                }
                else
                {
                    //due to the way that Linq executes in your program as a "lazy loader"
                    //  we need to query directly the number of tracks in the playlist
                    numberoftracks = context.PlaylistTracks
                                        .Where(x => x.Playlist.Name.Equals(moveTrack.PlaylistName)
                                                && x.Playlist.UserName.Equals(moveTrack.UserName))
                                        .Select(x => x)
                                        .Count();

                    //get track that needs to be moved
                    playlisttrackExist =  (from x in context.PlaylistTracks
                                                where x.Playlist.Name.Equals(moveTrack.PlaylistName)
                                                   && x.Playlist.UserName.Equals(moveTrack.UserName)
                                                   && x.TrackId == moveTrack.TrackID
                                                select x).FirstOrDefault();

                    //does it exists
                    if (playlisttrackExist == null)
                    {
                        brokenRules.Add(new BusinessRuleException<string>("Playlist track no longer exists. Refresh playlist.", "Playlist Name", moveTrack.PlaylistName));
                    }
                    else
                    {
                        //decide the logic to execute up or down (direction)
                        if (moveTrack.Direction.Equals("up"))
                        {
                            if (playlisttrackExist.TrackNumber == 1)
                            {
                                brokenRules.Add(new BusinessRuleException<string>("Playlist track already at the top. Unable to move track.", nameof(Track.Name), playlisttrackExist.Track.Name));
                            }
                            else
                            {
                                //do the move
                                PlaylistTrack othertrack = (from x in context.PlaylistTracks
                                                            where x.Playlist.Name.Equals(moveTrack.PlaylistName)
                                                               && x.Playlist.UserName.Equals(moveTrack.UserName)
                                                               && x.TrackNumber == playlisttrackExist.TrackNumber - 1
                                                            select x).FirstOrDefault();
                                if(othertrack == null)
                                {
                                    brokenRules.Add(new BusinessRuleException<string>("Playlist track to swap seems to be missing. Refresh your display.", nameof(MoveTrackItem.PlaylistName), moveTrack.PlaylistName));
                                }
                                else
                                {
                                    //good to swap
                                    playlisttrackExist.TrackNumber -= 1;
                                    othertrack.TrackNumber += 1;
                                    //staging for single field update on a record
                                    context.Entry(playlisttrackExist).Property(nameof(PlaylistTrack.TrackNumber)).IsModified = true;
                                    context.Entry(othertrack).Property(nameof(PlaylistTrack.TrackNumber)).IsModified = true;
                                }
                            }
                        }
                        else
                        { 
                            //down movement
                            if (playlisttrackExist.TrackNumber == numberoftracks)
                            {
                                brokenRules.Add(new BusinessRuleException<string>("Playlist track already at the bottom. Unable to move track.", nameof(Track.Name), playlisttrackExist.Track.Name));
                            }
                            else
                            {
                                //do the move
                                PlaylistTrack othertrack = (from x in context.PlaylistTracks
                                                            where x.Playlist.Name.Equals(moveTrack.PlaylistName)
                                                               && x.Playlist.UserName.Equals(moveTrack.UserName)
                                                               && x.TrackNumber == playlisttrackExist.TrackNumber + 1
                                                            select x).FirstOrDefault();
                                if (othertrack == null)
                                {
                                    brokenRules.Add(new BusinessRuleException<string>("Playlist track to swap seems to be missing. Refresh your display.", nameof(MoveTrackItem.PlaylistName), moveTrack.PlaylistName));
                                }
                                else
                                {
                                    //good to swap
                                    playlisttrackExist.TrackNumber += 1;
                                    othertrack.TrackNumber -= 1;
                                    //staging for single field update on a record
                                    context.Entry(playlisttrackExist).Property(nameof(PlaylistTrack.TrackNumber)).IsModified = true;
                                    context.Entry(othertrack).Property(nameof(PlaylistTrack.TrackNumber)).IsModified = true;
                                }
                            }

                        }
                    }
                }
                
                //commit??
                //are there any errors in this proces
                if (brokenRules.Count() > 0)
                {
                    //at least one error was discovered during the processing of the
                    //  transaction
                    //throw all errors in one batch
                    throw new BusinessRuleCollectionException("Move Playlist Track concerns:", brokenRules);
                }
                else
                {
                    //COMMIT THE TRANSACTION
                    //NOTE: there is ONE and ONLY ONE .SaveChanges() in a transaction
                    context.SaveChanges();
                }
               
            }
        }//eom


        public void DeleteTracks(string username, string playlistname, List<int> trackstodelete)
        {
            Playlist playlistExist = null;
            PlaylistTrack playlisttrackExist = null;
            using (var context = new ChinookSystemContext())
            {
                //code to go here
                if (string.IsNullOrEmpty(playlistname))
                {
                    brokenRules.Add(new BusinessRuleException<string>("Playlist name is missing. Unable to remove track(s).", "Playlist Name", "missing"));
                }
                if (string.IsNullOrEmpty(username))
                {
                    brokenRules.Add(new BusinessRuleException<string>("User name is missing. Unable to remove track(s).", "User Name", "missing"));
                }
                if (trackstodelete.Count <= 0)
                {
                    brokenRules.Add(new BusinessRuleException<string>("The were no requested tracks to remove. ", "Track selection", " empty"));
                }

                playlistExist = (from x in context.Playlists
                                 where x.Name.Equals(playlistname)
                                    && x.UserName.Equals(username)
                                 select x).FirstOrDefault();
                if (playlistExist == null)
                {
                    brokenRules.Add(new BusinessRuleException<string>("Playlist no longer exists. Unable to remove track(s0.", "Playlist Name", playlistname));
                }
                else
                {
                    //kept a separate collection of the tracks to kept (query ORDERED BY TRACKNUMBER)
                    var trackskept = context.PlaylistTracks
                                    .Where(x => x.Playlist.Name.Equals(playlistname)
                                    && x.Playlist.UserName.Equals(username)
                                    && !trackstodelete.Any(tod => tod == x.TrackId))
                                    .OrderBy(x => x.TrackNumber)
                                    .Select(x => x);

                    //traverse the removal track list (of trackids) and
                    //  remove each track in the list
                    foreach (int deletetrackid in trackstodelete)
                    {
                        playlisttrackExist = (from x in context.PlaylistTracks
                                              where x.Playlist.Name.Equals(playlistname)
                                                 && x.Playlist.UserName.Equals(username)
                                                 && x.TrackId == deletetrackid
                                              select x).FirstOrDefault();
                        //stage track to be deleted
                        if (playlisttrackExist != null)
                        {
                            playlistExist.PlaylistTracks.Remove(playlisttrackExist);
                        }
                    }
                    // 1 2 3 4 6 8 12
                    //what if we simply renumber
                    // 1 2 3 4 5 7 8
                    //renumber the kept separate collection
                    int tracknumber = 1;
                    foreach (var track in trackskept)
                    {
                        track.TrackNumber = tracknumber;
                        context.Entry(track).Property(nameof(PlaylistTrack.TrackNumber)).IsModified = true;
                        tracknumber++;
                    }
                }

                //commit
                //are there any errors in this proces
                if (brokenRules.Count() > 0)
                {
                    //at least one error was discovered during the processing of the
                    //  transaction
                    //throw all errors in one batch
                    throw new BusinessRuleCollectionException("Remove Playlist Track concerns:", brokenRules);
                }
                else
                {
                    //COMMIT THE TRANSACTION
                    //NOTE: there is ONE and ONLY ONE .SaveChanges() in a transaction
                    context.SaveChanges();
                }
            }
        }//eom
    }
}
