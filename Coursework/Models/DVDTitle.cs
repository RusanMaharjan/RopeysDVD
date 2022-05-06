using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coursework.Models;

public class DVDTitle
{
    [Key]
    public int DVDNumber { get; set; }

    public int CategoryNumber { get; set; }
    public int StudioNumber { get; set; }
    public int ProducerNumber { get; set; }
    [Required]
    public string TitleName { get; set; }
    [Required]
    public string DateReleased { get; set; }
    [Required]
    public int StandardCharge { get; set; }
    [Required]
    public int PenaltyCharge { get; set; }
    
    [ForeignKey("CategoryNumber")]
    public DVDCategory DvdCategory { get; set; }
    
    [ForeignKey("StudioNumber")]
    public Studio Studio { get; set; }
    
    [ForeignKey("ProducerNumber")]
    public Producer Producer { get; set; }
    [NotMapped] 
    public virtual string actors { get; set; }
}