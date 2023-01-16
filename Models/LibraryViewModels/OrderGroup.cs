using System;
using System.ComponentModel.DataAnnotations;

namespace Helga_ProiectMPA.Models.LibraryViewModels
{
    public class OrderGroup
    {
        [DataType(DataType.Date)]
        public DateTime? OrderingDate { get; set; }
        public int PlaylistCount { get; set; }

    }
}
