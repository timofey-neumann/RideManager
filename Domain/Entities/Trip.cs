using Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Trip : BaseEntity
{
    [MaxLength(100)]
    public required string PassengerName { get; set; }

    [EmailAddress]
    public required string PassengerEmail { get; set; }

    public required DateTime StartDateTime { get; set; }

    [MaxLength(255)]
    public required string StartAddress { get; set; }

    public required DateTime EndDateTime { get; set; }

    [MaxLength(255)]
    public required string EndAddress { get; set; }

    public required decimal CostWithoutVAT { get; set; }

    public required decimal VAT { get; set; }

    public required bool IsPersonal { get; set; }
    public DateTime? PersonalStatusSetAt { get; set; }

    public Guid? ReportId { get; set; }
    public Report? Report { get; set; }

    public required ReportDate ReportDate { get; set; }
}