namespace Domain.Entities;

public class TripDto
{
    public required Guid Id { get; set; }

    public required string PassengerName { get; set; }

    public required string PassengerEmail { get; set; }

    public required DateTime StartDateTime { get; set; }

    public required string StartAddress { get; set; }

    public required DateTime EndDateTime { get; set; }

    public required string EndAddress { get; set; }

    public required decimal CostWithoutVAT { get; set; }

    public required decimal VAT { get; set; }

    public required bool IsPersonal { get; set; }
}