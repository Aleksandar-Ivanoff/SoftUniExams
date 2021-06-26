using SharedTrip.Models.Trips;
using SharedTrip.Models.Users;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SharedTrip.Services
{
    using static Data.DataConstants;
    public class Validator : IValidator
    {
        public ICollection<string> ValidateTrip(AddTripsModel model)
        {
            var errors = new List<string>();

            if (model.StartPoint.Length == 0)
            {
                errors.Add($"StartPoint must not be  empty!");
            }
            if (model.EndPoint.Length == 0)
            {
                errors.Add($"EndPoint must not be  empty!");
            }
            if (model.DepartureTime.Length == 0)
            {
                errors.Add($"DepartureTime must not be  empty!");
            }

            try
            {
                var date = DateTime.ParseExact(model.DepartureTime, DepartureTimeFormat, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {

                errors.Add($"Datetime is invalid.DateTime format must be {DepartureTimeFormat} !");
            }

            if (model.Seats < SeatsMinLength || model.Seats > SeatsMaxLength)
            {
                errors.Add($"Seats must be between {SeatsMinLength} and {SeatsMaxLength}");
            }
            if (model.Description.Length > DescriptionMaxLength)
            {
                errors.Add($"Description length must be max {DescriptionMaxLength}!");
            }
            if (model.Description.Length == 0)
            {
                errors.Add($"Description must not be  empty!");
            }

            return errors;
        }

        public ICollection<string> ValidateUser(UserRegisterFormModel model)
        {
            var errors = new List<string>();


            if (model.Username.Length < UsernameMinLength || model.Username.Length > DefaultMaxLength)
            {
                errors.Add($"Username '{model.Username} is invalid! Characters must be between {UsernameMinLength} and {DefaultMaxLength}!");
            }

            if (model.Password.Length < PasswordMinLength || model.Password.Length > DefaultMaxLength)
            {
                errors.Add($"Password {model.Password} is invalid! It must be between {PasswordMinLength} and {DefaultMaxLength}");
            }
            if (!Regex.IsMatch(model.Email, EmailRegexPattern))
            {
                errors.Add($"Email '{model.Email}' is invalid!");
            }
            if (!model.Password.Equals(model.ConfirmPassword))
            {
                errors.Add("Passwords are different");
            }

            if (model.Password.Any(x => x == ' '))
            {
                errors.Add($"The provided password cannot certain whitespaces.");
            }
            return errors;
        }
    }
}
