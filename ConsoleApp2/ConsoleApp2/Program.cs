using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
class Program
{
    static void Main(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = config.GetConnectionString("DefaultConnection");

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        using var context = new AppDbContext(optionsBuilder.Options);

        int choice;
        do
        {
            Console.WriteLine("1. Add Instructor");
            Console.WriteLine("2. Add Course");
            Console.WriteLine("3. List Instructors");
            Console.WriteLine("4. List Courses");
            Console.WriteLine("5. Exit");
            Console.Write("Enter your choice: ");

            string input = Console.ReadLine();
            bool isValid = int.TryParse(input, out choice);

            if (!isValid)
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                continue;
            }

            switch (choice)
            {
                case 1:

                    Console.Write("Enter instructer name: ");
                    string instructerName = Console.ReadLine();

                    Console.Write("Enter instructer email: ");
                    string instructerEmail = Console.ReadLine();

                    context.Instructors.Add(new Instructor
                    {
                        Name = instructerName,
                        Email = instructerEmail

                    });
                    context.SaveChanges();
                    break;

                case 2:
                    Console.Write("Enter course name: ");
                    string name = Console.ReadLine();

                    Console.Write("Enter course description: ");
                    string description = Console.ReadLine();

                        var course = new Course
                        {
                            Name = name,
                            Description = description,
                            Prerequisites = new List<Course>(),
                            CourseInstructors = new List<CourseInstructor>()
                        };

                    Console.Write("Enter course instuctors Ids seperated by comma. Example: 1, 2: ");
                    string InstructorIds= Console.ReadLine();

                    var instructorIds = InstructorIds.Split(',').Select(id => int.Parse(id.Trim())).ToList();

                    foreach (var instructorId in instructorIds)
                    {
                        var instructor = context.Instructors.Find(instructorId);
                        if (instructor != null)
                        {
                            course.CourseInstructors.Add(new CourseInstructor
                            {
                                Course = course,
                                Instructor = instructor
                            });
                        }
                        else
                        {
                            Console.WriteLine($"Instructor ID {instructorId} not found.");
                        }
                    }

                    Console.Write("Enter course perquisites Ids seperated by comma or leave it empty if none. Example: 1, 2: ");
                    string perquisiteIds = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(perquisiteIds))
                    {
                        var prereqIds = perquisiteIds.Split(',').Select(id => int.Parse(id.Trim())).ToList();

                        foreach (var prereqId in prereqIds)
                        {
                            var prereqCourse = context.Courses.Find(prereqId);
                            if (prereqCourse != null)
                            {
                                course.Prerequisites.Add(prereqCourse);
                            }
                            else
                            {
                                Console.WriteLine($"Prerequisite Course ID {prereqId} not found.");
                            }
                        }
                    }
                        context.Courses.Add(course);
                        context.SaveChanges();
                    break;

                case 3:
                    foreach (var instructor in context.Instructors.Include(i => i.CourseInstructors).ThenInclude(ci => ci.Course))
                    {
                        var courseNames = string.Join(", ", instructor.CourseInstructors.Select(ci => ci.Course?.Name));
                        Console.WriteLine($"{instructor.Id}: {instructor.Name}: {instructor.Email}: Courses: {courseNames}");
                    }
                    break;

                case 4:
                    foreach (var course2 in context.Courses.Include(c => c.CourseInstructors).ThenInclude(ci => ci.Instructor).Include(c => c.Prerequisites))
                    {
                        var instructorNames = string.Join(", ", course2.CourseInstructors.Select(ci => ci.Instructor?.Name));
                        var prerequisiteNames = string.Join(", ", course2.Prerequisites.Select(p => p.Name));
                        
                        Console.WriteLine($"{course2.Id}: {course2.Name}, {course2.Description}");
                        Console.WriteLine($"  Instructors: {instructorNames}");
                        Console.WriteLine($"  Prerequisites: {prerequisiteNames}");
                    }
                    break;
                case 5:
                    Console.WriteLine("Exiting...");
                    break;

                default:
                    Console.WriteLine("Invalid number. Try again.");
                    break;
            }

        } while (choice != 5);

    }
}