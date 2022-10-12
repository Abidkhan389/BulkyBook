using BulkyBookWeb.EFCore.Repository;
using BulkyBookWeb.Models;

namespace bulkybookweb.efcore
{
    public class EFCategoryRepository : EFRepository<Category, BulkyContext>
    {
        public EFCategoryRepository(BulkyContext context) : base(context)
        {

        }
    }
}
