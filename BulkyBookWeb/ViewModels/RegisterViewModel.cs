

using BulkyBookWeb.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BulkyBookWeb.ViewModels
{
    public class RegisterViewModel
    {
       // public List<UserRoleModel> userRoles { get; set; }
        public RegisterViewModel()
        {
           // userRoles = new List<UserRoleModel>();
        }
        #region Vaiables

        private string _Email;
        private string _password;
        private string _City;
        private string _ConfirmPassword;
        //private string roleId;
        #endregion
        #region Properties
        [Required]
        //[EmailAddress]
        [Remote(action: "IsEmailInUse", controller:"Account")]
        //[ValidEmailDomainAttiributes (allowedDoamin:"Tech.com",ErrorMessage ="Email domain must be Tech.com")]
        //[ValidEmailDomain(allowedDomain: "Tech.com", ErrorMessage = "Email domain must be Tech.com")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
            ErrorMessage = "Invalid Email Format")]
        public string Email
        {
            get { return this._Email; }
            set { this._Email = value; }
        }
        //public string RoleId
        //{
        //    get { return this.roleId; }
        //    set { this.roleId = value; }
        //}
        public string UserCity
        {
            get { return this._City; }
            set { this._City = value; }
        }

        [Required]
        [DataType(DataType.Password)]
        public string Password
        {
            get { return this._password; }
            set { this._password = value; }
        }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password",
            ErrorMessage = "Password and Confirmation Password do not match")]
        public string ConfirmPassword
        {
            get { return this._ConfirmPassword; }
            set { this._ConfirmPassword = value; }
        }

        #endregion
    }

    //public class UserRoleModel
    //{
    //    public string roleId { get; set; }
    //    public string name { get; set; }
    //}
}