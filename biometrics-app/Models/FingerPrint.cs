namespace biometrics_app.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserFingerprint? UserFingerprint { get; set; }
    }

    public class UserFingerprint
    {
        public int UserId { get; set; }
        public byte[]? FingerprintData { get; set; }

        public User? User { get; set; }
    }

}
