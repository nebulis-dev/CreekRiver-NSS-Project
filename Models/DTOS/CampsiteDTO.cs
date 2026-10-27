using System.ComponentModel.DataAnnotations;

namespace CreekRiver.Models.DTO;

public class CampsiteDTO
{
    public int Id { get; set; }
    public string Nickname { get; set; }
    public string ImageUrl { get; set; }
    public int CampsiteTypeId { get; set; }
    public CampsiteTypeDTO CampsiteType { get; set; }
    public List<ReservationDTO> Reservations { get; set; }
    public bool Wifi { get; set; }
    public bool FirePit { get; set; }
    public bool WaterHookup { get; set; }
    public bool PetFriendly { get; set; }
}