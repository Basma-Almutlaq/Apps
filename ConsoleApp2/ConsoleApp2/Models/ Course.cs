using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
public class Course
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public List<Course>? Prerequisites { get; set; }  
    public List<CourseInstructor>? CourseInstructors { get; set; }
}