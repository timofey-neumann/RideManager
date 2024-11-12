using Microsoft.AspNetCore.Http;

namespace Infrastructure.Helpers;

public class ReportValidator
{
    private static readonly Lazy<ReportValidator> instance = new(() => new ReportValidator());
    private ReportValidator() { }

    public static ReportValidator Instance => instance.Value;

    public List<string> ValidateCreateParameters(int reportMonth, int reportYear)
    {
        return ValidateReportDate(reportMonth, reportYear);
    }

    public List<string> ValidateImportParameters(IFormFile? file, int reportMonth, int reportYear)
    {
        var errors = ValidateReportDate(reportMonth, reportYear);

        if (file == null || file.Length == 0)
            errors.Add("Файл не предоставлен.");
        else if (!file.FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
            errors.Add("Недопустимый формат файла. Ожидается файл .xlsx.");

        return errors;
    }

    private List<string> ValidateReportDate(int reportMonth, int reportYear)
    {
        var errors = new List<string>();

        if (reportMonth < 1 || reportMonth > 12)
            errors.Add("Некорректный месяц. Месяц должен быть в диапазоне от 1 до 12.");

        if (reportYear < 2000 || reportYear > DateTime.Now.Year)
            errors.Add("Некорректный год. Год должен быть в диапазоне от 2000 до текущего.");

        return errors;
    }
}
