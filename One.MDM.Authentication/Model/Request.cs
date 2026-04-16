namespace One.MDM.Authentication.Model
{

    public class LoginRequest
    {
        public string Username { get; set; } = "admin";
        public string Password { get; set; } = "123456";
    }

    public class RolePrivilegeRequest
    {
        public Role Role { get; set; }
        public List<Privilege> Privileges { get; set; }
    }
    public class CreateEntity
    {
        public string TableName { get; set; }
    }
}
