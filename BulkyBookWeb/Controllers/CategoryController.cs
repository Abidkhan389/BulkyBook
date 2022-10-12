using bulkybookweb.efcore;

using BulkyBookWeb.Models;
using BulkyBookWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly EFCategoryRepository eFCategoryRepository;

        public CategoryController(EFCategoryRepository eFCategoryRepository)
        {
            this.eFCategoryRepository = eFCategoryRepository;
        }
        // GET: CategoryController
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var CategoryList = await eFCategoryRepository.GetAll();
            return View(CategoryList);
        }

        // GET: CategoryController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CategoryController/Create
        [HttpGet]
      
        public ActionResult Create()
        {
            CreateCategoryViewModel model = new CreateCategoryViewModel();
            return View(model);
        }

        // POST: CategoryController/Create
        [HttpPost]
     

        [ValidateAntiForgeryToken]
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

        // GET: CategoryController/Edit/5
        [HttpGet]
     
        public async Task<IActionResult> Edit(int id)
        {
            var obj = await eFCategoryRepository.Get(id);
            CreateCategoryViewModel model = new CreateCategoryViewModel
            {
                ID = obj.Id,
                Displayorder = obj.Displayorder,
                CreatedDateTime = obj.CreatedDateTime,
                Name = obj.Name
            };

            return View(model);
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreateCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {

                Category UpdateCategory = await eFCategoryRepository.Get(model.ID);
                UpdateCategory.Displayorder = model.Displayorder;
                UpdateCategory.CreatedDateTime = model.CreatedDateTime;
                //UpdateCategory.ID = model.ID;
                UpdateCategory.Name = model.Name;
                await eFCategoryRepository.Update(UpdateCategory);
                TempData["success"] = "Category Updated Successfully";
                return RedirectToAction("Index");
            }
            else
                return View();
        }

        // GET: CategoryController/Delete/5
        [HttpGet]
       
        public async Task<IActionResult> Delete(int id)
        {
            if (ModelState.IsValid)
            {
                var obj = await eFCategoryRepository.Get(id);
                CreateCategoryViewModel model = new CreateCategoryViewModel
                {
                    ID = obj.Id,
                    Displayorder = obj.Displayorder,
                    CreatedDateTime = obj.CreatedDateTime,
                    Name = obj.Name
                };
                return View(model);
            }
            else
                return NotFound();
        }

        // POST: CategoryController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
       

        public async Task<IActionResult> DeletePost(int id)
        {
            var obj = await eFCategoryRepository.Get(id);
            if (obj == null)
            {
                return NotFound();
            }
            else
            {
                await eFCategoryRepository.Delete(obj.Id);
                TempData["success"] = "Category deleted Successfully";
                return RedirectToAction("Index");
            }

        }
    }
}
