using Domain.Enums;
using Domain.Common;

namespace Domain.Entities;

public class Report : BaseEntity
{
    public required Status Status { get; set; }

    public required List<Trip> Trip { get; set; }
}