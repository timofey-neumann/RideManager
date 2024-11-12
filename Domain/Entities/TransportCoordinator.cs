using Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class TransportCoordinator : BaseEntity
{
    [EmailAddress]
    public required string Email { get; set; }
}