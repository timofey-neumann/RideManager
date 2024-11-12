using Domain.DTOs;
using Domain.Entities;

namespace Domain.Interfaces;

public interface ICoordinatorReportService
{
    Task<List<TripDto>> GetTripsAsync();
    Task<List<ReportDto>> GetReportsAsync();
    Task CreateReportTripsAsync(int reportMonth, int reportYear);
    Task<List<InfoReportDto>> GetInfoReportTripsAsync(int reportMonth, int reportYear);
    Task<List<string>> ImportTripsFromExcelAsync(Stream fileStream, int reportMonth, int reportYear);
}