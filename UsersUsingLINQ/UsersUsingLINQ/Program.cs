class Program
{
    static void Main()
    {
        var users = new List<User>
        {
            new User { Id = 1, Name = "Ali", Email = "Ali@gmail.com", Age = 18 },
            new User { Id = 2, Name = "Sara", Email = "Sara@gmail.com", Age = 30 },
            new User { Id = 3, Name = "Mona", Email = "Mona@gmail.com", Age = 22 },
            new User { Id = 4, Name = "Omar", Email = "Omar@gmail.com", Age = 35 }
        };

        var filtered1 = from user in users
                        where user.Age > 25
                        orderby user.Age
                        select new { user.Name, user.Age };

        Console.WriteLine("Users older than 25:");
        foreach (var u in filtered1)
        {
            Console.WriteLine($"{u.Name} - {u.Age} years old");
        }

        Console.WriteLine();

        var filtered2 = users.Where(u => u.Age > 25);

        Console.WriteLine("Users older than 25:");
        foreach (var u in filtered2)
        {
            Console.WriteLine($"{u.Name} - {u.Age} years old");
        }

        Console.WriteLine();

        var names = users.Select(u => u.Name).OrderBy(u => u);

        foreach (var u in names)
        {
            Console.WriteLine(u);
        }

        Console.WriteLine();

        var firstAdult = users.First(u => u.Age <= 18);
        Console.WriteLine("The first one in the list who is 18 or less " + firstAdult.Name);

        int totalUsers = users.Count();
        Console.WriteLine("Total number of users are " + totalUsers);
        bool hasAdults = users.Any(u => u.Age >= 18);
        Console.WriteLine("Is there any adults users?");
        Console.WriteLine(hasAdults);
        bool allAdults = users.All(u => u.Age >= 18);
        Console.WriteLine("Are the users all adults?");
        Console.WriteLine(allAdults);

        Console.WriteLine();

        Console.WriteLine("The top 2 users in the list:");
        var top2 = users.Take(2);
        foreach (var u in top2)
        {
            Console.WriteLine(u.Name);
        }
        Console.WriteLine("The list after skiping the first users:");
        var skip2 = users.Skip(1);
        foreach (var u in skip2)
        {
            Console.WriteLine(u.Name);
        }

        Console.WriteLine();

        int maxAge = users.Max(u => u.Age);
        Console.WriteLine("The oldest one in users " + maxAge);
        int minAge = users.Min(u => u.Age);
        Console.WriteLine("The youngest one in users " + minAge);
        double avgAge = users.Average(u => u.Age);
        Console.WriteLine("The avrage of users ages " + avgAge);

    }  
}