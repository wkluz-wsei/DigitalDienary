namespace CoreApp.Domain.Entities;

public abstract class Person : EntityBase
{
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string NationalId { get; set; } = "";
    public string Email { get; set; } = "";
}