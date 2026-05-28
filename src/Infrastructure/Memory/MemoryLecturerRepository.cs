using CoreApp.Application.Repositories;
using CoreApp.Domain.Entities;

namespace Infrastructure.Memory;

public class MemoryLecturerRepository : MemoryGenericRepository<Lecturer>, ILecturerRepository
{
    public static readonly Guid LecturerNowakId = Guid.Parse("0d7c4923-0ed0-4db9-a392-4b3c520ef77e");
    public static readonly Guid LecturerKowalskaId = Guid.Parse("05f64714-3ac1-4327-8e02-fd519844126f");

    public MemoryLecturerRepository()
    {
        _data.Add(LecturerNowakId, new Lecturer
        {
            Id = LecturerNowakId,
            FirstName = "Jan",
            LastName = "Nowak",
            NationalId = "75010112345",
            Email = "jan.nowak@wsei.edu.pl",
            Title = "dr inż.",
            Faculty = "Informatyka"
        });

        _data.Add(LecturerKowalskaId, new Lecturer
        {
            Id = LecturerKowalskaId,
            FirstName = "Anna",
            LastName = "Kowalska",
            NationalId = "78020254321",
            Email = "anna.kowalska@wsei.edu.pl",
            Title = "mgr",
            Faculty = "Matematyka"
        });
    }
}
