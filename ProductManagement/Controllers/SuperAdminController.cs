using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Data.Repositories;
using ProductManagement.Models.ViewModel;
using ProductManagement.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProductManagement.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class SuperAdminController : Controller
    {
        private readonly ISuperAdminService _superAdminService;

        public SuperAdminController(ISuperAdminService superAdminService)
        {
            _superAdminService = superAdminService;
        }

        public IActionResult SuperAdminDashboard()
        {
            var admins = _superAdminService.AllAdmins();
            return View(admins);
        }

        public IActionResult UserList()
        {
            var user = _superAdminService.UserLists();
            return View(user);
        }

        public IActionResult AddAdmin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddAdmin(AddAdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                _superAdminService.AddAdmin(model); 
                return RedirectToAction("SuperAdminDashboard");
            }

            return View(model);
        }
        public IActionResult EditAdmin(Guid id)
        {
            var admin = _superAdminService.GetAdminById(id);

            if (admin != null)
            {
                var editModel = new EditAdminViewModel
                {
                    Id = id,
                    Email = admin.Email,
                    FirstName = admin.FirstName,
                    LastName = admin.LastName,
                    Role = admin.Role 
                };

                return View(editModel);
            }

            return RedirectToAction("SuperAdminDashboard");
        }

        [HttpPost]
        public IActionResult EditAdmin(EditAdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                var adminToUpdate = new EditAdminViewModel
                {
                    Id = model.Id,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Role = model.Role
                };

                _superAdminService.EditAdmin(adminToUpdate);

                return RedirectToAction("SuperAdminDashboard");
            }

            return View(model);
        }
        public IActionResult EditUser(Guid id)
        {
            var user = _superAdminService.GetUserById(id);

            if (user != null)
            {
                var editModel = new EditAdminViewModel
                {
                    Id = id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = user.Role 
                };

                return View(editModel);
            }

            return RedirectToAction("UserList");
        }

        [HttpPost]
        public IActionResult EditUser(EditAdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                var adminToUpdate = new EditAdminViewModel
                {
                    Id = model.Id,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Role = model.Role
                };

                _superAdminService.EditUser(adminToUpdate);

                return RedirectToAction("UserList");
            }

            return View(model);
        }

        public IActionResult DeleteAdmin(Guid id)
        {
            try
            {
                _superAdminService.DeleteAdmin(id);
                return RedirectToAction("SuperAdminDashboard");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting admin: {ex.Message}");
                return RedirectToAction("SuperAdminDashboard");
            }
        }

        public IActionResult DeleteUser(Guid id)
        {
            try
            {
                _superAdminService.DeleteUser(id);
                return RedirectToAction("UserList");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting user: {ex.Message}");
                return RedirectToAction("UserList");
            }
        }
      
        public IActionResult RoleList() 
        {
            var roles = _superAdminService.GetRoles();
            return View(roles);
        }
    }
}
