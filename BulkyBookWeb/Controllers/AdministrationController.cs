using BulkyBookWeb.ViewModels.Claims;
using bulkybookweb.ViewModels.Administration;
using BulkyBookWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BulkyBookWeb.Controllers
{
    //[Authorize(Roles ="Admin")]
    //[Authorize(Policy= "AdminRolePolicy")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
       

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;

        }
        [HttpGet]
        //[Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> ManageUserClaims(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id ={userId} cannot be found";
                return View("NotFound");
            }
            var existingUserClaims = await userManager.GetClaimsAsync(user);    
            var model = new UserClaimsViewModel
            {
                UserId= userId
            };
            foreach(Claim claim in ClaimsStore.AllClaims)
            {
                UserClaim userClaim = new UserClaim 
                {
                    ClaimType = claim.Type,
                };
                //if the user has the claim,set isselected  property  to true,
                //so the checkbox next to the claim is checked on the UI
                if(existingUserClaims.Any(c => c.Type== claim.Type  && c.Value=="true"))
                {
                    userClaim.IsSelected = true;
                }
                model.Claims.Add(userClaim);
            }
            return View(model);
        }
        [HttpPost]
        //[Authorize(Policy = "EditRolePolicy")]

        public async Task<IActionResult> ManageUserClaims(UserClaimsViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id ={model.UserId} cannot be found";
                return View("NotFound");
            }
            var claims = await userManager.GetClaimsAsync(user);
            var result = await userManager.RemoveClaimsAsync(user, claims);
            if(!result.Succeeded)
            {
                ModelState.AddModelError("","can not remove user existing claims");
                return View(model);
            }
            result = await userManager.AddClaimsAsync(user,
                //model.Claims.Where(c => c.IsSelected).Select(c => new Claim(c.ClaimType,c.ClaimType))) ; we can also use below logic for adding claim type
                model.Claims.Select(c => new Claim(c.ClaimType, c.IsSelected? "true":"false"))) ;
            if(!result.Succeeded)
            {
                ModelState.AddModelError("", "can not add selected claims to user");
                return View(model);
            }
            return RedirectToAction("EditUser" , new { Id= model.UserId});

        }

        [HttpGet]
        [Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> ManageUserRoles(string userId)
        {
            ViewBag.userId= userId;
            var user= await userManager.FindByIdAsync(userId);
            if( user==null)
            {
                ViewBag.ErrorMessage = $"User with Id ={userId} cannot be found";
                return View("NotFound");
            }
            var model = new List<UserRolesViewModel2>();
            foreach(var role in roleManager.Roles)
            {
                var userRolesViewModel = new UserRolesViewModel2 {
                    RoleId=role.Id,
                    RoleName=role.Name
                };
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.IsSelected = true;
                }
                else
                {
                    userRolesViewModel.IsSelected = false;
                }
                model.Add(userRolesViewModel);

            }
            return View(model);
        }
         
        [HttpPost]
        [Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> ManageUserRoles(List<UserRolesViewModel2> model, string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id ={userId} cannot be found";
                return View("NotFound");
            }
            var roles = await userManager.GetRolesAsync(user);
            var result = await userManager.RemoveFromRolesAsync(user,roles);
            if(!result.Succeeded)
            {
                ModelState.AddModelError("","cannot remove user existing roles");
                return View(model);
            }
            result = await userManager.AddToRolesAsync(user,model.Where(x=> x.IsSelected).Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "cannot add selected roles to user");
                return View(model);
            }
            return RedirectToAction("EditUser", new { Id= userId});

        }

            [HttpPost]
        public async Task<IActionResult> DeleteUSer(string id)
        {
            char[] removestring = { 'm', 'e', 't', 'h', 'o', 'd', '=' };
            id = id.TrimEnd(removestring);
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id={id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListUSers");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View("ListUSers");
            }

        }

        [HttpPost]
        [Authorize(Policy ="DeleteRolePolicy")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            //string tempid = id;
            char[] removestring = { 'm', 'e', 't', 'h', 'o', 'd', '=' };
            id = id.TrimEnd(removestring);
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id={id} cannot be found";
                return View("NotFound");
            }
            else
            {
               try
                {
                    var result = await roleManager.DeleteAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListRoles");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View("ListRoles");
                }
                catch (DbUpdateException ex)
                {
                    //logger.LogError($"Error deleting role {ex}");
                    ViewBag.ErrorTitle = $"{ role.Name} role is in use";
                    ViewBag.ErrorMessage = $"{role.Name} role can not be deleted as there are users in this role."+
                        $"If you want to delete this role, please remove users from this role and try to delete again";
                    return View("Error");

                }
            }

        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult ListUsers()
        {
            var users = userManager.Users;
            return View(users); 
        }
        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user=await userManager.FindByIdAsync(id);
          
            if(user == null)
            {
                ViewBag.ErrorMessage = $"User with id = {id} cannot be found";
                return View("NotFound");
            }
            var userClaims= await userManager.GetClaimsAsync(user);
            var userRoles = await userManager.GetRolesAsync(user);
            var model = new EditUserViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                City = user.City,
                Claims = userClaims.Select(c =>c.Type + ": " + c.Value).ToList(),
                Roles= userRoles
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                user.Email = model.Email;
                user.City = model.City;
                user.UserName=model.UserName;
                var result = await userManager.UpdateAsync(user);
                if(result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("",error.Description);
                }
                return View(model);
            }
            
            
        }

        // GET: AdministrationController
        [HttpGet]
       // [AllowAnonymous]
        public async Task<IActionResult> CreateRole()
        {
            return View();
        }
        [HttpPost]
       // [AllowAnonymous]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };
                IdentityResult result = await roleManager.CreateAsync(identityRole);
                if (result.Succeeded)
                {
                    //return RedirectToAction("Index", "Category");
                    return RedirectToAction("ListRoles", "Administration");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
        [HttpGet]
        //[AllowAnonymous]
        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id={id}cannot be found";
                return View("NotFound");
            }
            var model = new EditRoleViewModel
            {
                ID = role.Id,
                RoleName = role.Name
            };
            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    model._Users.Add(user.UserName);
                }
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.ID);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id={model.ID}cannot be found";
                return View("NotFound");
            }
            else
            {
                role.Name = model.RoleName;
                var Result = await roleManager.UpdateAsync(role);
                if (Result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }
                foreach (var error in Result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditUserInRole(string roleid)
        {
            ViewBag.roleid = roleid;
            var role = await roleManager.FindByIdAsync(roleid);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with  id={roleid} cannot be found";
                return View("NotFound");
            }
            var model = new List<UserRoleViewModel>();
            foreach (var user in userManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }
                model.Add(userRoleViewModel);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditUserInRole(List<UserRoleViewModel> model, string roleid)
        {
            var role = await roleManager.FindByIdAsync(roleid);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id = {roleid} cannot be found";
                return View("NotFound");
            }
            for (int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);
                IdentityResult result = null;
                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }
                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                    {
                        continue;
                    }
                    else
                    {
                        return RedirectToAction("EditRole", new { id = roleid });
                    }
                }
            }
            return RedirectToAction("EditRole", new { id = roleid });
        }

        public ActionResult Index()
        {
            return View();
        }

        // GET: AdministrationController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AdministrationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdministrationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdministrationController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdministrationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdministrationController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdministrationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
