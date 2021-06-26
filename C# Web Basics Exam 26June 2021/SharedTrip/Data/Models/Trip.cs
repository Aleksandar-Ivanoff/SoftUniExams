using SharedTrip.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedTrip.Data.Models
{
    using static DataConstants;
    public class Trip
    {
        [Required]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        
        public string StartPoint { get; set; }

        [Required]
        public string EndPoint { get; set; }

        public DateTime DepartureTime { get; set; }

        [MaxLength(SeatsMinLength)]
        public int Seats { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        public string ImagePath { get; set; }


        public ICollection<UserTrip> UserTrips { get; set; } = new List<UserTrip>();
    }
}
