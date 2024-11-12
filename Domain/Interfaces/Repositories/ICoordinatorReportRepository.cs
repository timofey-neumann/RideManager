using Domain.DTOs;
using Domain.Entities;

namespace Domain.Interfaces;

public interface ICoordinatorReportRepository
{
    Task AddTripAsync(Trip Trip);
    Task<List<Trip>> GetTripsAsync(Guid? reportId = null);
    Task<List<ReportDto>> GetReportsAsync(string? email = null);
    Task CreateReportTripsAsync(int reportMonth, int reportYear);
    Task<ReportDate> CreateReportDateAsync(int reportYear, int reportMonth);
    Task<List<InfoReportDto>> GetInfoReportTripsAsync(int reportMonth, int reportYear);
}