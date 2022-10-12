using Microsoft.AspNetCore.Identity;

namespace BulkyBookWeb.ViewModels
{
    public class ApplicationUser : IdentityUser
    {
        private string _city;
        public string City
        {
            get { return this._city; }
            set { this._city = value; }
        }
    }
}
