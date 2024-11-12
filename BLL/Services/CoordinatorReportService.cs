using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using System.Globalization;
using Microsoft.Extensions.Logging;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace BLL.Services;

public class CoordinatorReportService(ICoordinatorReportRepository repository, ILogger<CoordinatorReportService> logger) : ICoordinatorReportService
{
    public async Task<List<ReportDto>> GetReportsAsync()
    {
        return await repository.GetReportsAsync();
    }

    public async Task CreateReportTripsAsync(int reportMonth, int reportYear)
    {
        await repository.CreateReportTripsAsync(reportMonth, reportYear);
    }

    public async Task<List<InfoReportDto>> GetInfoReportTripsAsync(int reportMonth, int reportYear)
    {
        return await repository.GetInfoReportTripsAsync(reportMonth, reportYear);
    }

    public async Task<List<TripDto>> GetTripsAsync()
    {
        var trips = await repository.GetTripsAsync();

        return trips.Select(TripMapper.Instance.MapToTripDto).ToList();
    }

    public async Task<List<string>> ImportTripsFromExcelAsync(Stream fileStream, int reportMonth, int reportYear)
    {
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        var errorMessages = new List<string>();

        try
        {
            using (SpreadsheetDocument document = SpreadsheetDocument.Open(fileStream, false))
            {
                var workbookPart = document.WorkbookPart ?? throw new Exception("Отсутствует часть книги (WorkbookPart)");
                var sheet = workbookPart.Workbook.Descendants<Sheet>().FirstOrDefault() ?? throw new Exception("Не найден лист в Excel файле");
                var sheetId = sheet.Id?.Value ?? throw new Exception("Отсутствует ID листа");
                var worksheetPart = workbookPart.GetPartById(sheetId) as WorksheetPart ?? throw new Exception("Отсутствует часть рабочего листа (WorksheetPart)");

                var rows = worksheetPart.Worksheet.Descendants<Row>().Skip(1);

                var reportDate = await repository.CreateReportDateAsync(reportYear, reportMonth);

                var trips = new List<Trip>();

                foreach (var row in rows)
                {
                    var cells = row.Elements<Cell>().ToList();

                    try
                    {
                        var trip = new Trip
                        {
                            PassengerName = GetCellValue(workbookPart, cells[0]),
                            PassengerEmail = GetCellValue(workbookPart, cells[1]),
                            StartDateTime = ConvertToUtc(DateTime.ParseExact(GetCellValue(workbookPart, cells[2]), "d.M.yyyy H:mm", CultureInfo.InvariantCulture)),
                            StartAddress = GetCellValue(workbookPart, cells[3]),
                            EndDateTime = ConvertToUtc(DateTime.ParseExact(GetCellValue(workbookPart, cells[4]), "d.M.yyyy H:mm", CultureInfo.InvariantCulture)),
                            EndAddress = GetCellValue(workbookPart, cells[5]),
                            CostWithoutVAT = decimal.Parse(GetCellValue(workbookPart, cells[6])),
                            VAT = decimal.Parse(GetCellValue(workbookPart, cells[7])),
                            IsPersonal = false,
                            ReportDate = reportDate
                        };

                        trips.Add(trip);
                    }
                    catch (FormatException ex)
                    {
                        logger.LogError(ex, "Ошибка при обработке строки Excel.");
                        errorMessages.Add($"Ошибка при обработке строки: {ex.Message}. Строка пропущена.");
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Неизвестная ошибка при обработке строки Excel.");
                        errorMessages.Add($"Неизвестная ошибка: {ex.Message}. Строка пропущена.");
                    }
                }

                foreach (var trip in trips)
                {
                    await repository.AddTripAsync(trip);
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при обработке Excel файла.");
            errorMessages.Add($"Ошибка при обработке Excel файла: {ex.Message}");
        }

        return errorMessages;
    }

    private DateTime ConvertToUtc(DateTime date)
    {
        return DateTime.SpecifyKind(date, DateTimeKind.Utc);
    }

    private string GetCellValue(WorkbookPart workbookPart, Cell cell)
    {
        if (cell == null || cell.CellValue == null) return string.Empty;

        string value = cell.CellValue.InnerText;

        if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
        {
            var stringTable = workbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
            if (stringTable != null)
            {
                value = stringTable.SharedStringTable.ElementAt(int.Parse(value)).InnerText;
            }
        }

        return value;
    }
}
