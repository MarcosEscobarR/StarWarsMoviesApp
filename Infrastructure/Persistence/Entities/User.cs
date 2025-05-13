using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Persistence.Entities;

public class User : AuditableEntity
{
    public Guid Id { get; set; }
    [MaxLength(20)]
    public string Name { get; set; }
    
    [MaxLength(20)]
    public string LastName { get; set; }
    
    [MaxLength(30)]
    public string Email { get; set; }
    
    [MaxLength(15)]
    [MinLength(8)]
    public string Password { get; set; }
}