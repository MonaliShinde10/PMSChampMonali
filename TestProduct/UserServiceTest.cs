using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using ProductManagement.Models.ViewModel;
using ProductManagement.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace TestProduct
{
    public class UserServiceTest
    {
        [Fact]
        public async Task LoginAsync_ValidCredentials_ReturnsTrue()
        {
            var signInManagerMock = GetMockSignInManager(true);
            var userManagerMock = GetMockUserManager();
            var userService = new UserService(signInManagerMock.Object, userManagerMock.Object);
            var loginModel = new LoginViewModel
            {
                Email = "test@example.com",
                Password = "Password123"
            };
            var result = await userService.LoginAsync(loginModel);
            Assert.True(result);
        }

        [Fact]
        public async Task LoginAsync_InvalidCredentials_ReturnsFalse()
        {
            var signInManagerMock = GetMockSignInManager(false);
            var userManagerMock = GetMockUserManager();
            var userService = new UserService(signInManagerMock.Object, userManagerMock.Object);
            var loginModel = new LoginViewModel
            {
                Email = "test@example.com",
                Password ="invalid#123",
            };
            var result = await userService.LoginAsync(loginModel);
            Assert.False(result);
        }

        [Fact]
        public async Task RegisterAsync_ValidModel_ReturnsTrue()
        {
            var signInManagerMock = GetMockSignInManager(true);
            var userManagerMock = GetMockUserManager();
            var userService = new UserService(signInManagerMock.Object, userManagerMock.Object);

            var registerModel = new RegisterViewModel
            {
                Email = "test@example.com",
                Password = "Password123",
                FirstName = "John",
                LastName = "Doe"
            };

            userManagerMock.Setup(m => m.CreateAsync(It.IsAny<UserModel>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            userManagerMock.Setup(m => m.AddToRoleAsync(It.IsAny<UserModel>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            var result = await userService.RegisterAsync(registerModel);
            Assert.True(result);
        }

        [Fact]
        public async Task RegisterAsync_InvalidModel_ReturnsFalse()
        {
            var signInManagerMock = GetMockSignInManager(true);
            var userManagerMock = GetMockUserManager();
            var userService = new UserService(signInManagerMock.Object, userManagerMock.Object);

            var registerModel = new RegisterViewModel
            {
                Email = "test@example.com",
                Password = "WeakPwd", 
                FirstName = "John",
                LastName = "Doe"
            };

            userManagerMock.Setup(m => m.CreateAsync(It.IsAny<UserModel>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Code = "InvalidPassword", Description = "Password is invalid" }));
            var result = await userService.RegisterAsync(registerModel);

            Assert.False(result);
        }

        [Fact]
        public async Task FindByEmailAsync_ValidEmail_ReturnsUser()
        {
            var signInManagerMock = GetMockSignInManager(true);
            var userManagerMock = GetMockUserManager();
            var userService = new UserService(signInManagerMock.Object, userManagerMock.Object);
            var email = "test@example.com";
            var user = new IdentityUser { Email = email };
            userManagerMock.Setup(m => m.FindByEmailAsync(email))
                .ReturnsAsync(user);

            var result = await userService.FindByEmailAsync(email);

            Assert.NotNull(result);
            Assert.Equal(email, result.Email);
        }

        [Fact]
        public async Task GetRolesAsync_ValidEmail_ReturnsRoles()
        {
            var signInManagerMock = GetMockSignInManager(true);
            var userManagerMock = GetMockUserManager();
            var userService = new UserService(signInManagerMock.Object, userManagerMock.Object);
            var email = "test@example.com";
            var user = new IdentityUser { Email = email };
            var roles = new List<string> { "User", "Admin" };

            userManagerMock.Setup(m => m.FindByEmailAsync(email))
                .ReturnsAsync(user);

            userManagerMock.Setup(m => m.GetRolesAsync(user))
                .ReturnsAsync(roles);

            var result = await userService.GetRolesAsync(email);

            Assert.NotNull(result);
            Assert.Equal(roles, result);
        }

      
        [Fact]
        public async Task LogoutAsync_SignsOutUser()
        {
            var signInManagerMock = GetMockSignInManager(true);
            
            var userService = new UserService(signInManagerMock.Object, null); 
            await userService.LogoutAsync();
            signInManagerMock.Verify(
                sm => sm.SignOutAsync(),
                Times.Once,
                "SignOutAsync should be called once to sign out the user.");
        }

        private static Mock<UserManager<IdentityUser>> GetMockUserManager(bool findUserResult = true)
        {
            var store = new Mock<IUserStore<IdentityUser>>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>(
                store.Object,
                Mock.Of<IOptions<IdentityOptions>>(),
                Mock.Of<IPasswordHasher<IdentityUser>>(),
                new List<IUserValidator<IdentityUser>>(),
                new List<IPasswordValidator<IdentityUser>>(),
                Mock.Of<ILookupNormalizer>(),
                Mock.Of<IdentityErrorDescriber>(),
                null,
                Mock.Of<ILogger<UserManager<IdentityUser>>>()
            );

            userManagerMock.Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(findUserResult ? new IdentityUser() : null);

            return userManagerMock;
        }

        private static Mock<SignInManager<IdentityUser>> GetMockSignInManager(bool signInResult = true)
        {
            var userManagerMock = GetMockUserManager();
            var signInManagerMock = new Mock<SignInManager<IdentityUser>>(
                userManagerMock.Object,
                new HttpContextAccessor(),
                Mock.Of<IUserClaimsPrincipalFactory<IdentityUser>>(),
                null,
                null,
                null,
                null
            );

            signInManagerMock.Setup(m =>
                m.PasswordSignInAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(signInResult ? SignInResult.Success : SignInResult.Failed);

            return signInManagerMock;
        }

 

    }
}
