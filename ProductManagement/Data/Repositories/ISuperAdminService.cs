using ProductManagement.Models.DomainModel;
using ProductManagement.Models.ViewModel;

namespace ProductManagement.Data.Repositories
{
    public interface ISuperAdminService
    {

        SuperAdminDashboardViewModel GetAdminById(Guid id);
        void AddAdmin(AddAdminViewModel admin);
        void EditAdmin(EditAdminViewModel admin);
        void DeleteAdmin(Guid id);
        void DeleteUser(Guid id);
        void EditUser(EditAdminViewModel model);
        List<SuperAdminDashboardViewModel> AllAdmins();
        List<SuperAdminUserModel> UserLists();
        SuperAdminDashboardViewModel GetUserById(Guid id);
      
        IEnumerable<string> GetRoles();


    }
}