
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using ChinookSystem.DAL;
using ChinookSystem.Entities;
using ChinookSystem.ViewModels;
#endregion

namespace ChinookSystem.BLL
{
    public class AlbumController
    {
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
    }
}
