namespace One.MDM.Authentication.Model
{

    public class User
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
        public string? ModifiedBy { get; set; }
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }

        public ICollection<Privilege> Privileges { get; set; } = new List<Privilege>();
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
    public class UserRole
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public User User { get; set; }
        public Role Role { get; set; }

    }
    public class Entities
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
    }
    public class Privilege
    {
        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
        public Role Role { get; set; } = null!;
        public Entities Entities { get; set; } = null!;
        public int TableId { get; set; } 
        public string TableName { get; set; } = null!;
        public int View { get; set; }
        public int Read { get; set; }
        public int Write { get; set; }
        public int Update { get; set; }
        public int Delete { get; set; }
        public int Approve { get; set; }
        public int Assign { get; set; }
        public int Share { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }

    }
}
