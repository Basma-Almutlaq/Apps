using Microsoft.EntityFrameworkCore;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>().HasMany(c => c.Prerequisites).WithMany().UsingEntity(j => j.ToTable("CoursePrerequisites"));

        modelBuilder.Entity<CourseInstructor>().HasKey(ci => new { ci.CourseId, ci.InstructorId });

        modelBuilder.Entity<CourseInstructor>().HasOne(ci => ci.Course).WithMany(c => c.CourseInstructors).HasForeignKey(ci => ci.CourseId);

        modelBuilder.Entity<CourseInstructor>().HasOne(ci => ci.Instructor).WithMany(i => i.CourseInstructors).HasForeignKey(ci => ci.InstructorId);
    }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Instructor> Instructors { get; set; }
    public DbSet<CourseInstructor> CourseInstructors { get; set; }
}