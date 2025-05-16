using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class User : AuditableEntity
{
    public User(string name, string lastName, string email, string password)
    {
        Name = name;
        LastName = lastName;
        Email = email;
        Password = password;
    }
    
    [Key]
    public Guid Id { get; set; }
    
    [MaxLength(20)]
    public string Name { get; set; }
    
    [MaxLength(20)]
    public string LastName { get; set; }
    
    [MaxLength(30)]
    public string Email { get; set; }
    
    [MaxLength(255)]
    public string Password { get; set; }

    public UserRole Role { get; set; } = UserRole.User;

    public string FullName => $"{Name} {LastName}";
    
    
}

public enum UserRole
{
    Admin,
    User
}