using CreekRiver.Models;
using CreekRiver.Models.DTO;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using System.Linq.Expressions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// allows passing datetimes without time zone data 
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// allows our api endpoints to access the database through Entity Framework Core
builder.Services.AddNpgsql<CreekRiverDbContext>(builder.Configuration["CreekRiverDbConnectionString"]);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Get endpoints
app.MapGet("/api/campsites", (CreekRiverDbContext db) =>
{
    return db.Campsites
    .Select(c => new CampsiteDTO
    {
        Id = c.Id,
        Nickname = c.Nickname,
        ImageUrl = c.ImageUrl,
        CampsiteTypeId = c.CampsiteTypeId
    }).ToList();
});

app.MapGet("/api/campsites/{id}", (CreekRiverDbContext db, int id) =>
{
    return db.Campsites
        .Include(c => c.CampsiteType)
        .Include(c => c.Amenities)
        .Select(c => new CampsiteDTO
        {
            Id = c.Id,
            Nickname = c.Nickname,
            ImageUrl = c.ImageUrl,
            CampsiteTypeId = c.CampsiteTypeId,
            CampsiteType = new CampsiteTypeDTO
            {
                Id = c.CampsiteType.Id,
                CampsiteTypeName = c.CampsiteType.CampsiteTypeName,
                FeePerNight = c.CampsiteType.FeePerNight,
                MaxReservationDays = c.CampsiteType.MaxReservationDays
            },
            Amenities = c.Amenities.Select(a => new AmenityDTO
            {
                Id = a.Id,
                AmenityName = a.AmenityName
            }).ToList()
        })
        .Single(c => c.Id == id);
});

app.MapGet("/api/reservations", (CreekRiverDbContext db) =>
{
    return db.Reservations
        .Include(r => r.UserProfile)
        .Include(r => r.Campsite)
        .ThenInclude(c => c.CampsiteType)
        .Include(r => r.ReservationActivities)
        .ThenInclude(ra => ra.Activity)
        .OrderBy(res => res.CheckinDate)
        .Select(r => new ReservationDTO
        {
            Id = r.Id,
            CampsiteId = r.CampsiteId,
            UserProfileId = r.UserProfileId,
            CheckinDate = r.CheckinDate,
            CheckoutDate = r.CheckoutDate,
            CancelledDate = r.CancelledDate,
            UserProfile = new UserProfileDTO
            {
                Id = r.UserProfile.Id,
                FirstName = r.UserProfile.FirstName,
                LastName = r.UserProfile.LastName,
                Email = r.UserProfile.Email
            },
            Campsite = new CampsiteDTO
            {
                Id = r.Campsite.Id,
                Nickname = r.Campsite.Nickname,
                ImageUrl = r.Campsite.ImageUrl,
                CampsiteTypeId = r.Campsite.CampsiteTypeId,
                CampsiteType = new CampsiteTypeDTO
                {
                    Id = r.Campsite.CampsiteType.Id,
                    CampsiteTypeName = r.Campsite.CampsiteType.CampsiteTypeName,
                    MaxReservationDays = r.Campsite.CampsiteType.MaxReservationDays,
                    FeePerNight = r.Campsite.CampsiteType.FeePerNight
                }
            },
            ReservationActivities = r.ReservationActivities.Select(ra => new ReservationActivityDTO
            {
                Id = ra.Id,
                ActivityId = ra.ActivityId,
                Quantity = ra.Quantity,
                Activity = new ActivityDTO
                {
                    Id = ra.Activity.Id,
                    ActivityName = ra.Activity.ActivityName,
                    Price = ra.Activity.Price
                }
            }).ToList()
        })
        .ToList();
});

// Post Endpoints
app.MapPost("/api/campsites", (CreekRiverDbContext db, Campsite campsite) =>
{
    db.Campsites.Add(campsite);
    db.SaveChanges();
    return Results.Created($"/api/campsites/{campsite.Id}", campsite);
});

app.MapPost("/api/reservations", (CreekRiverDbContext db, Reservation newRes) =>
{
    try
    {
        db.Reservations.Add(newRes);
        db.SaveChanges();
        return Results.Created($"/api/reservations/{newRes.Id}", newRes);
    }
    catch (DbUpdateException)
    {
        return Results.BadRequest("Invalid data submitted");
    }
});

app.MapPost("/api/reservations/{id}/cancel", (CreekRiverDbContext db, int id) =>
{
    Reservation reservation = db.Reservations.SingleOrDefault(r => r.Id == id);
    if (reservation == null)
    {
        return Results.NotFound();
    }
    reservation.CancelledDate = DateTime.Now;
    db.SaveChanges();
    return Results.NoContent();
});

app.MapPost("/api/reservations/{id}/activities", (CreekRiverDbContext db, int id, ReservationActivity newReservationActivity) =>
{
    newReservationActivity.ReservationId = id;
    try
    {
        db.ReservationActivities.Add(newReservationActivity);
        db.SaveChanges();
        return Results.Created($"/api/reservations/{id}/activitites/{newReservationActivity.Id}", newReservationActivity);
    }
    catch (DbUpdateException)
    {
        return Results.BadRequest("Invalid data submitted");
    }
});

// Delete Endpoint(s)
app.MapDelete("/api/campsites/{id}", (CreekRiverDbContext db, int id) =>
{
    Campsite campsite = db.Campsites.SingleOrDefault(campsite => campsite.Id == id);
    if (campsite == null)
    {
        return Results.NotFound();
    }
    db.Campsites.Remove(campsite);
    db.SaveChanges();
    return Results.NoContent();
});

// Put Endpoints
app.MapPut("/api/campsites/{id}", (CreekRiverDbContext db, int id, Campsite campsite) =>
{
    Campsite campsiteToUpdate = db.Campsites.SingleOrDefault(campsite => campsite.Id == id);
    if (campsiteToUpdate == null)
    {
        return Results.NotFound();
    }
    campsiteToUpdate.Nickname = campsite.Nickname;
    campsiteToUpdate.CampsiteTypeId = campsite.CampsiteTypeId;
    campsiteToUpdate.ImageUrl = campsite.ImageUrl;

    db.SaveChanges();
    return Results.NoContent();
});

app.MapPut("/api/campsites/{id}/amenities", (CreekRiverDbContext db, int id, List<int> amenityIds) =>
{
    Campsite campsite = db.Campsites.Include(c => c.Amenities).SingleOrDefault(c => c.Id == id);
    if (campsite == null)
    {
        return Results.NotFound();
    }
    campsite.Amenities = db.Amenities.Where(a => amenityIds.Contains(a.Id)).ToList();
    db.SaveChanges();
    return Results.NoContent();
});

app.Run();
