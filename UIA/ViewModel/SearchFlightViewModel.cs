using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UIA.Models;

namespace UIA.ViewModel
{
    public class SearchFlightViewModel
    {
        public string Origin { get; set; }
        public string Destination { get; set; }

        [Display(Name = "Deperature Time")]
        public DateTime? DepartureTime { get; set; }

        public List<SelectListItem> AirportCode { get; set; }

        public List<Flight> FlightList { get; set; }

    }
}