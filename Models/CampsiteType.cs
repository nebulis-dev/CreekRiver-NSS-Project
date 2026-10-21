using System.ComponentModel.DataAnnotations;
using System.Xml;

namespace CreekRiver.Models;

public class CampsiteType
{
    public int Id { get; set; }
    [Required]
    public string CampsiteTypeName { get; set; }
    public int MaxReservationDays { get; set; }
    public decimal FeePerNight { get; set; }
}
