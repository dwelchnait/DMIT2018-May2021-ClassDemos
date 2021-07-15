
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using ChinookSystem.DAL;
using ChinookSystem.Entities;
using ChinookSystem.ViewModels;
using System.ComponentModel;
#endregion

namespace ChinookSystem.BLL
{
    [DataObject]
    public class AlbumController
    {
        #region Queries
        public List<AlbumItem> Albums_GetByArtist(int artistid)
        {
            using (var context = new ChinookSystemContext())
            {
                //this code uses a Linq query
                //this example uses "query" syntax
                IEnumerable<AlbumItem> results = from x in context.Albums
                                                 where x.ArtistId == artistid
                                                 select new AlbumItem
                                                 {
                                                    AlbumId = x.AlbumId,
                                                    Title = x.Title,
                                                    ArtistId = artistid,
                                                    ReleaseYear = x.ReleaseYear,
                                                    ReleaseLabel = x.ReleaseLabel
                                                 };
                return results.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select,false)]
        public List<AlbumItem> Albums_List()
        {
            using (var context = new ChinookSystemContext())
            {
                //this code uses a Linq query
                //this example uses "query" syntax
                IEnumerable<AlbumItem> results = from x in context.Albums
                                                 select new AlbumItem
                                                 {
                                                     AlbumId = x.AlbumId,
                                                     Title = x.Title,
                                                     ArtistId = x.ArtistId,
                                                     ReleaseYear = x.ReleaseYear,
                                                     ReleaseLabel = x.ReleaseLabel
                                                 };
                return results.ToList();
            }
        }

        public List<ArtistAlbumsByTitleandYear> Albums_ListArtistAblumsByTitleYear()
        {
            using (var context = new ChinookSystemContext())
            {
                IEnumerable<ArtistAlbumsByTitleandYear> results = context.Albums
                           .OrderBy(x => x.Artist.Name)
                           .ThenBy(x => x.Title)
                           .ThenByDescending(x => x.ReleaseYear)
                           .Where(x => x.ReleaseYear > 1979 && x.ReleaseYear < 1990)
                           .Select(x => new ArtistAlbumsByTitleandYear
                           {
                               Artist = x.Artist.Name,
                               Title = x.Title,
                               Year = x.ReleaseYear,
                               Label = x.ReleaseLabel == null ? "Unknown" : x.ReleaseLabel
                           });
                return results.ToList();
            }

        }
        #endregion

        #region Add, Update and Delete CRUD
        [DataObjectMethod(DataObjectMethodType.Insert,false)]
        public int Albums_Add(AlbumItem album)
        {
            using (var context = new ChinookSystemContext())
            {
                //due to the ffact that we have separated the handling of our entities
                //  from the data transfer between web app and class libraray
                //  using the ViewModel classes, we MUST create an instance
                //  of the entity and MOVE the data from the ViewModel class instance
                //  to the entity instance
                Album addAlbum = new Album()
                { 
                    //why no pkey set?
                    //pkey is an identity pkey, no value is needed
                    Title = album.Title,
                    ArtistId = album.ArtistId,
                    ReleaseYear = album.ReleaseYear,
                    ReleaseLabel = album.ReleaseLabel
                };

                //staging
                //setup in local memory
                //at this point you will NOT have sent anything to the database
                //  therefore, you will NOT have your new pkey as yet
                context.Albums.Add(addAlbum);

                //commit to database
                //on this command you:
                //  a) execute any entity validation annotation
                //  b) send your local memory staging to the database for execution
                //after a successful execution your entity instance will have the
                //  new pkey value
                context.SaveChanges();

                //at this point, if successful, your entity instance has the new 
                //  pkey value
                return addAlbum.AlbumId;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Update,false)]
        public void Albums_Update(AlbumItem album)
        {
            using (var context = new ChinookSystemContext())
            {
                Album updateAlbum = new Album()
                {
                    //why is the pkey set?
                    //you MUST identify the record that is to be updated
                    //this is done using the pkey
                    AlbumId = album.AlbumId,
                    Title = album.Title,
                    ArtistId = album.ArtistId,
                    ReleaseYear = album.ReleaseYear,
                    ReleaseLabel = album.ReleaseLabel
                };

                //staging
                //all fields on record are changed
                context.Entry(updateAlbum).State = System.Data.Entity.EntityState.Modified;

                //commit to database
                context.SaveChanges();

            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete,false)]
        public void Albums_Delete(AlbumItem album)
        {
            Albums_Delete(album.AlbumId);
        }

        public void Albums_Delete(int albumid)
        {
            using (var context = new ChinookSystemContext())
            {
                //example of a phyiscal delete
                //this is where the record is phyiscally removed from the database
                //thus, you will do a .Remove()
                var exists = context.Albums.Find(albumid);
                //the results is either the record or a null 
                if (exists == null)
                {
                    throw new Exception($"No albumby the id of ({albumid}) exists on file");
                }

                context.Albums.Remove(exists);
                context.SaveChanges();


                //example of a logical delte
                //this is where your will set a attribute on the database record
                //  which logical indicates not to use the record
                //this type of delete is actually an Update to the record
            }
        }
        #endregion
    }
}
