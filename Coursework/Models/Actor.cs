using System.ComponentModel.DataAnnotations;

namespace Coursework.Models;

public class Actor
{
    [Key]
    public int ActorNumber { get; set; }
    [Required]
    public string ActorSurname { get; set; }
    [Required]
    public string ActorFirstName { get; set; }
}