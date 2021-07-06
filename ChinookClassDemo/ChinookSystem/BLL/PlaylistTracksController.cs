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
        public void MoveTrack(string username, string playlistname, int trackid, int tracknumber, string direction)
        {
            using (var context = new ChinookSystemContext())
            {
                //code to go here 

            }
        }//eom


        public void DeleteTracks(string username, string playlistname, List<int> trackstodelete)
        {
            using (var context = new ChinookSystemContext())
            {
               //code to go here


            }
        }//eom
    }
}
