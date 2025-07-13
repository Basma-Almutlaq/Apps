public class Instructor
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public List<CourseInstructor>? CourseInstructors { get; set; }
}