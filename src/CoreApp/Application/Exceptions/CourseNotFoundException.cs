namespace CoreApp.Application.Exceptions;

public class CourseNotFoundException(string message) : Exception(message)
{
}
