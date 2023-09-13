using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using ProductManagement.Controllers;
using ProductManagement.Data.Repositories;
using ProductManagement.Models.DomainModel;
using ProductManagement.Models.ViewModel;
using ProductManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProduct
{
    public class SuperAdminControllerTest
    {
        [Fact]
        public void EditUser_ValidModel_RedirectsToUserList()
        {
            var adminServiceMock = new Mock<ISuperAdminService>();
            var controller = new SuperAdminController(adminServiceMock.Object);

            var model = new EditAdminViewModel
            {
                Id = Guid.NewGuid(), 
                Email = "newemail@example.com",
                FirstName = "NewFirstName",
                LastName = "NewLastName",
                Role = "NewRole"
            };

            var result = controller.EditUser(model) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("UserList", result.ActionName);

            adminServiceMock.Verify(service => service.EditUser(It.IsAny<EditAdminViewModel>()), Times.Once);
        }

        [Fact]
        public void EditAdmin_ValidModel_RedirectsToUserList()
        {
            var adminServiceMock = new Mock<ISuperAdminService>();
            var controller = new SuperAdminController(adminServiceMock.Object);

            var model = new EditAdminViewModel
            {
                Id = Guid.NewGuid(),
                Email = "newemail@example.com",
                FirstName = "NewFirstName",
                LastName = "NewLastName",
                Role = "NewRole"
            };

            var result = controller.EditAdmin(model) as RedirectToActionResult;
            Assert.NotNull(result);
            Assert.Equal("SuperAdminDashboard", result.ActionName);

            adminServiceMock.Verify(service => service.EditAdmin(It.IsAny<EditAdminViewModel>()), Times.Once);
        }
        [Fact]
        public void TestAddAdmin()
        {
            var superAdminServiceMock = new Mock<ISuperAdminService>();
            var superAdminController = new SuperAdminController(superAdminServiceMock.Object);

            var adminToAdd = new AddAdminViewModel
            {
                Email = "admin@example.com",
                FirstName = "John",
                LastName = "Doe",
                Password = "password",
                Role = "Admin"
            };

            superAdminServiceMock.Setup(service => service.AddAdmin(It.IsAny<AddAdminViewModel>()))
                .Verifiable();
            var result = superAdminController.AddAdmin(adminToAdd) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("SuperAdminDashboard", result.ActionName);

            superAdminServiceMock.Verify(service => service.AddAdmin(adminToAdd), Times.Once);
        }

        [Fact]
        public void TestSuperAdminDashboard()
        {
            var superAdminServiceMock = new Mock<ISuperAdminService>();
            var superAdminController = new SuperAdminController(superAdminServiceMock.Object);

            var admins = new List<SuperAdminDashboardViewModel>
            {
                new SuperAdminDashboardViewModel
                {
                    Id = "sjkdnfhue8223629283",
                    Email = "admin1@example.com",
                    FirstName = "John",
                    LastName = "Doe",
                    Role = "Admin"
                },
                new SuperAdminDashboardViewModel
                {
                    Id = "sjidefmkcjdkfjijs6236sa",
                    Email = "admin2@example.com",
                    FirstName = "Jane",
                    LastName = "Smith",
                    Role = "Admin"
                }
            };

            superAdminServiceMock.Setup(service => service.AllAdmins())
                .Returns(admins);
            var result = superAdminController.SuperAdminDashboard() as ViewResult;

            Assert.NotNull(result);
            Assert.Equal(admins, result.Model);
            Assert.Null(result.ViewName);
        }
        [Fact]
        public void UserList_ReturnsViewWithUserList()
        {
            var mockSuperAdminService = new Mock<ISuperAdminService>();
            var expectedUserList = new List<SuperAdminUserModel>
            {
                new SuperAdminUserModel { Id = "1", Email = "user1@example.com", FirstName = "John", LastName = "Doe", Role = "User" },
                new SuperAdminUserModel { Id = "2", Email = "user2@example.com", FirstName = "Jane", LastName = "Smith", Role = "User" }
            };

            mockSuperAdminService.Setup(service => service.UserLists()).Returns(expectedUserList);
            var controller = new SuperAdminController(mockSuperAdminService.Object);

            var result = controller.UserList() as ViewResult;
            Assert.NotNull(result);
            Assert.Equal(expectedUserList, result.Model); 
        }

        [Fact]
        public void TestDeleteAdmin()
        {
            var superAdminServiceMock = new Mock<ISuperAdminService>();
            var superAdminController = new SuperAdminController(superAdminServiceMock.Object);

            var adminId = Guid.NewGuid();

            superAdminServiceMock.Setup(service => service.DeleteAdmin(adminId))
                .Verifiable();
            var result = superAdminController.DeleteAdmin(adminId) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("SuperAdminDashboard", result.ActionName);

            superAdminServiceMock.Verify(service => service.DeleteAdmin(adminId), Times.Once);
        }


        [Fact]
        public void TestDeleteUser()
        {
            var superAdminServiceMock = new Mock<ISuperAdminService>();
            var superAdminController = new SuperAdminController(superAdminServiceMock.Object);

            var userId = Guid.NewGuid();

            superAdminServiceMock.Setup(service => service.DeleteUser(userId)) 
                .Verifiable();

            var result = superAdminController.DeleteUser(userId) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("UserList", result.ActionName);

            superAdminServiceMock.Verify(service => service.DeleteUser(userId), Times.Once);
        }

      
        [Fact]
        public void TestEditUser_InvalidModel()
        {
            var superAdminServiceMock = new Mock<ISuperAdminService>();
            var superAdminController = new SuperAdminController(superAdminServiceMock.Object);

            var model = new EditAdminViewModel
            {
                Id = Guid.NewGuid(),
                Email = "newemail@example.com",
                FirstName = "NewFirstName",
                LastName = "NewLastName",
                Role = "NewRole"
            };

            superAdminServiceMock.Setup(service => service.EditUser(It.IsAny<EditAdminViewModel>()))
                .Verifiable();

            superAdminController.ModelState.AddModelError("Key", "ErrorMessage"); 

            var result = superAdminController.EditUser(model) as ViewResult;
            Assert.NotNull(result);
            Assert.Equal(model, result.Model); 

            superAdminServiceMock.Verify(service => service.EditUser(It.IsAny<EditAdminViewModel>()), Times.Never);
        }
        [Fact]
        public void TestEditAdmin_InvalidModel()
        {
            var superAdminServiceMock = new Mock<ISuperAdminService>();
            var superAdminController = new SuperAdminController(superAdminServiceMock.Object);

            var model = new EditAdminViewModel
            {
                Id = Guid.NewGuid(),
                Email = "newemail@example.com",
                FirstName = "NewFirstName",
                LastName = "NewLastName",
                Role = "NewRole"
            };

            superAdminServiceMock.Setup(service => service.EditAdmin(It.IsAny<EditAdminViewModel>()))
                .Verifiable();

            superAdminController.ModelState.AddModelError("Key", "ErrorMessage"); 
            var result = superAdminController.EditAdmin(model) as ViewResult;
            Assert.NotNull(result);
            Assert.Equal(model, result.Model);
            superAdminServiceMock.Verify(service => service.EditAdmin(It.IsAny<EditAdminViewModel>()), Times.Never);
        }
        [Fact]
        public void RoleList_ReturnsViewWithExpectedModel()
        {
            var mockSuperAdminService = new Mock<ISuperAdminService>();
            var controller = new SuperAdminController(mockSuperAdminService.Object);
            var expectedRoles = new List<string>
            {
                "Admin",
                "User",
                "Manager"
            };
            mockSuperAdminService.Setup(service => service.GetRoles()).Returns(expectedRoles);
            var result = controller.RoleList() as ViewResult;
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            Assert.Null(result.ViewName);
            Assert.IsType<List<string>>(result.Model);

            var model = Assert.IsAssignableFrom<List<string>>(result.Model);
            Assert.Equal(expectedRoles, model);
        }

      
        [Fact]
        public void EditAdmin_ValidInput_UpdatesUserAndRole()
        {
            var userManagerMock = GetMockUserManager();
            var signInManagerMock = GetMockSignInManager(userManagerMock.Object);
            var roleManagerMock = GetMockRoleManager();

            var service = new SuperAdminService(userManagerMock.Object, signInManagerMock.Object, roleManagerMock.Object);

            var admin = new EditAdminViewModel
            {
                Id = Guid.NewGuid(),
                Email = "updated@example.com",
                FirstName = "Updated",
                LastName = "Admin",
                Role = "Admin"
            };

            userManagerMock.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new UserModel { Id = admin.Id.ToString() });
            userManagerMock.Setup(m => m.GetRolesAsync(It.IsAny<UserModel>()))
                .ReturnsAsync(new List<string>());
            userManagerMock.Setup(m => m.RemoveFromRolesAsync(It.IsAny<UserModel>(), It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(IdentityResult.Success);
            userManagerMock.Setup(m => m.AddToRoleAsync(It.IsAny<UserModel>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            userManagerMock.Setup(m => m.UpdateAsync(It.IsAny<UserModel>()))
                .ReturnsAsync(IdentityResult.Success);
            service.EditAdmin(admin);

            userManagerMock.Verify(m => m.FindByIdAsync(It.IsAny<string>()), Times.Once);
            userManagerMock.Verify(m => m.GetRolesAsync(It.IsAny<UserModel>()), Times.Once);
            userManagerMock.Verify(m => m.RemoveFromRolesAsync(It.IsAny<UserModel>(), It.IsAny<IEnumerable<string>>()), Times.Once);
            userManagerMock.Verify(m => m.AddToRoleAsync(It.IsAny<UserModel>(), It.IsAny<string>()), Times.Once);
            userManagerMock.Verify(m => m.UpdateAsync(It.IsAny<UserModel>()), Times.Once);
        }


        #region Mock Helper Methods

        private Mock<UserManager<IdentityUser>> GetMockUserManager()
        {
            var userStoreMock = new Mock<IUserStore<IdentityUser>>();
            var optionsAccessorMock = new Mock<IOptions<IdentityOptions>>();
            var passwordHasherMock = new Mock<IPasswordHasher<IdentityUser>>();
            var userValidators = new List<IUserValidator<IdentityUser>>();
            var passwordValidators = new List<IPasswordValidator<IdentityUser>>();
            var keyNormalizerMock = new Mock<ILookupNormalizer>();
            var errorsMock = new Mock<IdentityErrorDescriber>();
            var servicesMock = new Mock<IServiceProvider>();
            var loggerMock = new Mock<ILogger<UserManager<IdentityUser>>>();

            return new Mock<UserManager<IdentityUser>>(
                userStoreMock.Object,
                optionsAccessorMock.Object,
                passwordHasherMock.Object,
                userValidators,
                passwordValidators,
                keyNormalizerMock.Object,
                errorsMock.Object,
                servicesMock.Object,
                loggerMock.Object);
        }

        private Mock<SignInManager<IdentityUser>> GetMockSignInManager(UserManager<IdentityUser> userManager)
        {
            var contextAccessorMock = new Mock<IHttpContextAccessor>();
            var claimsPrincipalFactoryMock = new Mock<IUserClaimsPrincipalFactory<IdentityUser>>();
            var optionsAccessorMock = new Mock<IOptions<IdentityOptions>>();
            var loggerMock = new Mock<ILogger<SignInManager<IdentityUser>>>();
            var authenticationSchemeProviderMock = new Mock<IAuthenticationSchemeProvider>();

            return new Mock<SignInManager<IdentityUser>>(
                userManager,
                contextAccessorMock.Object,
                claimsPrincipalFactoryMock.Object,
                optionsAccessorMock.Object,
                loggerMock.Object,
                authenticationSchemeProviderMock.Object,
                null); 
        }



        private Mock<RoleManager<IdentityRole>> GetMockRoleManager()
        {
            var roleStoreMock = new Mock<IRoleStore<IdentityRole>>();
            var roleValidators = new List<IRoleValidator<IdentityRole>>();
            var keyNormalizerMock = new Mock<ILookupNormalizer>();
            var errorsMock = new Mock<IdentityErrorDescriber>();
            var loggerMock = new Mock<ILogger<RoleManager<IdentityRole>>>();

            return new Mock<RoleManager<IdentityRole>>(
                roleStoreMock.Object,
                roleValidators,
                keyNormalizerMock.Object,
                errorsMock.Object,
                loggerMock.Object);
        }

        #endregion

        [Fact]
        public void SuperAdminDashboard_Get_ReturnsViewResult()
        {
            var superAdminServiceMock = new Mock<ISuperAdminService>();
            var controller = new SuperAdminController(superAdminServiceMock.Object);
            var result = controller.SuperAdminDashboard() as ViewResult;
            Assert.NotNull(result);
        }

        [Fact]
        public void UserList_Get_ReturnsViewResult()
        {
            var superAdminServiceMock = new Mock<ISuperAdminService>();
            var controller = new SuperAdminController(superAdminServiceMock.Object);

            var result = controller.UserList() as ViewResult;

            Assert.NotNull(result);
        }

        [Fact]
        public void AddAdmin_Get_ReturnsViewResult()
        {
            var superAdminServiceMock = new Mock<ISuperAdminService>();
            var controller = new SuperAdminController(superAdminServiceMock.Object);
            var result = controller.AddAdmin() as ViewResult;

            Assert.NotNull(result);
        }

        [Fact]
        public void RoleList_Get_ReturnsViewResult()
        {
            var superAdminServiceMock = new Mock<ISuperAdminService>();
            var controller = new SuperAdminController(superAdminServiceMock.Object);
            var result = controller.RoleList() as ViewResult;
            Assert.NotNull(result);
        }
        [Fact]
        public void SuperAdminDashboard_ReturnsView_WithSuperAdminDashboardViewModel()
        {
            var superAdminServiceMock = new Mock<ISuperAdminService>();
            superAdminServiceMock.Setup(service => service.AllAdmins())
                .Returns(new List<SuperAdminDashboardViewModel>
                {
            new SuperAdminDashboardViewModel
            {
                Id = Guid.NewGuid().ToString(),
                Email = "admin@example.com",
                FirstName = "John",
                LastName = "Doe",
                Role = "SuperAdmin"
            }
                });

            var controller = new SuperAdminController(superAdminServiceMock.Object);
            controller.ControllerContext = new ControllerContext();
            var result = controller.SuperAdminDashboard() as ViewResult;

            Assert.NotNull(result);
            Assert.IsType<List<SuperAdminDashboardViewModel>>(result.Model);

            var model = result.Model as List<SuperAdminDashboardViewModel>;
            Assert.NotNull(model);
            Assert.Single(model); 
        }


    }
}
