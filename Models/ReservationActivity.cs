namespace CreekRiver.Models;

public class ReservationActivity
{
    public int Id { get; set; }
    public int ReservationId { get; set; }
    public Reservation Reservation { get; set; }
    public int ActivityId { get; set; }
    public Activity Activity { get; set; }
    public int Quantity { get; set; }
}