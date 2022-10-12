using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BulkyBookWeb.Models;
using BulkyBookWeb.ViewModels;
using bulkybookweb.efcore;
using Microsoft.AspNetCore.Authorization;

namespace BulkyBookWeb.Controllers
{
    public class TransactionCategoryModalController : Controller
    {
        private readonly BulkyContext _context;
        private readonly EFCategoryRepository eFCategoryRepository;

        public TransactionCategoryModalController(BulkyContext context, EFCategoryRepository eFCategoryRepository)
        {
            _context = context;
            this.eFCategoryRepository = eFCategoryRepository;
        }

        // GET: TransactionCategoryModal
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var CategoryList = await eFCategoryRepository.GetAll();
            return View(CategoryList);
        }

        // GET: TransactionCategoryModal/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CreateCategoryViewModel == null)
            {
                return NotFound();
            }

            var createCategoryViewModel = await _context.CreateCategoryViewModel
                .FirstOrDefaultAsync(m => m.ID == id);
            if (createCategoryViewModel == null)
            {
                return NotFound();
            }

            return View(createCategoryViewModel);
        }

        // GET: TransactionCategoryModal/Create
        public async Task<IActionResult> AddorEdit(int id=0)
        {
            if(ModelState.IsValid)
            {
                if (id == 0)
                {
                    return View();
                }
                else
                {
                    var result = await eFCategoryRepository.Get(id);
                    CreateCategoryViewModel model = new CreateCategoryViewModel
                    {
                        Name = result.Name,
                        CreatedDateTime = result.CreatedDateTime,
                        Displayorder = result.Displayorder,
                        ID = result.Id

                    };
                    if (model == null)
                    {
                        return NotFound();
                    }
                    return View(model);
                }
                

            }
            return View();
        }

        // POST: TransactionCategoryModal/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Displayorder,CreatedDateTime")] CreateCategoryViewModel createCategoryViewModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(createCategoryViewModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(createCategoryViewModel);
        }

        // GET: TransactionCategoryModal/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CreateCategoryViewModel == null)
            {
                return NotFound();
            }

            var createCategoryViewModel = await _context.CreateCategoryViewModel.FindAsync(id);
            if (createCategoryViewModel == null)
            {
                return NotFound();
            }
            return View(createCategoryViewModel);
        }

        // POST: TransactionCategoryModal/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Displayorder,CreatedDateTime")] CreateCategoryViewModel createCategoryViewModel)
        {
            if (id != createCategoryViewModel.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(createCategoryViewModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CreateCategoryViewModelExists(createCategoryViewModel.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(createCategoryViewModel);
        }

        // GET: TransactionCategoryModal/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CreateCategoryViewModel == null)
            {
                return NotFound();
            }

            var createCategoryViewModel = await _context.CreateCategoryViewModel
                .FirstOrDefaultAsync(m => m.ID == id);
            if (createCategoryViewModel == null)
            {
                return NotFound();
            }

            return View(createCategoryViewModel);
        }

        // POST: TransactionCategoryModal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CreateCategoryViewModel == null)
            {
                return Problem("Entity set 'BulkyContext.CreateCategoryViewModel'  is null.");
            }
            var createCategoryViewModel = await _context.CreateCategoryViewModel.FindAsync(id);
            if (createCategoryViewModel != null)
            {
                _context.CreateCategoryViewModel.Remove(createCategoryViewModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CreateCategoryViewModelExists(int id)
        {
          return (_context.CreateCategoryViewModel?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
