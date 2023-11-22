namespace MovieManagement.Entities
{
    public class User : BaseEntity
    {
        public int? Point { get; set; } = 0;
        public string Username { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public int? RankCustomerId { get; set; }
        public int UserStatusId { get; set; }
        public bool? IsActive { get; set; } = true;
        public int RoleId { get; set; }
        public Role? Role { get; set; }
        public RankCustomer? RankCustomer { get; set; }
        public UserStatus? UserStatus { get; set; }
        public IEnumerable<Bill>? Bills { get; set; }
        public IEnumerable<RefreshToken>? RefreshTokens { get; set; }
        public IEnumerable<ConfirmEmail>? ConfirmEmails { get; set;}
    }
}
