using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using One.MDM.Authentication.Controllers;
using One.MDM.Authentication.Model;
using One.MDM.Authentication.Services;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace One.MDM.Authentication.Models
{
    public class AuthService: IAuthService
    {
        private readonly IConfiguration _config;
        private readonly BakeryDbContext _bakeryDbContext;
        public AuthService(IConfiguration config, BakeryDbContext bakeryDbContext)
        {
            _config = config;
            _bakeryDbContext = bakeryDbContext;
        }

        public async Task<object> Login(string username, string password)
        {
            var user = await _bakeryDbContext.Users
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user != null)
            {
                if (username.Equals(user.Username) && BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    var roles = new List<string>();
                    var privilegeRoles = new List<Privilege>();

                    foreach (var userRole in user.UserRoles)
                    {
                        var role = await _bakeryDbContext.Roles
                            .FirstOrDefaultAsync(r => r.Id.Equals(userRole.RoleId));

                        if (role != null)
                        {
                            roles.Add(role.Name);

                            var privileges = await _bakeryDbContext.Privileges
                                .Where(p => p.RoleId == role.Id)
                                .ToListAsync();   // nhớ await

                            privilegeRoles.AddRange(privileges);
                        }
                    }
                    var mergedList = privilegeRoles
                                        .GroupBy(p => p.TableName)
                                        .Select(g => new Privilege
                                        {
                                            TableId = g.First().TableId,
                                            TableName = g.Key,
                                            View = g.Max(x => x.View),
                                            Read = g.Max(x => x.Read),
                                            Write = g.Max(x => x.Write),
                                            Update = g.Max(x => x.Update),
                                            Delete = g.Max(x => x.Delete),
                                            Approve = g.Max(x => x.Approve),
                                            Assign = g.Max(x => x.Assign),
                                            Share = g.Max(x => x.Share)
                                        })
                                        .ToList();
                    var token = GenerateJwtToken(username, roles.ToArray());
                    return (new { Token = token, Roles = roles.ToArray(), PrivilegeRoles = mergedList });
                }
            }
            return null;
        }

        public async Task<string> CreateUser(string username, string password)
        {
            var existingUser = await _bakeryDbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Username == username);

            if (existingUser != null)
            {
                return "Username đã tồn tại";
            }
            //random a number for code
            int code;
            do
            {
                var rng = RandomNumberGenerator.Create();
                var bytes = new byte[4];
                rng.GetBytes(bytes);
                int raw = BitConverter.ToInt32(bytes, 0);
                code = Math.Abs(raw % 900000) + 100000;
            } while (_bakeryDbContext.Users
                .AsNoTracking()
                .Any(u => u.Code == code.ToString()));

            var user = new User
            {
                Id = Guid.NewGuid(),
                Code = code.ToString(),
                Username = username,
                Status = "ACTIVE",
                Password = BCrypt.Net.BCrypt.HashPassword(password),
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                CreatedBy = "System",
                ModifiedBy = "System"
            };

            _bakeryDbContext.Users.Add(user);
            await _bakeryDbContext.SaveChangesAsync();

            return "Success";
        }

        public Task<PagedResult<User>> GetUser(int page = 0, int size = 20)
        {
            var query = _bakeryDbContext.Users.AsNoTracking();

            var totalElements = query.Count();
            var totalPages = (int)Math.Ceiling(totalElements / (double)size);

            var result = query
                .Skip(page * size)
                .Take(size)
                .OrderBy(x => x.Username)
                .ToList();

            var response = new PagedResult<User>
            {
                Data = result,
                Page = new PageInfo
                {
                    Number = page,
                    Size = size,
                    TotalElements = totalElements,
                    TotalPages = totalPages,
                    First = page == 0,
                    Last = page >= totalPages - 1
                }
            };

            return Task.FromResult(response);
        }

        public async Task<object> GetUserRoles(Guid userId)
        {
            var existingUser = await _bakeryDbContext.Users
               .AsNoTracking()
               .FirstOrDefaultAsync(u => u.Id == userId);
            var roleUser = await _bakeryDbContext.UserRoles
                  .Where(ur => ur.UserId == userId)
                  .Select(ur => new
                  {
                      UserId = ur.UserId,
                      RoleId = ur.RoleId
                  })
          .ToListAsync();
            var roles = await _bakeryDbContext.Roles.ToListAsync();
           
            return new
            {
                user = existingUser,
                RoleUsers = roleUser,
                Roles = roles.ToList()
            };
        }


        public Task<List<Role>> GetRoles()
        {
            return Task.FromResult(_bakeryDbContext.Roles.ToList());
        }
        public Task<List<Entities>> GetEntities()
        {
            return Task.FromResult(_bakeryDbContext.Entities.ToList());
        }
        public async Task<string> CreateEntity(CreateEntity request)
        {
            var existingEntities = await _bakeryDbContext.Entities
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Name == request.TableName);

            if (existingEntities != null)
            {
                return "table đã tồn tại";
            }

            var entities = new Entities
            {
                Name = request.TableName,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                CreatedBy = "System",
                ModifiedBy = "System"
            };

            _bakeryDbContext.Entities.Add(entities);
            await _bakeryDbContext.SaveChangesAsync();

            return "Success";
        }
        public async Task<string> CreateRolePrivilege(RolePrivilegeRequest request)
        {
            //var existingUser = await _bakeryDbContext.Users
            //    .AsNoTracking()
            //    .FirstOrDefaultAsync(u => u.Username == username);

            //if (existingUser != null)
            //{
            //    return "Username đã tồn tại";
            //}

            //var user = new User
            //{
            //    Id = Guid.NewGuid(),
            //    Username = username,
            //    Password = BCrypt.Net.BCrypt.HashPassword(password),
            //    CreatedDate = DateTime.UtcNow,
            //    ModifiedDate = DateTime.UtcNow,
            //    CreatedBy = "System",
            //    ModifiedBy = "System"
            //};

            //_bakeryDbContext.Users.Add(user);
            //await _bakeryDbContext.SaveChangesAsync();

            return "Success";
        }
        public async Task<List<Privilege>> GetRolePrivileges(Guid roleId)
        {
            var privileges = await _bakeryDbContext.Privileges
                .Where(p => p.RoleId == roleId)
                .ToListAsync();

            return privileges;
        }

        public string GenerateJwtToken(string username, string[] roles)
        {
            var secretKey = _config["Jwt:Key"] ?? "this_is_a_super_secret_key_1234567890";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, username)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"] ?? "bakery-app",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}