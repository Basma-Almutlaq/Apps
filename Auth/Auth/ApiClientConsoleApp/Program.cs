using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text;
using System.Collections.Generic;

namespace ApiClientConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5137"); 

            Console.WriteLine("Enter your Email: ");
            String email = Console.ReadLine();

            Console.WriteLine("Enter your Password: ");
            String password = Console.ReadLine();

            var loginData = new LoginRequest
            {
                Email = email,
                Password = password
            };

            var response = await client.PostAsJsonAsync("accounts/login", loginData);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Login failed: {response.StatusCode}");
                return;
            }

            var loginResult = await response.Content.ReadFromJsonAsync<LoginResponse>();
            string token = loginResult?.Token ?? "";

            Console.WriteLine("Logged in successfully!");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var products = await client.GetFromJsonAsync<List<Product>>("api/products");

                Console.WriteLine("Products:");
                foreach (var product in products!)
                {
                    Console.WriteLine($"ID: {product.Id}, Name: {product.Name}, Description: {product.Description}, Price: {product.Price}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching products: {ex.Message}");
            }
        }
    }
}
