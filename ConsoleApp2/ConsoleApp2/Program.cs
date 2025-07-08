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

                    context.Instructers.Add(new Instructer
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

                    Console.Write("Enter course perquisite: ");
                    string perquisite = Console.ReadLine();

                    Console.Write("Enter course instuctor id: ");
                    string instuctor = Console.ReadLine();

                    if (context.Instructers.FirstOrDefault(i => i.Id == Int32.Parse(instuctor)) == null)
                    {
                        Console.WriteLine("Instructor ID not found. Please add the instructor first.");
                    }
                    else
                    {
                        context.Courses.Add(new Course
                        {
                            Name = name,
                            Description = description,
                            Perquisite = perquisite,
                            InstructerId = Int32.Parse(instuctor)
                        });
                        context.SaveChanges();
                    }
                    break;

                case 3:
                    foreach (var Instructer in context.Instructers.Include(i => i.Courses))
                    {
                        var courseNames = string.Join(", ", Instructer.Courses.Select(c => c.Name));
                        Console.WriteLine($"{Instructer.Id}: {Instructer.Name}: {Instructer.Email}: Courses: {courseNames}");
                    }
                    break;

                case 4:
                    foreach (var Course in context.Courses)
                    {
                        Console.WriteLine($"{Course.Id}: {Course.Name}: {Course.Description}: {Course.InstructerId}");
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