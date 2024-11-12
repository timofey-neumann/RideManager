using Domain.Exceptions;
using Domain.Interfaces;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WEB.Controllers;

[Route("api/coordinator")]
[Authorize(Roles = "Coordinator")]
public class CoordinatorReportsController(ICoordinatorReportService service) : Controller
{
    [HttpPost("import")]
    public async Task<IActionResult> ImportTripsAsync(IFormFile file, int reportMonth, int reportYear)
    {
        var validationErrors = ReportValidator.Instance.ValidateImportParameters(file, reportMonth, reportYear);

        if (validationErrors.Count != 0)
            return BadRequest(validationErrors);

        using (var stream = file.OpenReadStream())
        {
            var errorMessages = await service.ImportTripsFromExcelAsync(stream, reportMonth, reportYear);

            if (errorMessages.Count != 0)
                return BadRequest(errorMessages);
        }

        return Ok("Импорт данных прошел успешно.");
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateReportTripsAsync(int reportMonth, int reportYear)
    {
        var validationErrors = ReportValidator.Instance.ValidateCreateParameters(reportMonth, reportYear);

        if (validationErrors.Count != 0)
            return BadRequest(validationErrors);

        await service.CreateReportTripsAsync(reportMonth, reportYear);

        return Ok("Создание отчета прошло успешно.");
    }

    [HttpGet("reports")]
    public async Task<IActionResult> GetAllReporAsync()
    {
        var reports = await service.GetReportsAsync();

        if (reports == null || reports.Count == 0)
        {
            throw new BadRequestException("Список отчетов пуст.");
        }

        return Ok(reports);
    }

    [HttpGet("trips")]
    public async Task<IActionResult> GetAllTripsAsync()
    {
        var trips = await service.GetTripsAsync();

        if (trips == null || trips.Count == 0)
        {
            throw new BadRequestException("Список поездок пуст.");
        }

        return Ok(trips);
    }

    [HttpGet("info-report")]
    public async Task<IActionResult> GetInfoReportTripsAsync(int reportMonth, int reportYear)
    {
        var validationErrors = ReportValidator.Instance.ValidateCreateParameters(reportMonth, reportYear);

        if (validationErrors.Count != 0)
            return BadRequest(validationErrors);

        var reports = await service.GetInfoReportTripsAsync(reportMonth, reportYear);

        if (reports == null || reports.Count == 0)
        {
            throw new BadRequestException("Информационный отчет не может быть создан.");
        }

        return Ok(reports);
    }
}