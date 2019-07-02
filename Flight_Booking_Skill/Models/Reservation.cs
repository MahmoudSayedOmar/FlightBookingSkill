using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flight_Booking_Skill.Models
{
    public class Reservation
    {
        public string Destination { set;get;}

        public string ReservationDate { set; get; }

        public int NumberOfTickets { set; get; }

    }
}
