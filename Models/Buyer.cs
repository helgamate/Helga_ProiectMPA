using System.ComponentModel.DataAnnotations.Schema;
namespace Helga_ProiectMPA.Models
{
    public class Buyer
    {
        public int BuyerID { get; set; }
        public string Name { get; set; }
        public string Adress { get; set; }
        public DateTime BirthDate { get; set; }
        public ICollection<Ordering> Orderings { get; set; }

    }
}
