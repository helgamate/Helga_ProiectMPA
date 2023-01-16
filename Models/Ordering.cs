namespace Helga_ProiectMPA.Models
{
    public class Ordering
    {
        public int OrderingID { get; set; }
        public int BuyerID { get; set; }
        public int PlaylistID { get; set; }
        public DateTime OrderingDate { get; set; }

        public Buyer Buyer { get; set; }
        public Playlist Playlist { get; set; }
    }
}
