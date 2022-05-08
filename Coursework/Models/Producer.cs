using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Coursework.Models;

public class Producer
{
    [Key]
    [DisplayName("Producer Number")]
    public int ProducerNumber { get; set; }
    [Required]
    [DisplayName("Producer Name")]
    public string ProducerName { get; set; }
}