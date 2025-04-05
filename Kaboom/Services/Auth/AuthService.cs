using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Kaboom.Interfaces;
using Kaboom.Models;
using Kaboom.Models.Admin;
using Kaboom.Models.AuthUserModel;
using Kaboom.Models.Users;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace Kaboom.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMongoDatabase _mongodatabase;
        private IConfiguration _configuration;
        private readonly IDataBaseService _dataBaseService;
        public AuthService(ApplicationDbContext context, IMongoClient mongoClient, IConfiguration configuration, IDataBaseService dataBaseService)
        {
            _context = context;
            _mongodatabase = mongoClient.GetDatabase("MongoDbConnection");
            _configuration = configuration;
            _dataBaseService = dataBaseService;
        }

        public string GenerateJWTtoken(AuthUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(ClaimTypes.Role,user.Role ??"User"),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecurityKey"]));
            //var key = new SymmetricSecurityKey(Convert.FromBase64String(_configuration["Jwt:SecurityKey"]));
            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires:DateTime.UtcNow.AddMinutes(30),
                signingCredentials:creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken(int userId)
        {
            var userExists = _context.AuthUser.Any(u => u.Id == userId);
            if (!userExists)
                throw new Exception("User not found. Cannot generate refresh token.");
            var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var refreshtokenentity = new RefreshToken
            {
                Token = refreshToken,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                AuthUserId = userId,
                IsRevoked = false
            };
            string provider = _dataBaseService.GetCurrentDatabaseProvider();
            if(provider == DatabaseProviders.Sqlserver)
            {
                _context.RefreshToken.Add(refreshtokenentity);
                _context.SaveChanges();
            }
            else if(provider == DatabaseProviders.MongoDb)
            {
                var collection = _mongodatabase.GetCollection<RefreshToken>("RefreshTokens");
                collection.InsertOne(refreshtokenentity);
            }
            return refreshToken;
        }

        public string Login(string email, string password)
        {
            string provider = _dataBaseService.GetCurrentDatabaseProvider();
            AuthUser user = null;
            if (provider == DatabaseProviders.Sqlserver)
            {
                user = _context.Set<AuthUser>().FirstOrDefault(x => x.Email == email);
            }
            else if(provider == DatabaseProviders.MongoDb)
            {
                var collection = _mongodatabase.GetCollection<AuthUser>("AuthUsers");
                user = collection.Find(u=>u.Email == email).FirstOrDefault();
            }
            if(user == null || !VerifyPassword(password,user.PasswordHash,user.Salt))
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }
            return GenerateJWTtoken(user);
        }

        public bool ValidateRefreshToken(int UserId, string token)
        {
            string provider = _dataBaseService.GetCurrentDatabaseProvider();
            RefreshToken storedToken = null;
            if(provider == DatabaseProviders.Sqlserver)
            {
                storedToken = _context.RefreshToken.FirstOrDefault(rt=>rt.AuthUserId == UserId && rt.Token == token);
            }
            else if(provider == DatabaseProviders.MongoDb)
            {
                var collection = _mongodatabase.GetCollection<RefreshToken>("RefreshTokens");
                storedToken =  collection.Find(rt=>rt.AuthUserId ==UserId && rt.Token == token).FirstOrDefault();    
            }
            if(storedToken == null || storedToken.IsRevoked || storedToken.ExpiryDate < DateTime.UtcNow)
            {
                return false;
            }
            return true;
        }

        public string RegisterUser(AuthUser user, string password)
        {
            var (hashedpassword, salt) = HashPassword(password);
            user.PasswordHash = hashedpassword;
            user.Salt = salt;
            user.PlainTextPassword = password;
            string provider = _dataBaseService.GetCurrentDatabaseProvider();
            AuthUser existingUser = null;
            if (provider == DatabaseProviders.Sqlserver)
            {
                existingUser = _context.AuthUser.FirstOrDefault(x => x.Email == user.Email);
                if (existingUser != null)
                {
                    throw new InvalidOperationException("Email is already in use. Please use a different email.");
                }
                //user.PasswordHash = HashPassword(password);
                var data = new AuthUser
                {
                    Name = user.Name,
                    Email = user.Email,
                    PasswordHash = hashedpassword,
                    Salt = salt,
                    Role = user.Role,
                    RefreshTokens = user.RefreshTokens,
                    ProfileImageUrl = user.ProfileImageUrl,
                    PlainTextPassword = password,

                };
                if (data.Role == null)
                {
                    return "Role is required";
                }
                _context.AuthUser.Add(data);
                 _context.SaveChanges();
                switch (user.Role.ToLower())
                {
                    case "user":
               
                var newUser = new Users
                {
                    UserEmail = user.Email,                
                   UserName = user.Name,
                    PasswordHash = hashedpassword,
                    Role = "User",
                    ProfileImageUrl = user.ProfileImageUrl
                };
                _context.Users.Add(newUser);
                _context.SaveChanges();
                        break;
                    case "admin":
                        var AdminUser = new Admins
                        {
                            Email = user.Email,
                            UserName = data.Name,
                            PasswordHash = hashedpassword,
                            Role = "Admin",
                            ProfileImageUrl = user.ProfileImageUrl
                        };
                        _context.Admins.Add(AdminUser);
                        _context.SaveChanges();
                        break;
                    default:
                        throw new InvalidOperationException("Invalid Role Specified");
                }
                
            }
            
            else if(provider == DatabaseProviders.MongoDb)
            {
                var collection = _mongodatabase.GetCollection<AuthUser>("AuthUsers");
                existingUser = collection.Find(x =>
           x.Email == user.Email || VerifyPassword(password, x.PasswordHash, x.Salt)).FirstOrDefault();
                collection.InsertOneAsync(user);
            }
            if (existingUser != null)
            {
                throw new InvalidOperationException("Email is already in use. Please use a different email.");
            }
            return  "User Registered Successfully";
        }

         private (string hashedPassword, string salt) HashPassword(string password) 
        {
            byte[] saltBytes = new byte[16]; // 16-byte salt
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }

            string salt = Convert.ToBase64String(saltBytes);
            string hashedPassword = Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password: password,
                    salt: saltBytes,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 10000,
                    numBytesRequested: 32
                ));

            return (hashedPassword, salt);
        }
        private bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
        {
            byte[] saltBytes = Convert.FromBase64String(storedSalt);
            string hashedInputPassword = Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password: enteredPassword,
                    salt: saltBytes,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 10000,
                    numBytesRequested: 32
                ));

            return hashedInputPassword == storedHash;
        }

        public AuthUser GetUserByEmail(string email)
        {
            string provider = _dataBaseService.GetCurrentDatabaseProvider();

            if (provider == DatabaseProviders.Sqlserver)
            {
                return _context.AuthUser.Where(x => x.Email == email).FirstOrDefault();
            }
            else if (provider == DatabaseProviders.MongoDb)
            {
                var collection = _mongodatabase.GetCollection<AuthUser>("AuthUsers");
                return collection.Find(u => u.Email == email).FirstOrDefault();
            }

            return null;
        }

        public string Logout(int id)
        {
            string provider = _dataBaseService.GetCurrentDatabaseProvider();
            if(provider == DatabaseProviders.Sqlserver)
            {
                var tokens = _context.RefreshToken.Where(rt => rt.AuthUserId == id).ToList();
                if (tokens.Any())
                {
                    _context.RemoveRange(tokens);
                    _context.SaveChanges();
                }
            }
            else if (provider == DatabaseProviders.MongoDb)
            {
                var collection = _mongodatabase.GetCollection<RefreshToken>("RefreshTokens");
                collection.DeleteMany(rt => rt.AuthUserId == id);
            }
            return "User Logged out scuuessfully";
        }

        public AuthUser UpdateAuthUser(int userId, AuthUser updatedAuthUser)
        {
            if(_dataBaseService.GetCurrentDatabaseProvider() == DatabaseProviders.Sqlserver)
            {
                var authuser = _context.AuthUser.FirstOrDefault(x=>x.Id == userId);
                if (authuser == null)
                {
                    throw new Exception("User not Found");
                }
                if (!string.IsNullOrEmpty(updatedAuthUser.Email))
                {
                    var emailexist = _context.AuthUser.Any(x => x.Email == updatedAuthUser.Email && x.Id != userId);
                    if (emailexist)
                    {
                        throw new Exception("Email is already associated with another account");
                    }
                    authuser.Email = updatedAuthUser.Email;
                }
                if(!string.IsNullOrEmpty(updatedAuthUser.PasswordHash))
                {
                    authuser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updatedAuthUser.PasswordHash);
                }
                if (!string.IsNullOrEmpty(updatedAuthUser.Role))
                {
                    authuser.Role = updatedAuthUser.Role;
                }
                if(!string.IsNullOrEmpty(updatedAuthUser.ProfileImageUrl))
                {
                    authuser.ProfileImageUrl = updatedAuthUser.ProfileImageUrl;
                }
                _context.AuthUser.Update(authuser);
                _context.SaveChanges();
                return authuser;

            }
            else if(_dataBaseService.GetCurrentDatabaseProvider() == DatabaseProviders.MongoDb)
            {

            }
            throw new Exception("Invalid database provider configuration");
        }

        public List<AuthUser> GetAllUser()
        {
            var users = _context.AuthUser
      .Select(user => new AuthUser
      {
          Id = user.Id,
          Email = user.Email ?? "No Email",  // Prevents NULL email issue
          PasswordHash = user.PasswordHash ?? "",
          Role = user.Role ?? "User",  // Default Role if NULL
          ProfileImageUrl = user.ProfileImageUrl ?? "default.png",
          RefreshTokens = user.RefreshTokens ?? new List<RefreshToken>()  // Handle null lists
      })
      .ToList();

            return users;



        }

        public bool DeleteAuthUsers(List<int> userIds)
        {
            var users = _context.AuthUser.Where(u=>userIds.Contains(u.Id)).ToList();    
            if(users ==null ||users.Count == 0)
            {
                return false;
            }
            var refreshtokens = _context.RefreshToken.Where(u=>userIds.Contains(u.AuthUserId)).ToList(); 
            if(refreshtokens.Any())
            {
                _context.RefreshToken.RemoveRange(refreshtokens);
            }
            var adminUsers = _context.Admins.Where(u => userIds.Contains(u.Id)).ToList();
            if (adminUsers.Any())
            {
                _context.Admins.RemoveRange(adminUsers);
            }
            var User = _context.Users.Where(u => userIds.Contains(u.Id)).ToList();
            if (User.Any())
            {
                _context.Users.RemoveRange(User);
            }
            _context.AuthUser.RemoveRange(users);
            _context.SaveChanges(); 
            return true;
        }
    }
}
