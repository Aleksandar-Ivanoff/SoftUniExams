using Microsoft.EntityFrameworkCore;
using MyWebServer.Controllers;
using MyWebServer.Http;
using SharedTrip.Data;
using SharedTrip.Data.Models;
using SharedTrip.Models;
using SharedTrip.Models.Trips;
using SharedTrip.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedTrip.Controllers
{
    using static Data.DataConstants;
    public class TripsController : Controller
    {
        private readonly ApplicationDbContext data;
        private readonly IValidator validator;
        private readonly IPasswordHasher hasher;
        public TripsController(ApplicationDbContext context, IValidator validator, IPasswordHasher passwordHasher)
        {
            this.data = context;
            this.validator = validator;
            this.hasher = passwordHasher;
        }

        [Authorize]
        public HttpResponse All()
        {
            var trips = this.data.Trips.Select(t => new ListingAllTripsViewModel { 
            DepartureTime=t.DepartureTime,
            EndPoint=t.EndPoint,
            Id=t.Id,
            Seats=t.Seats,
            StartPoint=t.StartPoint,
            
            }).ToList();

            return this.View(trips);
        }
        
        [Authorize]
        public HttpResponse Add()
        {
            var trips = this.data.Trips.Select(t => new ListingAllTripsViewModel
            {
                DepartureTime = t.DepartureTime,
                EndPoint = t.EndPoint,
                Id = t.Id,
                Seats = t.Seats,
                StartPoint = t.StartPoint,

            }).ToList();

            return this.View(trips);
        }

        [Authorize]
        [HttpPost]
        public HttpResponse Add(AddTripsModel model)
        {
            var errors = this.validator.ValidateTrip(model);

            if (errors.Any())
            {
                return Redirect("/Trips/Add");
            }

            var trip = new Trip {
                EndPoint = model.EndPoint,
                Description = model.Description,
                ImagePath = model.ImagePath,
                Seats = model.Seats,
                StartPoint = model.StartPoint,
                DepartureTime = DateTime.ParseExact(model.DepartureTime, DepartureTimeFormat,CultureInfo.InvariantCulture),
            };

            this.data.Trips.Add(trip);
            this.data.SaveChanges();
            return this.Redirect("/Trips/All");
        }

        [Authorize]
        public HttpResponse Details(string tripId)
        {

          
            var trip = this.data.Trips.Where(t => t.Id == tripId).Select(x => new TripModel
            {
                DepartureTime = x.DepartureTime.ToString(),
                Description = x.Description,
                EndPoint = x.EndPoint,
                Id = x.Id,
                Image = x.ImagePath,
                Seats = x.Seats,
                StartPoint = x.StartPoint,

            }).FirstOrDefault();

            if (trip is null)
            {
                return NotFound();
            }

           

            ;
            return this.View(trip);
        }

        [Authorize]
        public HttpResponse AddUserToTrip(string tripId)
        {


            var trip = this.data.Trips.Where(t => t.Id == tripId).FirstOrDefault();
            var user = this.data.Users.Where(x => x.Id == this.User.Id).FirstOrDefault();

            var isAlreadyJoined = this.data.UserTrips.Any(x => x.TripId == tripId && x.UserId == this.User.Id);

            if (isAlreadyJoined)
            {
                return Redirect($"/Trips/Details?tripId={tripId}");  
            }
            if (trip.Seats == 0)
            {
                return Redirect($"/Trips/Details?tripId={tripId}");
            }
            trip.Seats -= 1;
            var userTrip = new Data.Models.UserTrip { Trip = trip, User = user };

            this.data.UserTrips.Add(userTrip);
            this.data.SaveChanges();

            return this.Redirect("/Trips/All");
        }
    }
}
