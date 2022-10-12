using bulkybookweb.efcore;
using BulkyBookWeb.Models;
using BulkyBookWeb.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BulkyBookWeb.Controllers
{
    public class ManageModalCategoryController : Controller
    {
        private readonly EFCategoryRepository eFCategoryRepository;

        public ManageModalCategoryController(EFCategoryRepository eFCategoryRepository)
        {
            this.eFCategoryRepository = eFCategoryRepository;
        }
        // GET: ManageModalCategory
        public async Task<IActionResult> Index()
        {
            var model= await eFCategoryRepository.GetAll();

            return View(model);
        }

        // GET: Transaction/AddOrEdit(Insert)
        // GET: Transaction/AddOrEdit/5(Update)
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                CreateCategoryViewModel model = new CreateCategoryViewModel();
                return View(model);
            }
                
            else
            {
                var transactionModel = await eFCategoryRepository.Get(id);
                if (transactionModel == null)
                {
                    return NotFound();
                }
                return View(transactionModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, [Bind("ID,Name,Displayorder,CreatedDateTime")] CreateCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                //Insert
                if (id == 0)
                {
                    Category newcategory = new Category
                    {
                        CreatedDateTime = DateTime.Now,
                        Name = model.Name,
                        Displayorder = model.Displayorder
                    };
                    var obj = await eFCategoryRepository.Add(newcategory);


                }
                //Update
                else
                {
                    try
                    {
                        Category editcateogry = await eFCategoryRepository.Get(id);
                        editcateogry.Name = model.Name;
                        editcateogry.Displayorder = model.Displayorder;
                        editcateogry.CreatedDateTime = model.CreatedDateTime;
                        await eFCategoryRepository.Update(editcateogry);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "Index", eFCategoryRepository.GetAll()) });
            }
            //return null;
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit",model ) });
        }

        // POST: Transaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Model =await eFCategoryRepository.Get(id);
            if (Model == null)
                return BadRequest();
            else
            {
                Model = await eFCategoryRepository.Delete(id);
            }
            return Json(new { html = Helper.RenderRazorViewToString(this, "_ViewAll", eFCategoryRepository.GetAll()) });
        }

    }
    
}
