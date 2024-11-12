using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces;

namespace BLL.Services;

public class PassengerReportService(IPassengerReportRepository repository) : IPassengerReportService
{
    public async Task EditTripByIdAsync(Guid tripId)
    {
        await repository.EditTripByIdAsync(tripId);
    }

    public async Task ClosedReportByIdAsync(Guid reportId)
    {
        await repository.ClosedReportByIdAsync(reportId);
    }

    public async Task<List<ReportDto>> GetReportsByEmailAsync(string email)
    {
        return await repository.GetReportsAsync(email);
    }

    public async Task<List<TripDto>> GetReportByIdAsync(Guid reportId)
    {
        var trips = await repository.GetTripsAsync(reportId);

        return trips.Select(TripMapper.Instance.MapToTripDto).ToList();
    }
}