using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace az.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public DateTime BookingDate { get; set; }
        public double Price { get; set; }
        [Required]
        public string UserId { get; set; }
        public int MovieId { get; set; }
        public int NoOfSeats { get; set; }
    }
}
