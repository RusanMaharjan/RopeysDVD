using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Coursework.Models;

public class Actor
{
    [Key]
    [DisplayName("Actor Number")]
    public int ActorNumber { get; set; }
    [Required]
    [DisplayName("Actor Surname")]
    public string ActorSurname { get; set; }
    [Required]
    [DisplayName("Actor Firstname")]
    public string ActorFirstName { get; set; }
}