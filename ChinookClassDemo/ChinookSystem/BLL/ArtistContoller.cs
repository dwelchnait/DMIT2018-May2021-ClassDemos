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
    public class ArtistContoller
    {
        [DataObjectMethod(DataObjectMethodType.Select,false)]
        public  List<SelectionList> Artists_List()
        {
            using(var context = new ChinookSystemContext())
            {
                //this code uses a Linq query
                //this example uses "method" syntax
                IEnumerable<SelectionList> results = context.Artists
                                                        .Select( row => new SelectionList
                                                        {
                                                            ValueField = row.ArtistId,
                                                            DisplayField = row.Name
                                                        });
                return results.OrderBy(x => x.DisplayField).ToList();
            }
        }
    }
}
