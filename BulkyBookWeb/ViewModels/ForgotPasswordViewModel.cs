using System.ComponentModel.DataAnnotations;

namespace BulkyBookWeb.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
            ErrorMessage = "Invalid Email Format")]
        public string Email { get; set; }

    }
}
