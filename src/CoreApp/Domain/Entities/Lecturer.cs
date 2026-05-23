namespace CoreApp.Domain.Entities;

public class Lecturer : Person
{
    public string Title { get; set; } = "";
    public string Faculty { get; set; } = "";

    public List<Course> TaughtCourses { get; set; } = new();
}