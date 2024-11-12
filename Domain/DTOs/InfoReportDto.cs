namespace Domain.DTOs;

public class InfoReportDto
{
    public required string Year { get; set; }

    public required string Month { get; set; }

    public required string PassengerName { get; set; }

    public required string Code { get; set; }

    public required string VAT { get; set; }
}