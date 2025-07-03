using Microsoft.EntityFrameworkCore;
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
            Console.WriteLine("1. Add Patient");
            Console.WriteLine("2. Edit Patient Info");
            Console.WriteLine("3. List Patients");
            Console.WriteLine("4. Exit");
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
                    Console.Write("Enter name: ");
                    string name = Console.ReadLine();

                    Console.Write("Enter email: ");
                    string email = Console.ReadLine();

                    context.Patient.Add(new Patients
                    {
                        Name = name,
                        Email = email

                    });
                    context.SaveChanges();
                    break;
                    
                case 2:
                    Console.WriteLine("Enter patient Id");
                    string id = Console.ReadLine();
                    Patients patient = context.Patient.SingleOrDefault(p => p.Id == Int32.Parse(id));

                    Console.WriteLine("Enter new patient name");
                    string nName = Console.ReadLine();
                    Console.WriteLine("Enter new patient email");
                    string nEmail = Console.ReadLine();

                    if (patient != null)
                    {
                        patient.Name = nName;
                        patient.Email = nEmail;

                        context.SaveChanges();
                    }
                    break;

                case 3:
                    foreach (var Patients in context.Patient)
                    {
                        Console.WriteLine($"{Patients.Id}: {Patients.Name}: {Patients.Email}");
                    }
                    break;
                case 4:
                    Console.WriteLine("Exiting...");
                    break;

                default:
                    Console.WriteLine("Invalid number. Try again.");
                    break;
            }

        } while (choice != 4);

    }
}