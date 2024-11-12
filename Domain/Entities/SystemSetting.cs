using Domain.Common;

namespace Domain.Entities;

public class SystemSetting : BaseEntity
{
    public required int HoursForPersonalTripStatusChange { get; set; }
}