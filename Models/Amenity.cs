using System.ComponentModel.DataAnnotations;

namespace CreekRiver.Models;

public class Amenity
{
    public int Id { get; set; }
    [Required]
    public string AmenityName { get; set; }
    public List<Campsite> Campsites { get; set; }
}