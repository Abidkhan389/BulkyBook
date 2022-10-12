using bulkybookweb.efcore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    public class datatableController : Controller
    {
        private readonly EFCategoryRepository eFCategoryRepository;

        public datatableController(EFCategoryRepository eFCategoryRepository)
        {
            this.eFCategoryRepository = eFCategoryRepository;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var CategoryList = await eFCategoryRepository.GetAll();
            return View(CategoryList);
        }
    }
}
