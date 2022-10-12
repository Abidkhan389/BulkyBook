using BulkyBookWeb.Models;
using BulkyBookWeb.EFCore.Repository;
namespace BulkyBookWeb.EFCore
{
    public class EFCoreUserRepository : EFRepository<AspNetRoles, BulkyContext>
    {
        public EFCoreUserRepository(BulkyContext context) : base(context)
        {

        }
    }
}
