using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Differencing;
using ProductManagement.Models.DomainModel;
using ProductManagement.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductManagement.Data.Repositories
{
    public class SuperAdminService : ISuperAdminService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SuperAdminService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void AddAdmin(AddAdminViewModel admin)
        {
            var userModel = new UserModel
            {
                UserName = admin.Email,
                Email = admin.Email,
                FirstName = admin.FirstName,
                LastName = admin.LastName,
            };

            var result = _userManager.CreateAsync(userModel, admin.Password).Result;

            if (result.Succeeded)
            {
                _userManager.AddToRoleAsync(userModel, admin.Role).Wait();
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"Error: {error.Description}");
                }
            }
        }
        public void EditAdmin(EditAdminViewModel admin)
        {
            var user = _userManager.FindByIdAsync(admin.Id.ToString()).Result;

            if (user != null)
            {
                user.Email = admin.Email;
                ((UserModel)user).FirstName = admin.FirstName;
                ((UserModel)user).LastName = admin.LastName;

                var currentRoles = _userManager.GetRolesAsync(user).Result;
                if (!currentRoles.Contains(admin.Role))
                {
                    _userManager.RemoveFromRolesAsync(user, currentRoles).Wait();
                    _userManager.AddToRoleAsync(user, admin.Role).Wait();
                }

                var result = _userManager.UpdateAsync(user).Result;

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Error: {error.Description}");
                    }
                }
            }
            else
            {
                Console.WriteLine("User not found.");
            }
        }
        public void EditUser(EditAdminViewModel admin)
        {
            var user = _userManager.FindByIdAsync(admin.Id.ToString()).Result;

            if (user != null)
            {
                user.Email = admin.Email;
                ((UserModel)user).FirstName = admin.FirstName;
                ((UserModel)user).LastName = admin.LastName;

                var currentRoles = _userManager.GetRolesAsync(user).Result;
                if (!currentRoles.Contains(admin.Role))
                {
                    _userManager.RemoveFromRolesAsync(user, currentRoles).Wait();
                    _userManager.AddToRoleAsync(user, admin.Role).Wait();
                }

                var result = _userManager.UpdateAsync(user).Result;

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Error: {error.Description}");
                    }
                }
            }
            else
            {
                Console.WriteLine("User not found.");
            }
        }


        public void DeleteAdmin(Guid id)
        {
            var user = _userManager.FindByIdAsync(id.ToString()).Result;

            if (user != null)
            {
                var result = _userManager.DeleteAsync(user).Result;

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Error: {error.Description}");
                    }
                }
            }
            else
            {
                Console.WriteLine("User not found.");
            }
        }
        public void DeleteUser(Guid id)
        {
            var user = _userManager.FindByIdAsync(id.ToString()).Result;

            if (user != null)
            {
                var result = _userManager.DeleteAsync(user).Result;

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Error: {error.Description}");
                    }
                }
            }
            else
            {
                Console.WriteLine("User not found.");
            }
        }
    

    
    public List<SuperAdminDashboardViewModel> AllAdmins()
        {
            var admins = _userManager.GetUsersInRoleAsync("Admin").Result;

            if (admins == null)
            {
                return new List<SuperAdminDashboardViewModel>();
            }

            var adminList = admins.Select(user =>
            {
                if (user is UserModel userModel)
                {
                    return new SuperAdminDashboardViewModel
                    {
                        Id = userModel.Id,
                        Email = userModel.Email,
                        FirstName = userModel.FirstName,
                        LastName = userModel.LastName,
                        Role = string.Join(", ", _userManager.GetRolesAsync(user).Result)
                    };
                }
                else
                {
                    return null;
                }
            }).Where(adminViewModel => adminViewModel != null).ToList();

            return adminList;
        }


        public List<SuperAdminUserModel> UserLists()
        {
            var admins = _userManager.GetUsersInRoleAsync("User").Result;

            var adminList = admins
                .OfType<UserModel>() 
                .Select(user => new SuperAdminUserModel
                {
                    Id = user.Id,
                    Email = user.Email ?? "Unknown Email",
                    FirstName = user.FirstName ?? "Unknown FirstName",
                    LastName = user.LastName ?? "Unknown LastName",
                    Role = string.Join(", ", _userManager.GetRolesAsync(user).Result)
                })
                .ToList();

            return adminList;
        }

        public SuperAdminDashboardViewModel GetAdminById(Guid id)
        {
            var user = _userManager.FindByIdAsync(id.ToString()).Result;

            if (user != null)
            {
                if (user is UserModel userModel)
                {
                    return new SuperAdminDashboardViewModel
                    {
                        Id = userModel.Id,
                        Email = userModel.Email,
                        FirstName = userModel.FirstName,
                        LastName = userModel.LastName,
                        Role = string.Join(", ", _userManager.GetRolesAsync(user).Result)
                    };
                }
                else
                {
                    return null;
                }
            }

            return null;
        }

        public SuperAdminDashboardViewModel GetUserById(Guid id)
        {
            var user = _userManager.FindByIdAsync(id.ToString()).Result;

            if (user != null)
            {
                if (user is UserModel userModel)
                {
                    return new SuperAdminDashboardViewModel
                    {
                        Id = userModel.Id,
                        Email = userModel.Email,
                        FirstName = userModel.FirstName,
                        LastName = userModel.LastName,
                        Role = string.Join(", ", _userManager.GetRolesAsync(user).Result)
                    };
                }
                else
                {
                    return null;
                }
            }

            return null;
        }

      
        public IEnumerable<string> GetRoles()
        {
            return _roleManager.Roles.Select(r => r.Name);
        }
    }
}
