using System.ComponentModel.DataAnnotations.Schema;
public class Course
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Perquisite { get; set; }
    public int InstructerId { get; set; }
    [ForeignKey("InstructerId")]
    public Instructer? Instructer { get; set; }
}