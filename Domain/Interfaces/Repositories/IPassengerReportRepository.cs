using Domain.DTOs;
using Domain.Entities;

namespace Domain.Interfaces;

public interface IPassengerReportRepository
{
    Task EditTripByIdAsync(Guid tripId);
    Task ClosedReportByIdAsync(Guid reportId);
    Task<List<Trip>> GetTripsAsync(Guid? reportId = null);
    Task<List<ReportDto>> GetReportsAsync(string? email = null);
}