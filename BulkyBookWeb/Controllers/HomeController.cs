//using BulkyBookWeb.Models;
using bulkybookweb.efcore;
using BulkyBookWeb.Models;
using BulkyBookWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BulkyBookWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EFCategoryRepository eFCategoryRepository;

        public HomeController(ILogger<HomeController> logger, EFCategoryRepository eFCategoryRepository)
        {
            _logger = logger;
            this.eFCategoryRepository = eFCategoryRepository;
        }

        public async Task<IActionResult> Index()
        {
            var model= await eFCategoryRepository.GetAll();
            CreateCategoryViewModel obj= new CreateCategoryViewModel();

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            CreateCategoryViewModel model = new CreateCategoryViewModel();
            return View("_CategoryModelPartial",model);
        }
        [HttpPost]


        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCategoryViewModel model)
        {

            if (ModelState.IsValid)
            {
                Category newcategory = new Category
                {
                    Name = model.Name,
                    Displayorder = model.Displayorder,
                    // CreatedDateTime = model.CreatedDateTime
                };
                await eFCategoryRepository.Add(newcategory);
                TempData["success"] = "Category Created Successfully";
                return RedirectToAction("Index", new { id = newcategory.Id });
            }
            else
            {
                return View();
            }

        }

        public IActionResult Privacy()
        {
            return View();
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("Error/{statusCode}")]
        public IActionResult httpStatusCodeHandler(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry, the resource you requested could not be found";
                    break;

            }
            //return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            return View("NotFound");
        }
    }
}