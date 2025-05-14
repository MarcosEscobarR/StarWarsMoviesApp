using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Movie: AuditableEntity
{
    [Key]
    public Guid Id { get; set; }
    
    [MaxLength(100)]
    public string Title { get; set; }
    
    [MaxLength(255)]
    public string Producer { get; set; }
    
    [MaxLength(50)]
    public string Director { get; set; }
    public DateTime? ReleaseDate { get; set; }
}