using Microsoft.EntityFrameworkCore;
using CreekRiver.Models;

public class CreekRiverDbContext : DbContext
{

    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<Campsite> Campsites { get; set; }
    public DbSet<CampsiteType> CampsiteTypes { get; set; }
    public DbSet<Amenity> Amenities { get; set; }
    
    public CreekRiverDbContext(DbContextOptions<CreekRiverDbContext> context) : base(context)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // seed data with campsite types
        modelBuilder.Entity<CampsiteType>().HasData(new CampsiteType[]
        {
            new CampsiteType {Id = 1, CampsiteTypeName = "Tent", FeePerNight = 15.99M, MaxReservationDays = 7},
            new CampsiteType {Id = 2, CampsiteTypeName = "RV", FeePerNight = 26.50M, MaxReservationDays = 14},
            new CampsiteType {Id = 3, CampsiteTypeName = "Primitive", FeePerNight = 10.00M, MaxReservationDays = 3},
            new CampsiteType {Id = 4, CampsiteTypeName = "Hammock", FeePerNight = 12M, MaxReservationDays = 7}
        });

        modelBuilder.Entity<Campsite>().HasData(new Campsite[]
        {
            new Campsite {Id = 1, CampsiteTypeId = 1, Nickname = "Barred Owl", ImageUrl="https://tnstateparks.com/assets/images/content-images/campgrounds/249/colsp-area2-site73.jpg"},
            new Campsite {Id = 2, CampsiteTypeId = 2, Nickname = "The Backyard", ImageUrl="https://www.startpage.com/av/proxy-image?piurl=https%3A%2F%2Fwww.campspot.com%2Fassets%2Fimg%2Fbackground-elkamp-1920x1280-new-2.jpg&sp=1793146847Tbb88162a98b501703861fedb13189385a69ccf1dab97da9f3be9a7a77e84b381"}

        });

        modelBuilder.Entity<UserProfile>().HasData(new UserProfile[]
        {
            new UserProfile {Id = 1, FirstName = "Baron", LastName = "Cloud", Email = "example@email.com"},
        });

        modelBuilder.Entity<Amenity>().HasData(new Amenity[]
        {
            new Amenity {Id = 1, AmenityName = "WiFi"},
            new Amenity {Id = 2, AmenityName = "Fire Pit"},
            new Amenity {Id = 3, AmenityName = "Water Hookup"},
            new Amenity {Id = 4, AmenityName = "Pet Friendly"}
        });

        modelBuilder.Entity("AmenityCampsite").HasData(new object[]
        {
            new {AmenitiesId = 1, CampsitesId = 1},
            new {AmenitiesId = 2, CampsitesId = 1},
            new {AmenitiesId = 2, CampsitesId = 2}
        });
    }


}