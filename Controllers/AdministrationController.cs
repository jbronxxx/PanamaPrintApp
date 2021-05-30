using CompanyData.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CompanyData.Controllers
{
    [Authorize(Roles = "Администратор")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager) 
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new()
                {
                    Name = model.RoleName
                };

                var result = await _roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = _roleManager.Roles;

            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Учетная запись с id {id} не найдена.";
                return RedirectToAction("NotFound");
            }

            var model = new EditRoleModel
            {
                ID = role.Id,
                RoleName = role.Name
            };

            foreach (var user in _userManager.Users)
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleModel editRoleModel)
        {
            var role = await _roleManager.FindByIdAsync(editRoleModel.ID);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Учетная запись с id {editRoleModel.ID} не найдена.";
                return RedirectToAction("NotFound");
            }

            else
            {
                role.Name = editRoleModel.RoleName;
                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(editRoleModel);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Учетная запись {role.Name} не найдена.";
                return RedirectToAction("NotFound");
            }
            else
            {
                await _roleManager.DeleteAsync(role);
            }

            return RedirectToAction("ListRoles");
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;

            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Учетная запись с ID {roleId} не найдена.";
                return View("NotFound");
            }

            var model = new List<UsersInRoleModel>();

            foreach (var user in _userManager.Users)
            {
                var userRoleModel = new UsersInRoleModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                    userRoleModel.IsSelected = true;

                else if (await _userManager.IsInRoleAsync(user, role.Name))
                    userRoleModel.IsSelected = true;

                model.Add(userRoleModel);
            }

            return View(model); 
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UsersInRoleModel> model, string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Учетная запись с ID {roleId} не найдена.";
                return View("NotFound");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(model[i].UserId);

                IdentityResult result;

                if (model[i].IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                }

                else if (!model[i].IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                }

                else
                    continue;

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                       return RedirectToAction("EditRole", new { id = roleId });
                }
            }

            return RedirectToAction("EditRole", new { id = roleId });
        }
    }
}
