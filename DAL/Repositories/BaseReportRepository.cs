using Domain.DTOs;
using Domain.Entities;
using DAL.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public abstract class BaseReportRepository(RideManagerContext context)
{
    protected readonly RideManagerContext _context = context;

    public async Task<List<Trip>> GetTripsAsync(Guid? reportId = null)
    {
        return await _context.Trips
        .Where(t => reportId == null || t.ReportId == reportId)
        .ToListAsync();
    }

    public async Task<List<ReportDto>> GetReportsAsync(string? email = null)
    {
        var query = _context.Trips
            .Where(t => t.ReportId != null && (email == null || t.PassengerEmail == email))
            .Select(t => new ReportDto
            {
                Id = t.ReportId!.Value,
                Year = t.ReportDate.Year,
                Month = t.ReportDate.Month,
                PassengerName = t.PassengerName,
                PassengerEmail = t.PassengerEmail,
                Status = t.Report!.Status
            })
            .Distinct();

        return await query.ToListAsync();
    }
}