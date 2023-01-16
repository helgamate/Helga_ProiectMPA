namespace Helga_ProiectMPA.Models
{
    public class PublishedPlaylist
    {
        public int PublisherID { get; set; }
        public int PlaylistID { get; set; }
        public Publisher Publisher { get; set; }
        public Playlist  Playlist { get; set; }
    }

}
