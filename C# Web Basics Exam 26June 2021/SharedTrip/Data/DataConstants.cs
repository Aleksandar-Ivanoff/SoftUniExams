using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedTrip.Data
{
   public class DataConstants
    {
        public const int DefaultMaxLength = 20;
        public const int RepositoryNameMaxLength = 10;
        public const int DescriptionMaxLength = 80;
        public const int UsernameMinLength = 5;
        public const int PasswordMinLength = 6;
        public const string EmailRegexPattern = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";
        public const int RepositoryNameMinLength = 3;
        public const int SeatsMinLength = 2;
        public const int SeatsMaxLength = 6;
        public const string DepartureTimeFormat = "dd.MM.yyyy HH:mm";

    }
}
