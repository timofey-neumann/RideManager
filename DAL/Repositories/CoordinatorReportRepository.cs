using Domain.DTOs;
using Domain.Enums;
using DAL.Persistence;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class CoordinatorReportRepository(RideManagerContext context) : BaseReportRepository(context), ICoordinatorReportRepository
{
    public async Task AddTripAsync(Trip Trip)
    {
        _context.Trips.Add(Trip);
        await _context.SaveChangesAsync();
    }

    public async Task<ReportDate> CreateReportDateAsync(int reportYear, int reportMonth)
    {
        var reportDate = await _context.ReportDates
            .FirstOrDefaultAsync(r => r.Year == reportYear && r.Month == reportMonth);

        if (reportDate == null)
        {
            reportDate = new ReportDate
            {
                Year = reportYear,
                Month = reportMonth,
                Trip = []
            };
            _context.ReportDates.Add(reportDate);
            await _context.SaveChangesAsync();
        }

        return reportDate;
    }

    public async Task<List<InfoReportDto>> GetInfoReportTripsAsync(int reportMonth, int reportYear)
    {
        var infoReport = await _context.Trips
        .Where(trip => trip.ReportDate.Month == reportMonth
                       && trip.ReportDate.Year == reportYear
                       && trip.Report != null
                       && trip.Report.Status == Status.Closed)
        .Select(trip => new InfoReportDto
        {
            Year = trip.ReportDate.Year.ToString(),
            Month = trip.ReportDate.Month.ToString(),
            PassengerName = trip.PassengerName,
            Code = "Code_Budget",
            VAT = $"Без НДС: {trip.CostWithoutVAT:F2}, НДС: {trip.VAT:F2}"
        })
        .ToListAsync();

        return infoReport;
    }

    public async Task CreateReportTripsAsync(int reportMonth, int reportYear)
    {
        var trips = await _context.Trips
            .Where(t => t.ReportDate.Month == reportMonth && t.ReportDate.Year == reportYear && t.ReportId == null)
            .ToListAsync();

        if (trips.Count == 0)
        {
            throw new NotFoundException("Поездки за указанный период отсутствует или уже добавлены в отчет.");
        }

        var groupedTrips = trips.GroupBy(t => t.PassengerEmail);

        foreach (var group in groupedTrips)
        {
            var report = new Report
            {
                Status = Status.New,
                Trip = [.. group]
            };

            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            foreach (var trip in group)
            {
                trip.ReportId = report.Id;
            }
        }

        await _context.SaveChangesAsync();
    }
}
