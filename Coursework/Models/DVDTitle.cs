using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coursework.Models;

public class DVDTitle
{
    [Key]
    [DisplayName("DVD Number")]
    public int DVDNumber { get; set; }

    [DisplayName("Category Number")]
    public int CategoryNumber { get; set; }
    
    [DisplayName("Studio Number")]
    public int StudioNumber { get; set; }
    
    [DisplayName("Producer Number")]
    public int ProducerNumber { get; set; }
    
    [Required]
    [DisplayName("Title Name")]
    public string TitleName { get; set; }
    
    [Required]
    [DisplayName("Date Released")]
    public string DateReleased { get; set; }
    
    [Required]
    [DisplayName("Standard Charge")]
    public int StandardCharge { get; set; }
    
    [Required]
    [DisplayName("Penalty Charge")]
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