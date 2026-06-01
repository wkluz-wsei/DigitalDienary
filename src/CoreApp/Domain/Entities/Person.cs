using CoreApp.Domain.ValueObjects;

namespace CoreApp.Domain.Entities;

public abstract class Person : EntityBase
{
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public required PESEL NationalId { get; set; }
    public string Email { get; set; } = "";
}