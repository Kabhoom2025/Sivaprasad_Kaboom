using Kaboom.Interfaces;
using Kaboom.Models;
using Kaboom.Models.Admin;
using Kaboom.Models.AuthUserModel;
using Kaboom.Models.Order;
using Kaboom.Models.product;
using Kaboom.Models.Users;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Kaboom.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _context;
        private readonly IDataBaseService _dataBaseService;
        private readonly IAuthService _authService;
        private readonly IMongoDatabase _mongodatabase;

        public AdminService(ApplicationDbContext context, IDataBaseService dataBaseService, IAuthService authService,IMongoClient mongoClient)
        {
            _context = context;
            _dataBaseService = dataBaseService;
            _authService = authService;
            _mongodatabase = mongoClient.GetDatabase("MongoDbConnection");
        }
        public bool DeleteUser(int userId)
        {
            var user = _context.Users.FirstOrDefault(u=>u.Id == userId);
            if (user == null)
            {
                return false;
            }
            _context.Users.Remove(user);
            _context.SaveChanges();
            return true;
        }

        public Admins GetAdminByEmail(string email)
        {
            return _context.Admins.FirstOrDefault(a => a.Email == email);
        }

        public Admins GetAdminById(int id)
        {
            return _context.Admins.FirstOrDefault(a => a.Id == id);
        }

        public List<Orders> GetAllOrders()
        {
            return _context.Orders.Include(o => o.OrderItems).ToList();
        }

        public List<Products> GetAllProducts()
        {
            return _context.Products.ToList();
        }

        public List<Users> GetAllUsers()
        {
            return _context.Users.ToList();
        }
        public List<Admins> GetAllAdmins()
        {
            return _context.Admins.ToList();
        }
        public Admins RegisterAdmin(Admins admin)
        {
           if(_dataBaseService.GetCurrentDatabaseProvider() == DatabaseProviders.Sqlserver)
            {
                if(_context.Admins.Any(x=>x.Email == admin.Email))
                {
                    throw new Exception("Admin with this email Already Exists");
                }
                admin.PasswordHash = BCrypt.Net.BCrypt.HashPassword(admin.PasswordHash);
                var authAdmin = new AuthUser
                {
                    Name = admin.UserName,
                    Email = admin.Email,
                    PasswordHash = admin.PasswordHash,
                    Role = "Admin",
                    ProfileImageUrl = admin.ProfileImageUrl
                };
                var authuser = _authService.RegisterUser(authAdmin, admin.PasswordHash);
                if(authuser == null)
                {
                    throw new Exception("Invalid Credentials");
                }
                _context.Admins.Add(admin);
                _context.SaveChanges();
                return admin;
            }
           throw new Exception("Invalid Database Provider");
        }

        public bool UpdateAdminProfile(int id, Admins admin)
        {
            var existingadmin = _context.Admins.FirstOrDefault(a=>a.Id == id);
            if(existingadmin == null)
            {
                return false;
            }
            existingadmin.UserName = admin.UserName ?? existingadmin.UserName;
            existingadmin.Email = admin.Email ?? existingadmin.Email;
            existingadmin.Role = admin.Role ?? existingadmin.Role;
            existingadmin.ProfileImageUrl = admin.ProfileImageUrl ?? existingadmin.ProfileImageUrl;
            _context.SaveChanges();
            return true;
        }
        public Users RegisterUserFromAdmin(Users user)
        {
            if (_dataBaseService.GetCurrentDatabaseProvider() == DatabaseProviders.Sqlserver)
            {
                // Check if user already exists
                var existingUser = _context.Users.FirstOrDefault(x => x.UserEmail == user.UserEmail);
                if (existingUser != null)
                {
                    throw new Exception("Email already exists");
                }

                // Hash the password before saving
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);

                // Determine role (default to User if not provided)
                string userRole = string.IsNullOrEmpty(user.Role) ? "User" : user.Role;

                // Create a corresponding AuthUser entry
                var auth = new AuthUser
                {
                    Name = user.UserName,
                    Email = user.UserEmail,
                    PasswordHash = user.PasswordHash,
                    Role = "User",  // Admin or User
                    ProfileImageUrl = user.ProfileImageUrl
                };

                _context.AuthUser.Add(auth);
                _context.SaveChanges();

                // Now create an entry in Users table
                var newUser = new Users
                {
                    UserEmail = user.UserEmail,
                    UserName = user.UserName,
                    PasswordHash = user.PasswordHash,
                    Role = "User",
                    ProfileImageUrl = user.ProfileImageUrl
                };

                _context.Users.Add(newUser);
                _context.SaveChanges();

                return newUser;
            }
            else if (_dataBaseService.GetCurrentDatabaseProvider() == DatabaseProviders.MongoDb)
            {
                var collection = _mongodatabase.GetCollection<Users>("Users");
                var existing = collection.Find(u => u.UserEmail == user.UserEmail).FirstOrDefault();
                if (existing != null)
                    throw new Exception("User already exists");

                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
                collection.InsertOne(user);
                return user;
            }

            return user;
        }

    }
}
