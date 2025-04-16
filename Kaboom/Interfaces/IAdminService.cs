using Kaboom.Models.Admin;
using Kaboom.Models.Features;
using Kaboom.Models.Order;
using Kaboom.Models.product;
using Kaboom.Models.Users;

namespace Kaboom.Interfaces
{
    public interface IAdminService
    {
        Admins RegisterAdmin(Admins admin);
        Admins GetAdminById(int id);
        Admins GetAdminByEmail(string email);
        List<Users> GetAllUsers();
        List<Products> GetAllProducts();
        List<Orders> GetAllOrders();
        bool UpdateAdminProfile(int id, Admins admin);
        bool DeleteUser(int userId);
        public Users RegisterUserFromAdmin(Users user);
        public List<Admins> GetAllAdmins();
        public PreferenceToggle SetFeature(PreferenceToggle preferences);
        public List<PreferenceToggle> GetFeature();
        public Users RegisterUserFromAdmin(Users user, int adminId);
        public List<Users> GetUsersForAdmin(int adminId);
    }
}
