using Domain.Enums;
using System.Text.Json.Serialization;

namespace Domain.DTOs;

public class ReportDto
{
    public required Guid Id { get; set; }
    
    public required int Year { get; set; }

    public required int Month { get; set; }

    public required string PassengerName { get; set; }

    public required string PassengerEmail { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required Status Status { get; set; }
}