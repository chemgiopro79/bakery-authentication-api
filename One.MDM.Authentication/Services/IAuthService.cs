using One.MDM.Authentication.Controllers;
using One.MDM.Authentication.Model;

namespace One.MDM.Authentication.Services
{
    public interface IAuthService
    {
        Task<object> Login(string username, string password);
        string GenerateJwtToken(string username, string[] roles);
        Task<string> CreateUser(string username, string password);
        Task<string> CreateRolePrivilege(RolePrivilegeRequest request);
        Task<string> CreateEntity(CreateEntity request);
        Task<List<User>> GetUser();
        Task<object> GetUserRoles(Guid userId);
        Task<List<Role>> GetRoles();
        Task<List<Entities>> GetEntities();
        Task<List<Privilege>> GetRolePrivileges(Guid roleId);
    }
}
