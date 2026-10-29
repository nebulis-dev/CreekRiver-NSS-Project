using System.ComponentModel.DataAnnotations;

namespace CreekRiver.Models;

public class Activity
{
    public int Id { get; set; }
    [Required]
    public string ActivityName { get; set; }
    public decimal Price { get; set; }
}