using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coursework.Models;

public class DVDCopy
{
    [Key]
    public int CopyNumber { get; set; }

    [DisplayName("DVD Number")]
    public int DVDNumber { get; set; }
    [Required]
    [DisplayName("Date Purchased")]
    public DateTime DatePurchased { get; set; }
    [ForeignKey("DVDNumber")]
    public DVDTitle DvdTitle { get; set; }
}