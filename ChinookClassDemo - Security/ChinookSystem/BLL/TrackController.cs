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
#endregion

namespace ChinookSystem.BLL
{
    [DataObject]
    public class TrackController
    {
        //[DataObjectMethod(DataObjectMethodType.Select, false)]
        //public List<Track> Track_List()
        //{
        //    using (var context = new ChinookSystemContext())
        //    {
        //        return context.Tracks.ToList();
        //    }
        //}

        //[DataObjectMethod(DataObjectMethodType.Select, false)]
        //public Track Track_Find(int trackid)
        //{
        //    using (var context = new ChinookContext())
        //    {
        //        return context.Tracks.Find(trackid);
        //    }
        //}

        //[DataObjectMethod(DataObjectMethodType.Select, false)]
        //public List<Track> Track_GetByAlbumId(int albumid)
        //{
        //    using (var context = new ChinookContext())
        //    {
        //        var results = from aRowOn in context.Tracks
        //                      where aRowOn.AlbumId.HasValue
        //                      && aRowOn.AlbumId == albumid
        //                      select aRowOn;
        //        return results.ToList();
        //    }
        //}

        [DataObjectMethod(DataObjectMethodType.Select,false)]
        public List<TrackList> List_TracksForPlaylistSelection(string tracksby, string arg)
        {
            using (var context = new ChinookSystemContext())
            {
                List<TrackList> results = null;
                int id = 0;
                if (int.TryParse(arg, out id))
                {
                    results = (from x in context.Tracks
                               where x.GenreId == id &&
                                       tracksby.Equals("Genre")
                               select new TrackList
                               {
                                   TrackID = x.TrackId,
                                   Name = x.Name,
                                   Title = x.Album.Title,
                                   ArtistName = x.Album.Artist.Name,
                                   GenreName = x.Genre.Name,
                                   Composer = x.Composer,
                                   Milliseconds = x.Milliseconds,
                                   Bytes = x.Bytes,
                                   UnitPrice = x.UnitPrice
                               }).ToList();
                }
                else
                {
                    results = (from x in context.Tracks
                               where (x.Album.Artist.Name.Contains(arg) &&
                                       tracksby.Equals("Artist")) ||
                                     (x.Album.Title.Contains(arg) &&
                                       tracksby.Equals("Album")) 
                               select new TrackList
                               {
                                   TrackID = x.TrackId,
                                   Name = x.Name,
                                   Title = x.Album.Title,
                                   ArtistName = x.Album.Artist.Name,
                                   GenreName = x.Genre.Name,
                                   Composer = x.Composer,
                                   Milliseconds = x.Milliseconds,
                                   Bytes = x.Bytes,
                                   UnitPrice = x.UnitPrice
                               }).ToList();
                }
                return results;
            }
        }//eom

       
    }//eoc
}
