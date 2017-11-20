using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using UIA.Models;

namespace UIA.ViewModel
{
    public class BookFlightViewModel
    {
        public int FlightId { get; set; }
        public Flight Flight { get; set; }

        [Required]
        public string SeatsSelected { get; set; }

        public String[] BookedSeats { get; set; }

    }
}