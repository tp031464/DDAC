using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UIA.Models;
using UIA.ViewModel;

namespace UIA.Controllers
{
    public class FlightController : Controller
    {

        private UIAEntities db = new UIAEntities();

        // GET: Flight
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult SearchFlight(SearchFlightViewModel searchFlightViewModel)
        {

            var listOfCountryCode = new List<SelectListItem>
            {
                new SelectListItem{ Text="KUL (Kuala Lumpur)", Value = "KUL" },
                new SelectListItem{ Text="BKK (Bangkok)", Value = "BKK" },
                new SelectListItem{ Text="ICN (Seoul)", Value = "ICN" },
                new SelectListItem{ Text="HND (Tokyo)", Value = "HND" },
                new SelectListItem{ Text="DXB (Dubai)", Value = "DXB" },
                new SelectListItem{ Text="KBP (Kiev)", Value = "KBP" },
                new SelectListItem{ Text="PEK (Beijing)", Value = "PEK" },
                new SelectListItem{ Text="SIN (Singapore)", Value = "SIN" }
            };

            searchFlightViewModel.AirportCode = listOfCountryCode;

            var today = DateTime.Today;
            var search = db.Flights.Where(f => f.DepartureTime > today);

            if (!string.IsNullOrEmpty(searchFlightViewModel.Origin))
            {
                search = search.Where(f => f.Origin == searchFlightViewModel.Origin);
            }

            if (!string.IsNullOrEmpty(searchFlightViewModel.Destination))
            {
                search = search.Where(f => f.Destination == searchFlightViewModel.Destination);
            }

            searchFlightViewModel.FlightList = search.OrderBy(f => f.DepartureTime).ToList();

            return View(searchFlightViewModel);

        }

        [HttpGet]
        [Authorize]
        public ActionResult BookFlight(int flightId)
        {
            BookFlightViewModel flightBookingViewModel = new BookFlightViewModel();

            var flight = db.Flights.FirstOrDefault(f => f.Id == flightId);

            flightBookingViewModel.Flight = flight;
            flightBookingViewModel.FlightId = flightId;

            string bookedSeats = string.Join(",", (db.Bookings.Where(b => b.FlightId == flightId).Select(b => b.Seat)).ToArray());

            TempData["BookedSeats"] = bookedSeats;

            return View(flightBookingViewModel);
        }

        [HttpPost]
        [Authorize]
        public ActionResult BookFlight(BookFlightViewModel bookingFlightViewModel)
        {
            string sessionUserId = (User.Identity.Name.Split('|')[1]);

            Booking booking = new Booking()
            {
                UserId = Convert.ToInt32(sessionUserId),
                FlightId = bookingFlightViewModel.FlightId,
                Seat = bookingFlightViewModel.SeatsSelected
            };
            
            db.Bookings.Add(booking);
            db.SaveChanges();

            return RedirectToAction("BookedFlight", "Flight");
        }

        [Authorize]
        public ActionResult BookedFlight(BookedFlightViewModel bookedFlightViewModel)
        {

            int sessionUserId = Convert.ToInt32((User.Identity.Name.Split('|')[1]));

            var bookingList = (db.ListAllBookings.Where(v =>
                v.DepartureTime >= DateTime.Today && v.UserId == sessionUserId));
            
            bookedFlightViewModel.BookingList = bookingList.ToList();

            return View(bookedFlightViewModel);
        }

        [Authorize]
        public ActionResult BookedFlightDetail(int bookingId)
        {
            BookingDetailViewModel bookingDetailViewModel = new BookingDetailViewModel();

            int sessionUserId = Convert.ToInt32((User.Identity.Name.Split('|')[1]));

            var booking = (db.ListAllBookings.FirstOrDefault(b =>
                b.DepartureTime >= DateTime.Today && b.UserId == sessionUserId && b.BookingId == bookingId));

            bookingDetailViewModel.BookingDetail = booking;

            if (bookingDetailViewModel.BookingDetail != null && bookingDetailViewModel.BookingDetail.Seat.Contains(","))
            {
                ViewBag.SeatList = bookingDetailViewModel.BookingDetail.Seat.Split(',');
            }
            else
            {
                if (bookingDetailViewModel.BookingDetail != null)
                    ViewBag.SeatList = bookingDetailViewModel.BookingDetail.Seat;
            }

            return View(bookingDetailViewModel);
        }
    }
}