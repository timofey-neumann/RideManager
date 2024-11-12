using Domain.DTOs;
using Domain.Entities;

namespace Domain.Interfaces;

public interface IPassengerReportService
{
    Task EditTripByIdAsync(Guid tripId);
    Task ClosedReportByIdAsync(Guid reportId);
    Task<List<TripDto>> GetReportByIdAsync(Guid reportId);
    Task<List<ReportDto>> GetReportsByEmailAsync(string email);
}