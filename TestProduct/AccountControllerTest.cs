using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Controller;
using Moq;
using ProductManagement.Controllers;
using ProductManagement.Models.ViewModel;
using ProductManagement.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TestProduct
{
    public class AccountControllerTest
    {
        [Fact]
        public void Register_Get_ReturnsViewResult()
        {
            var userServiceMock = new Mock<IUserService>();
            var controller = new AccountController(userServiceMock.Object);

            var result = controller.Register() as ViewResult;
            Assert.NotNull(result);
        }

        [Fact]
        public void Register_InvalidModel_ReturnsViewResult()
        {
            var userServiceMock = new Mock<IUserService>();
            var controller = new AccountController(userServiceMock.Object);
            controller.ModelState.AddModelError("SomeField", "Some error message");

            var result = controller.Register() as ViewResult;

            Assert.NotNull(result);
        }
        #nullable disable
        [Fact]
        public void Register_NullUserService_ReturnsViewResult()
        {
            IUserService userService = null;
            var controller = new AccountController(userService);
            var result = controller.Register() as ViewResult;

            Assert.NotNull(result);
        }

        [Fact]
        public void Login_Get_ReturnsViewResult()
        {
            var userServiceMock = new Mock<IUserService>();
            var controller = new AccountController(userServiceMock.Object);
            var result = controller.Login() as ViewResult;

            Assert.NotNull(result);
        }
        [Fact]
        public void Login_InvalidModel_ReturnsViewResult()
        {
            var userServiceMock = new Mock<IUserService>();
            var controller = new AccountController(userServiceMock.Object);
            controller.ModelState.AddModelError("SomeField", "Some error message");
            var result = controller.Login() as ViewResult;
            Assert.NotNull(result);
        }
        [Fact]
        public void Login_NullUserService_ReturnsViewResult()
        {
            IUserService userService = null;
            var controller = new AccountController(userService);
            var result = controller.Login() as ViewResult;

            Assert.NotNull(result);
        }

        [Fact]
        public void Index_Get_ReturnsViewResult()
        {
            var userServiceMock = new Mock<IUserService>();
            var controller = new AccountController(userServiceMock.Object);
            var result = controller.Index() as ViewResult;

            Assert.NotNull(result);
        }
        [Fact]
        public void Index_InvalidModel_ReturnsViewResult()
        {
            var userServiceMock = new Mock<IUserService>();
            var controller = new AccountController(userServiceMock.Object);
            controller.ModelState.AddModelError("SomeField", "Some error message");
            var result = controller.Index() as ViewResult;
            Assert.NotNull(result);
        }
        [Fact]
        public void Index_NullUserService_ReturnsViewResult()
        {
            IUserService userService = null; 
            var controller = new AccountController(userService);
            var result = controller.Index() as ViewResult;

            Assert.NotNull(result);
        }

        [Fact]
        public void AccessDenied_Get_ReturnsViewResult()
        {
            var userServiceMock = new Mock<IUserService>();
            var controller = new AccountController(userServiceMock.Object);
            var result = controller.AccessDenied() as ViewResult;

            Assert.NotNull(result);
        }
        [Fact]
        public void AccessDenied_InvalidModel_ReturnsViewResult()
        {
            var userServiceMock = new Mock<IUserService>();
            var controller = new AccountController(userServiceMock.Object);
            controller.ModelState.AddModelError("SomeField", "Some error message");
            var result = controller.AccessDenied() as ViewResult;
            Assert.NotNull(result);
        }

        [Fact]
        public void AccessDenied_NullUserService_ReturnsViewResult()
        {
            IUserService userService = null; 
            var controller = new AccountController(userService);
            var result = controller.AccessDenied() as ViewResult;
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Register_ValidModel_ReturnsRedirectToLogin()
        {
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(mock => mock.RegisterAsync(It.IsAny<RegisterViewModel>())).ReturnsAsync(true);
            var controller = new AccountController(userServiceMock.Object);
            var model = new RegisterViewModel();

            var result = await controller.Register(model) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Login", result.ActionName);
        }
        [Fact]
        public async Task Register_InvalidModel_ReturnsViewWithModel()
        {
            var userServiceMock = new Mock<IUserService>();
            var controller = new AccountController(userServiceMock.Object);
            controller.ModelState.AddModelError("ErrorKey", "ErrorMessage");
            var model = new RegisterViewModel();

            var result = await controller.Register(model) as ViewResult;
            Assert.NotNull(result);
            Assert.Equal(model, result.Model);
        }
       

        [Fact]
        public async Task Login_ValidCredentialsAsUser_RedirectsToCorrectDashboard()
        {
            var userServiceMock = new Mock<IUserService>();

            userServiceMock.Setup(service => service.LoginAsync(It.IsAny<LoginViewModel>()))
                .ReturnsAsync(true);

            userServiceMock.Setup(service => service.GetRolesAsync("validemail@example.com"))
                .ReturnsAsync(new string[] { "User" });

            userServiceMock.Setup(service => service.FindByEmailAsync("validemail@example.com"))
                .ReturnsAsync(new UserModel()); 

            var controller = new AccountController(userServiceMock.Object);
            var loginViewModel = new LoginViewModel
            {
                Email = "validemail@example.com",
                Password = "validpassword"
            };
            var result = await controller.Login(loginViewModel) as RedirectToActionResult;
            Assert.NotNull(result);
            Assert.Equal("UserDashboard", result.ActionName);
            Assert.Equal("User", result.ControllerName);
        }
        [Fact]
        public async Task Login_ValidCredentialsAsSuperAdmin_RedirectsToCorrectDashboard()
        {
            var userServiceMock = new Mock<IUserService>();

            userServiceMock.Setup(service => service.LoginAsync(It.IsAny<LoginViewModel>()))
                .ReturnsAsync(true);

            userServiceMock.Setup(service => service.GetRolesAsync("validemail@example.com"))
                .ReturnsAsync(new string[] { "SuperAdmin" });

            userServiceMock.Setup(service => service.FindByEmailAsync("validemail@example.com"))
                .ReturnsAsync(new UserModel());

            var controller = new AccountController(userServiceMock.Object);
            var loginViewModel = new LoginViewModel
            {
                Email = "validemail@example.com",
                Password = "validpassword"
            };

            var result = await controller.Login(loginViewModel) as RedirectToActionResult;
            Assert.NotNull(result);
            Assert.Equal("SuperAdminDashboard", result.ActionName);
            Assert.Equal("SuperAdmin", result.ControllerName);
        }
        [Fact]
        public async Task Login_ValidCredentialsAsAdmin_RedirectsToCorrectDashboard()
        {
            var userServiceMock = new Mock<IUserService>();

            userServiceMock.Setup(service => service.LoginAsync(It.IsAny<LoginViewModel>()))
                .ReturnsAsync(true);

            userServiceMock.Setup(service => service.GetRolesAsync("validemail@example.com"))
                .ReturnsAsync(new string[] { "Admin" });

            userServiceMock.Setup(service => service.FindByEmailAsync("validemail@example.com"))
                .ReturnsAsync(new UserModel());

            var controller = new AccountController(userServiceMock.Object);
            var loginViewModel = new LoginViewModel
            {
                Email = "validemail@example.com",
                Password = "validpassword"
            };

            var result = await controller.Login(loginViewModel) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("AdminDashboard", result.ActionName);
            Assert.Equal("Product", result.ControllerName);
        }
        [Fact]
        public async Task Login_InvalidCredentials_ReturnsViewWithError()
        {
            var userServiceMock = new Mock<IUserService>();

            userServiceMock.Setup(service => service.LoginAsync(It.IsAny<LoginViewModel>()))
                .ReturnsAsync(false);

            var controller = new AccountController(userServiceMock.Object);
            var loginViewModel = new LoginViewModel
            {
                Email = "invalidemail@example.com",
                Password = "invalidpassword"
            };

            var result = await controller.Login(loginViewModel) as ViewResult;
            Assert.NotNull(result);
            Assert.False(controller.ModelState.IsValid);
            Assert.Contains("Invalid login attempt.", controller.ModelState?[""]?.Errors[0]?.ErrorMessage);
        }
        [Fact]
        public async Task Login_InvalidCredentials_ReturnsViewResult()
        {
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(service => service.LoginAsync(It.IsAny<LoginViewModel>()))
                .ReturnsAsync(false); 

            var controller = new AccountController(userServiceMock.Object);
            var loginViewModel = new LoginViewModel
            {
                Email = "invalidemail@example.com",
                Password = "invalidpassword"
            };
            var result = await controller.Login(loginViewModel) as ViewResult;

            Assert.NotNull(result);
        }   
        [Fact]
        public async Task Logout_ReturnsRedirectToAction_Index()
        {
            var userServiceMock = new Mock<IUserService>();
            var signInManagerMock = new Mock<SignInManager<IdentityUser>>();

            var controller = new AccountController(userServiceMock.Object);
            var result = await controller.Logout();
            Assert.IsType<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;

            Assert.Equal("Index", redirectResult?.ActionName);
            Assert.Equal("Account", redirectResult?.ControllerName);
           userServiceMock.Verify(service => service.LogoutAsync(), Times.Once);
        }


        [Fact]
        public async Task Login_InvalidLoginAttempt_ReturnsViewWithModelError()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.LoginAsync(It.IsAny<LoginViewModel>())).ReturnsAsync(false);

            var controller = new AccountController(userServiceMock.Object);
            var model = new LoginViewModel
            {
                Email = "invalid@example.com",
                Password = "invalidpassword"
            };

            // Act
            var result = await controller.Login(model) as ViewResult;

            Assert.NotNull(result);
            Assert.False(controller.ModelState.IsValid);

            // Iterate through error messages and check for the specific error message
            var errorMessages = result.ViewData.ModelState[""].Errors;
            var containsErrorMessage = errorMessages.Any(error => error.ErrorMessage == "Invalid login attempt.");
            Assert.True(containsErrorMessage, "Expected error message not found.");
        }
    }
}
