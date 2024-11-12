using Domain.Entities;

namespace Domain.DTOs;

public class TripMapper
{
    private static readonly Lazy<TripMapper> instance = new(() => new TripMapper());
    private TripMapper() { }

    public static TripMapper Instance => instance.Value;

    public TripDto MapToTripDto(Trip trip)
    {
        return new TripDto
        {
            Id = trip.Id,
            PassengerName = trip.PassengerName,
            PassengerEmail = trip.PassengerEmail,
            StartDateTime = trip.StartDateTime,
            StartAddress = trip.StartAddress,
            EndDateTime = trip.EndDateTime,
            EndAddress = trip.EndAddress,
            CostWithoutVAT = trip.CostWithoutVAT,
            VAT = trip.VAT,
            IsPersonal = trip.IsPersonal
        };
    }
}
