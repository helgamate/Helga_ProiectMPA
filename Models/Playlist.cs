using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Helga_ProiectMPA.Models
{
    public class Playlist
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }

        [Column(TypeName = "decimal(6, 2)")]
        public decimal Price { get; set; }
        public ICollection<Ordering> Orderings { get; set; }

        public ICollection<PublishedPlaylist> PublishedPlaylists { get; set; }



    }
}
