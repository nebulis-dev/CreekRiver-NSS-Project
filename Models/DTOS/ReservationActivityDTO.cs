namespace CreekRiver.Models.DTO;

public class ReservationActivityDTO
{
    public int Id { get; set; }
    public int ActivityId { get; set; }
    public ActivityDTO Activity { get; set; }
    public int Quantity { get; set; }
}