using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using System.Data.Entity;
using ChinookSystem.Entities;
#endregion

namespace ChinookSystem.DAL
{
    internal class ChinookSystemContext: DbContext
    {
        //we need a constructor to pass the connection string name
        //  as an argument value to EntityFramework DbContext
        //we want to do this every time we create an instance of this
        //  context class
        public ChinookSystemContext() : base("name=ChinookDB")
        {

        }

        public DbSet<Artist> Artists { get; set; }
        public DbSet<Album> Albums { get; set; }
    }
}
