//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Diagnostics;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace BulkyBookWeb.Controllers
//{
//    public class ErrorController : Controller
//    {
//        private readonly Logger<ErrorController> logger;

//        public ErrorController(Logger<ErrorController> logger)
//        {
//            this.logger = logger;
//        }
//        [Route("Error")]
//        [AllowAnonymous]
//        public IActionResult Error()
//        {
//            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
//            logger.LogError($"The path {exceptionDetails.Path} threw new exception" +
//                $"{exceptionDetails.Error}");
//            return View("Error");
//        }
//        // GET: ErrorController
//        public ActionResult Index()
//        {
//            return View();
//        }

//        // GET: ErrorController/Details/5
//        public ActionResult Details(int id)
//        {
//            return View();
//        }

//        // GET: ErrorController/Create
//        public ActionResult Create()
//        {
//            return View();
//        }

//        // POST: ErrorController/Create
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create(IFormCollection collection)
//        {
//            try
//            {
//                return RedirectToAction(nameof(Index));
//            }
//            catch
//            {
//                return View();
//            }
//        }

//        // GET: ErrorController/Edit/5
//        public ActionResult Edit(int id)
//        {
//            return View();
//        }

//        // POST: ErrorController/Edit/5
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit(int id, IFormCollection collection)
//        {
//            try
//            {
//                return RedirectToAction(nameof(Index));
//            }
//            catch
//            {
//                return View();
//            }
//        }

//        // GET: ErrorController/Delete/5
//        public ActionResult Delete(int id)
//        {
//            return View();
//        }

//        // POST: ErrorController/Delete/5
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Delete(int id, IFormCollection collection)
//        {
//            try
//            {
//                return RedirectToAction(nameof(Index));
//            }
//            catch
//            {
//                return View();
//            }
//        }
//    }
//}
