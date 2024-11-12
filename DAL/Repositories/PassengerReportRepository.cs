using Domain.Enums;
using DAL.Persistence;
using Domain.Interfaces;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class PassengerReportRepository(RideManagerContext context) : BaseReportRepository(context), IPassengerReportRepository
{
    public async Task ClosedReportByIdAsync(Guid reportId)
    {
        var report = await _context.Reports.FirstOrDefaultAsync(r => r.Id == reportId) ?? throw new NotFoundException("Такого отчета не существует.");
        report.Status = Status.Closed;

        await _context.SaveChangesAsync();
    }

    public async Task EditTripByIdAsync(Guid tripId)
    {
        var systemSetting = await _context.SystemSettings
            .FirstOrDefaultAsync() ?? throw new BadRequestException("Настройки системы не найдены.");

        var trip = await _context.Trips
            .Include(t => t.Report)
            .FirstOrDefaultAsync(t => t.Id == tripId) ?? throw new NotFoundException("Поездка не найдена.");

        if (trip.ReportId != null && trip.Report!.Status == Status.New)
        {
            if (trip.IsPersonal)
            {
                var timeDifference = DateTime.UtcNow - trip.PersonalStatusSetAt.GetValueOrDefault();

                if (timeDifference.TotalHours > systemSetting.HoursForPersonalTripStatusChange)
                {
                    throw new BadRequestException("Вы не можете снять пометку о личной поездке после этого времени.");
                }
            }
            else
            {
                trip.IsPersonal = true;
                trip.PersonalStatusSetAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }
        else
        {
            throw new BadRequestException("Поездку уже нельзя изменить.");
        }
    }
}