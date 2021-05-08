using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


#region Additional Namespaces
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
#endregion

namespace ChinookSystem.Entities
{
    [Table("Artists")]
    internal class Artist
    {
        [Key]
        public int ArtistId { get; set; }

        [Required(ErrorMessage = "Artist Name is required.")]
        [StringLength(120, MinimumLength = 1, ErrorMessage = "Artist Name is limited to 120 characters")]
        public string Name { get; set; }
    }
}