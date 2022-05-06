using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coursework.Models;

public class CastMember
{
    [Key]
    public int Id { get; set; }
    
    public int DVDNumber { get; set; }
    
    public int ActorNumber { get; set; }  

    [ForeignKey("DVDNumber")]
    public DVDTitle DvdTitle { get; set; }
    [Key]
    [ForeignKey("ActorNumber")]
    public Actor Actor { get; set; }
}