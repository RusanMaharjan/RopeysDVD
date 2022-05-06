using System.ComponentModel.DataAnnotations;

namespace Coursework.Models;

public class LoanType
{
    [Key]
    public int LoanTypeNumber { get; set; }
    [Required]
    public string Loan_Type { get; set; }
    [Required]
    public string LoanDuration { get; set; }
}