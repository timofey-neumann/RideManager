using Domain.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WEB.Controllers;

[Route("api/passenger")]
[Authorize(Roles = "Employee, Coordinator")]
public class PassengerReportsController(IPassengerReportService service) : BaseController
{
    [HttpGet("reports")]
    public async Task<IActionResult> GetAllReporAsync()
    {
        var emailClaim = User.FindFirst(ClaimTypes.Email);
        string? email = emailClaim?.Value;

        ValidateEmail(email);

        var reports = await service.GetReportsByEmailAsync(email!);

        if (reports == null || reports.Count == 0)
        {
            return BadRequest(new { message = "Список отчетов пуст." });
        }

        return Ok(reports);
    }

    [HttpGet("report")]
    public async Task<IActionResult> GetReportIdAsync(Guid reportId)
    {
        var report = await service.GetReportByIdAsync(reportId);

        if (report == null || report.Count == 0)
        {
            return BadRequest(new { message = "Такого отчета не существует." });
        }

        return Ok(report);
    }

    [HttpPut("edit-report")]
    public async Task<IActionResult> EditTripByIdAsync(Guid tripId)
    {
        await service.EditTripByIdAsync(tripId);
        return Ok();
    }

    [HttpPut("closed-report")]
    public async Task<IActionResult> ClosedReportIdAsync(Guid reportId)
    {
        await service.ClosedReportByIdAsync(reportId);
        return Ok();
    }
}