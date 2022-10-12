

namespace BulkyBookWeb.ViewModels.Claims
{
    public class UserClaimsViewModel
    {
        public UserClaimsViewModel()
        {
            Claims = new List<UserClaim>();
        }
        public List<UserClaim> Claims { get; set; }
        public string UserId { get; set; }
    }
}
