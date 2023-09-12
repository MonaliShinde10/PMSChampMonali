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
            var roleManagerMock = GetMockRoleManager();
            var userService = new UserService(signInManagerMock.Object, userManagerMock.Object, roleManagerMock.Object);
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
            var roleManagerMock = GetMockRoleManager();
            var userService = new UserService(signInManagerMock.Object, userManagerMock.Object, roleManagerMock.Object);
            var loginModel = new LoginViewModel
            {
                Email = "test@example.com",
                Password = "InvalidPassword" 
            };
            var result = await userService.LoginAsync(loginModel);
            Assert.False(result);
        }

        [Fact]
        public async Task RegisterAsync_ValidModel_ReturnsTrue()
        {
            var signInManagerMock = GetMockSignInManager();
            var userManagerMock = GetMockUserManager();
            var roleManagerMock = GetMockRoleManager();
            var userService = new UserService(signInManagerMock.Object, userManagerMock.Object, roleManagerMock.Object);
            var registerModel = new RegisterViewModel
            {
                Email = "test@example.com",
                Password = "Password123", 
                FirstName = "John",
                LastName = "Doe"
            };

            var result = await userService.RegisterAsync(registerModel);

            Assert.True(result);
        }

        [Fact]
        public async Task RegisterAsync_InvalidModel_ReturnsFalse()
        {
            // Arrange
            var signInManagerMock = GetMockSignInManager();
            var userManagerMock = GetMockUserManager(false); // User creation will fail
            var roleManagerMock = GetMockRoleManager();
            var userService = new UserService(signInManagerMock.Object, userManagerMock.Object, roleManagerMock.Object);
            var registerModel = new RegisterViewModel
            {
                Email = "test@example.com",
                Password = "InvalidPassword", // Assuming invalid password
                FirstName = "John",
                LastName = "Doe"
            };

            // Act
            var result = await userService.RegisterAsync(registerModel);

            // Assert
            Assert.False(result);
        }
        [Fact]
        public async Task FindByEmailAsync_ValidEmail_ReturnsUser()
        {
            var userManagerMock = GetMockUserManager(true);
            var signInManagerMock = GetMockSignInManager();
            var roleManagerMock = GetMockRoleManager();
            var userService = new UserService(signInManagerMock.Object, userManagerMock.Object, roleManagerMock.Object);

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
            var userManagerMock = GetMockUserManager();
            var signInManagerMock = GetMockSignInManager();
            var roleManagerMock = GetMockRoleManager();
            var userService = new UserService(signInManagerMock.Object, userManagerMock.Object, roleManagerMock.Object);

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
            var signInManagerMock = GetMockSignInManager();
            var userService = new UserService(signInManagerMock.Object, null, null); 
            await userService.LogoutAsync();
            signInManagerMock.Verify(
                sm => sm.SignOutAsync(),
                Times.Once,
                "SignOutAsync should be called once to sign out the user.");
        }

        private Mock<UserManager<IdentityUser>> GetMockUserManager(bool findUserResult = true)
        {
            var store = new Mock<IUserStore<IdentityUser>>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>(
                store.Object,
                Mock.Of<IOptions<IdentityOptions>>(),
                Mock.Of<IPasswordHasher<IdentityUser>>(),
                new IUserValidator<IdentityUser>[0],
                new IPasswordValidator<IdentityUser>[0],
                Mock.Of<ILookupNormalizer>(),
                Mock.Of<IdentityErrorDescriber>(),
                null,
                Mock.Of<ILogger<UserManager<IdentityUser>>>()
            );

            userManagerMock.Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(findUserResult ? new IdentityUser() : null);

            return userManagerMock;
        }

        private Mock<SignInManager<IdentityUser>> GetMockSignInManager(bool signInResult = true)
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

        private Mock<RoleManager<IdentityRole>> GetMockRoleManager()
        {
            var store = new Mock<IRoleStore<IdentityRole>>();
            var roleManagerMock = new Mock<RoleManager<IdentityRole>>(
                store.Object,
                null,
                null,
                null,
                null
            );

            return roleManagerMock;
        }
    }
}
