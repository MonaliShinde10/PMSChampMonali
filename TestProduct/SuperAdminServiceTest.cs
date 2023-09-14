using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using ProductManagement.Controllers;
using ProductManagement.Data.Repositories;
using ProductManagement.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProduct
{
    public class SuperAdminServiceTest
    {
        [Fact]
        public void AddAdmin_ValidInput_CreatesAdminUser()
        {
            var userManagerMock = GetMockUserManager();
            var roleManagerMock = GetMockRoleManager();

            var service = new SuperAdminService(userManagerMock.Object, roleManagerMock.Object);

            var admin = new AddAdminViewModel
            {
                Email = "newadmin@example.com",
                FirstName = "New",
                LastName = "Admin",
                Password = "Password123",
                Role = "Admin"
            };

            userManagerMock.Setup(m => m.CreateAsync(It.IsAny<UserModel>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            userManagerMock.Setup(m => m.AddToRoleAsync(It.IsAny<UserModel>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            service.AddAdmin(admin);
            userManagerMock.Verify(m => m.CreateAsync(It.IsAny<UserModel>(), It.IsAny<string>()), Times.Once);
            userManagerMock.Verify(m => m.AddToRoleAsync(It.IsAny<UserModel>(), It.IsAny<string>()), Times.Once);
        }
        [Fact]
        public void AddAdmin_RoleAssignmentFailed_LogsErrors()
        {
            var userManagerMock = GetMockUserManager();
            var roleManagerMock = GetMockRoleManager();

            var service = new SuperAdminService(userManagerMock.Object, roleManagerMock.Object);

            var admin = new AddAdminViewModel
            {
                Email = "newadmin@example.com",
                FirstName = "New",
                LastName = "Admin",
                Password = "Password123",
                Role = "Admin"
            };

            userManagerMock.Setup(m => m.CreateAsync(It.IsAny<UserModel>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            userManagerMock.Setup(m => m.AddToRoleAsync(It.IsAny<UserModel>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Role assignment failed" }));

            service.AddAdmin(admin);
        }
        [Fact]
        public void EditAdmin_ValidInput_UserUpdated()
        {
            var userManagerMock = GetMockUserManager();
        
            var roleManagerMock = GetMockRoleManager();

            var service = new SuperAdminService(userManagerMock.Object, roleManagerMock.Object);

            var admin = new EditAdminViewModel
            {
                Id = Guid.NewGuid(),
                Email = "updated@example.com",
                FirstName = "Updated",
                LastName = "Admin",
                Role = "Admin"
            };

            userManagerMock.Setup(m => m.FindByIdAsync(admin.Id.ToString()))
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
            userManagerMock.Verify(m => m.FindByIdAsync(admin.Id.ToString()), Times.Once);
            userManagerMock.Verify(m => m.GetRolesAsync(It.IsAny<UserModel>()), Times.Once);
            userManagerMock.Verify(m => m.RemoveFromRolesAsync(It.IsAny<UserModel>(), It.IsAny<IEnumerable<string>>()), Times.Once);
            userManagerMock.Verify(m => m.AddToRoleAsync(It.IsAny<UserModel>(), It.IsAny<string>()), Times.Once);
            userManagerMock.Verify(m => m.UpdateAsync(It.IsAny<UserModel>()), Times.Once);
        }

        [Fact]
        public void EditAdmin_UserNotFound_ReturnsError()
        {
            var userManagerMock = GetMockUserManager();
            var roleManagerMock = GetMockRoleManager();

            var service = new SuperAdminService(userManagerMock.Object, roleManagerMock.Object);

            var admin = new EditAdminViewModel
            {
                Id = Guid.NewGuid(),
                Email = "updated@example.com",
                FirstName = "Updated",
                LastName = "Admin",
                Role = "Admin"
            };

            userManagerMock.Setup(m => m.FindByIdAsync(admin.Id.ToString()))
                .ReturnsAsync((UserModel)null); 

            service.EditAdmin(admin);
        }
        [Fact]
        public void EditUser_ValidInput_UserUpdated()
        {
            var userManagerMock = GetMockUserManager();
            var roleManagerMock = GetMockRoleManager();

            var service = new SuperAdminService(userManagerMock.Object, roleManagerMock.Object);
            var admin = new EditAdminViewModel
            {
                Id = Guid.NewGuid(),
                Email = "updated@example.com",
                FirstName = "Updated",
                LastName = "User",
                Role = "User"
            };

            userManagerMock.Setup(m => m.FindByIdAsync(admin.Id.ToString()))
                .ReturnsAsync(new UserModel { Id = admin.Id.ToString() });
            userManagerMock.Setup(m => m.GetRolesAsync(It.IsAny<UserModel>()))
                .ReturnsAsync(new List<string>());
            userManagerMock.Setup(m => m.RemoveFromRolesAsync(It.IsAny<UserModel>(), It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(IdentityResult.Success);
            userManagerMock.Setup(m => m.AddToRoleAsync(It.IsAny<UserModel>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            userManagerMock.Setup(m => m.UpdateAsync(It.IsAny<UserModel>()))
                .ReturnsAsync(IdentityResult.Success);

            service.EditUser(admin);

            userManagerMock.Verify(m => m.FindByIdAsync(admin.Id.ToString()), Times.Once);
            userManagerMock.Verify(m => m.GetRolesAsync(It.IsAny<UserModel>()), Times.Once);
            userManagerMock.Verify(m => m.RemoveFromRolesAsync(It.IsAny<UserModel>(), It.IsAny<IEnumerable<string>>()), Times.Once);
            userManagerMock.Verify(m => m.AddToRoleAsync(It.IsAny<UserModel>(), It.IsAny<string>()), Times.Once);
            userManagerMock.Verify(m => m.UpdateAsync(It.IsAny<UserModel>()), Times.Once);
        }

        [Fact]
        public void EditUser_UserNotFound_ReturnsError()
        {
            var userManagerMock = GetMockUserManager();
            var roleManagerMock = GetMockRoleManager();

            var service = new SuperAdminService(userManagerMock.Object, roleManagerMock.Object);
            var admin = new EditAdminViewModel
            {
                Id = Guid.NewGuid(),
                Email = "updated@example.com",
                FirstName = "Updated",
                LastName = "User",
                Role = "User"
            };

            userManagerMock.Setup(m => m.FindByIdAsync(admin.Id.ToString()))
                .ReturnsAsync((UserModel)null); 
                 service.EditUser(admin);

        }

      

        [Fact]
        public void AddAdmin_UserCreationFailed_LogsErrors()
        {
            var userManagerMock = GetMockUserManager();
            var roleManagerMock = GetMockRoleManager();

            var service = new SuperAdminService(userManagerMock.Object, roleManagerMock.Object);
            var admin = new AddAdminViewModel
            {
                Email = "newadmin@example.com",
                FirstName = "New",
                LastName = "Admin",
                Password = "Password123",
                Role = "Admin"
            };

            userManagerMock.Setup(m => m.CreateAsync(It.IsAny<UserModel>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "User creation failed" }));

            service.AddAdmin(admin);

          
        }
        [Fact]
        public void DeleteUser_ValidUser_DeletedSuccessfully()
        {
            var userManagerMock = GetMockUserManager();
            var roleManagerMock = GetMockRoleManager();

            var service = new SuperAdminService(userManagerMock.Object, roleManagerMock.Object);
            var userId = Guid.NewGuid();
            userManagerMock.Setup(m => m.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(new UserModel { Id = userId.ToString() });
            userManagerMock.Setup(m => m.DeleteAsync(It.IsAny<UserModel>()))
                .ReturnsAsync(IdentityResult.Success);
            service.DeleteUser(userId);
            userManagerMock.Verify(m => m.FindByIdAsync(userId.ToString()), Times.Once);
            userManagerMock.Verify(m => m.DeleteAsync(It.IsAny<UserModel>()), Times.Once);
        }

        [Fact]
        public void DeleteUser_UserNotFound_ReturnsError()
        {
            var userManagerMock = GetMockUserManager();
            var roleManagerMock = GetMockRoleManager();

            var service = new SuperAdminService(userManagerMock.Object, roleManagerMock.Object);
            var userId = Guid.NewGuid();
            userManagerMock.Setup(m => m.FindByIdAsync(userId.ToString()))
                .ReturnsAsync((UserModel)null); 
            service.DeleteUser(userId);
        }

        [Fact]
        public void DeleteUser_UserDeletionFailed_LogsErrors()
        {
            var userManagerMock = GetMockUserManager();
            var roleManagerMock = GetMockRoleManager();

            var service = new SuperAdminService(userManagerMock.Object, roleManagerMock.Object);
            var userId = Guid.NewGuid();
            userManagerMock.Setup(m => m.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(new UserModel { Id = userId.ToString() });
            userManagerMock.Setup(m => m.DeleteAsync(It.IsAny<UserModel>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "User deletion failed" }));
            service.DeleteUser(userId);

        }
        [Fact]
        public void DeleteAdmin_ValidUser_DeletesSuccessfully()
        {
            var userManagerMock = GetMockUserManager();
            var roleManagerMock = GetMockRoleManager();

            var service = new SuperAdminService(userManagerMock.Object, roleManagerMock.Object);
            var userId = Guid.NewGuid();
            userManagerMock.Setup(m => m.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(new UserModel { Id = userId.ToString() });
            userManagerMock.Setup(m => m.DeleteAsync(It.IsAny<UserModel>()))
                .ReturnsAsync(IdentityResult.Success);

            service.DeleteAdmin(userId);

            userManagerMock.Verify(m => m.FindByIdAsync(userId.ToString()), Times.Once);
            userManagerMock.Verify(m => m.DeleteAsync(It.IsAny<UserModel>()), Times.Once);
        }
        [Fact]
        public void DeleteAdmin_UserNotFound_ReturnsError()
        {
            var userManagerMock = GetMockUserManager();
            var roleManagerMock = GetMockRoleManager();

            var service = new SuperAdminService(userManagerMock.Object, roleManagerMock.Object);
            var userId = Guid.NewGuid();
            userManagerMock.Setup(m => m.FindByIdAsync(userId.ToString()))
                .ReturnsAsync((UserModel)null);

            service.DeleteAdmin(userId);
        }
        [Fact]
        public void DeleteAdmin_UserDeletionFailed_LogsErrors()
        {
            var userManagerMock = GetMockUserManager();
            var roleManagerMock = GetMockRoleManager();

            var service = new SuperAdminService(userManagerMock.Object, roleManagerMock.Object);
            var userId = Guid.NewGuid();
            userManagerMock.Setup(m => m.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(new UserModel { Id = userId.ToString() });
            userManagerMock.Setup(m => m.DeleteAsync(It.IsAny<UserModel>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "User deletion failed" }));

            service.DeleteAdmin(userId);
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
        public void RoleList_InvalidInput_ReturnsEmptyList()
        {
            var mockSuperAdminService = new Mock<ISuperAdminService>();
            var controller = new SuperAdminController(mockSuperAdminService.Object);
            mockSuperAdminService.Setup(service => service.GetRoles()).Returns(new List<string>());

            var result = controller.RoleList() as ViewResult;

            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            Assert.Null(result.ViewName);
            Assert.IsType<List<string>>(result.Model);

            var model = Assert.IsAssignableFrom<List<string>>(result.Model);
            Assert.Empty(model);
        }

        [Fact]
        public void EditAdmin_InvalidInput_DoesNotUpdateUserOrRole()
        {
            var userManagerMock = GetMockUserManager();
            var roleManagerMock = GetMockRoleManager();

            var service = new SuperAdminService(userManagerMock.Object, roleManagerMock.Object);
            var admin = new EditAdminViewModel
            {
                Id = Guid.NewGuid(),
                Email = "updated@example.com",
                FirstName = "Updated",
                LastName = "Admin",
                Role = "Admin"
            };

            userManagerMock.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((UserModel)null);

            service.EditAdmin(admin);

            userManagerMock.Verify(m => m.FindByIdAsync(It.IsAny<string>()), Times.Once);
            userManagerMock.Verify(m => m.GetRolesAsync(It.IsAny<UserModel>()), Times.Never);
            userManagerMock.Verify(m => m.RemoveFromRolesAsync(It.IsAny<UserModel>(), It.IsAny<IEnumerable<string>>()), Times.Never);
            userManagerMock.Verify(m => m.AddToRoleAsync(It.IsAny<UserModel>(), It.IsAny<string>()), Times.Never);
            userManagerMock.Verify(m => m.UpdateAsync(It.IsAny<UserModel>()), Times.Never);
        }
       

        [Fact]
        public void AllAdmins_InvalidRole_ReturnsEmptyList()
        {
            var userManagerMock = GetMockUserManager();
            var roleManagerMock = GetMockRoleManager();

            var service = new SuperAdminService(userManagerMock.Object, roleManagerMock.Object);
            var result = service.AllAdmins();

            Assert.NotNull(result);
            Assert.IsType<List<SuperAdminDashboardViewModel>>(result);
            Assert.Empty(result);
        }


        [Fact]
        public void EditAdmin_ValidInput_UpdatesUserAndRole()
        {
            var userManagerMock = GetMockUserManager();
            var roleManagerMock = GetMockRoleManager();

            var service = new SuperAdminService(userManagerMock.Object, roleManagerMock.Object);
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

        private static Mock<UserManager<IdentityUser>> GetMockUserManager()
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

      



        private static Mock<RoleManager<IdentityRole>> GetMockRoleManager()
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
    }
}
