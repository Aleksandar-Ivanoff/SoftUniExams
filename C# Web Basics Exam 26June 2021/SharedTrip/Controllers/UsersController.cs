using MyWebServer.Controllers;
using MyWebServer.Http;
using SharedTrip.Data;
using SharedTrip.Data.Models;
using SharedTrip.Models.Users;
using SharedTrip.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedTrip.Controllers
{
   public class UsersController : Controller
    {
        private readonly ApplicationDbContext data;
        private readonly IValidator validator;
        private readonly IPasswordHasher hasher;
        public UsersController(ApplicationDbContext context, IValidator validator, IPasswordHasher passwordHasher)
        {
            this.data = context;
            this.validator = validator;
            this.hasher = passwordHasher;
        }

        public HttpResponse Login()
        {
            return this.View();
        }
        [HttpPost]
        public HttpResponse Login(UserLoginFormModel model)
        {
            var hashedPassword = hasher.HashPassword(model.Password);
            var userId = this.data.Users.Where(u => u.Username == model.Username && u.Password == hashedPassword).Select(u => u.Id).FirstOrDefault();

            if (userId == null)
            {
                return Redirect("/Users/Login");
                //return Error("Username and password combination is not valid");
            }

            this.SignIn(userId);

            return this.Redirect("/Trips/All");
        }

        public HttpResponse Register()
        {
            return this.View();
        }

        [HttpPost]
        public HttpResponse Register(UserRegisterFormModel model)
        {
            var modelErrors = validator.ValidateUser(model);

            if (this.data.Users.Any(u => u.Username == model.Username))
            {
                modelErrors.Add($"User with  {model.Username} username is already exists!");
            }
            if (this.data.Users.Any(u => u.Email == model.Email))
            {
                modelErrors.Add($"User with  {model.Username} email is already exists!");
            }

            if (modelErrors.Any())
            {
                return Redirect("/Users/Register");
                //return Error(modelErrors);
            }

            var user = new User
            {
                Password = hasher.HashPassword(model.Password),
                Username = model.Username,
                Email = model.Email,
            };

            this.data.Users.Add(user);
            this.data.SaveChanges();

            return this.Redirect("/Users/Login");
        }

        public HttpResponse Logout()
        {
            this.SignOut();
            return Redirect("/");
        }
    }
}
