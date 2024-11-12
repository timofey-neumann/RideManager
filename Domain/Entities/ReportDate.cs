using Domain.Common;

namespace Domain.Entities;

public class ReportDate : BaseEntity
{
    public required int Year { get; set; }

    public required int Month { get; set; }

    public required List<Trip> Trip { get; set; }
}