using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coursework.Models;

public class CastMember
{
    [Key]
    [DisplayName("CastMember Id")]
    public int Id { get; set; }
    [DisplayName("DVD Number")]
    public int DVDNumber { get; set; }
    
    [DisplayName("Actor Number")]
    public int ActorNumber { get; set; }  

    [ForeignKey("DVDNumber")]
    public DVDTitle DvdTitle { get; set; }
    [Key]
    [ForeignKey("ActorNumber")]
    public Actor Actor { get; set; }
}