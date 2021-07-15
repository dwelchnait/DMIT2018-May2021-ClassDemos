using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinookSystem.ViewModels
{
    public class MoveTrackItem
    {
        public string PlaylistName { get; set; }
        public string UserName { get; set; }
        public int TrackID { get; set; }
        public int TrackNumber { get; set; }
        public string Direction { get; set; }
    }
}
