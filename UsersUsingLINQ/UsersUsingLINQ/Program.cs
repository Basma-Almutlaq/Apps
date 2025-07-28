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

        var filtered2 = users.Where(u => u.Age > 25);

        Console.WriteLine("Users older than 25:");
        foreach (var u in filtered2)
        {
            Console.WriteLine($"{u.Name} - {u.Age} years old");
        }
    }  
}