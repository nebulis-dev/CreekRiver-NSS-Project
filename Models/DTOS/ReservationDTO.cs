namespace CreekRiver.Models.DTO;

public class ReservationDTO
{
    public int Id { get; set; }
    public int CampsiteId { get; set; }
    public CampsiteDTO Campsite { get; set; }
    public int UserProfileId { get; set; }
    public UserProfileDTO UserProfile { get; set; }
    public DateTime CheckinDate { get; set; }
    public DateTime CheckoutDate { get; set; }

    public DateTime? CancelledDate { get; set; }

    public List<ReservationActivityDTO> ReservationActivities { get; set; }

    public int TotalNights => (CheckinDate - CheckoutDate).Days;

    private static readonly decimal _reservationsBaseFee = 10M;

    public decimal TotalCost
    {
        get
        {
            decimal activitiesCost = ReservationActivities.Sum(ra => ra.Activity.Price * ra.Quantity);
            return Campsite.CampsiteType.FeePerNight * TotalNights + _reservationsBaseFee + activitiesCost;
        }
    }
}