using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;

namespace BulkyBookWeb.ViewModels
{
    public class LoginViewModel
    {
        #region Vaiables
        private string _Email;
        private string _password;
        private bool _RememberMe;
        public string ReturnUrl { get; set; }

        // AuthenticationScheme is in Microsoft.AspNetCore.Authentication namespace
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        #endregion
        #region Properties
        [Required]
        //[EmailAddress]
        public string Email
        {
            get { return this._Email; }
            set { this._Email = value; }
        }

        [Required]
        [DataType(DataType.Password)]
        public string Password
        {
            get { return this._password; }
            set { this._password = value; }
        }
        [Display(Name = "Remember Me")]
        public bool RememberMe
        {
            get { return this._RememberMe; }
            set { this._RememberMe = value; }
        }
        #endregion
    }
}
