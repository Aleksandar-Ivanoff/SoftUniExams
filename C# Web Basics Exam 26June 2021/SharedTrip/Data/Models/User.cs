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
    public class User
    {
        [Required]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(DefaultMaxLength)]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        //[MaxLength(DefaultMaxLength)]
        public string Password { get; set; }

        public ICollection<UserTrip> UserTrips { get; set; } = new List<UserTrip>();

    }
}
