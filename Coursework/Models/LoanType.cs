using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Coursework.Models;

public class LoanType
{
    [Key]
    [DisplayName("Loan Type Number")]
    public int LoanTypeNumber { get; set; }
    
    [Required]
    [DisplayName("Loan Type")]
    public string Loan_Type { get; set; }
    
    [Required]
    [DisplayName("Loan Duration")]
    public string LoanDuration { get; set; }
}