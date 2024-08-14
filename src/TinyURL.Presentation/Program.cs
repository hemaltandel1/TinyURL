using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;
using TinyURL.Application.Extensions;
using TinyURL.Infrastructure.Extensions;

namespace TinyURL.Presentation;

public class Program
    {
        private static IUrlShortener _urlShortener;

        public static async Task Main(string[] args)
        {
        // Setup DI
        var serviceProvider = ConfigureServices();

            // Resolve the dependency
            _urlShortener = serviceProvider.GetRequiredService<IUrlShortener>();

            // Start the application
            await RunMenuAsync();
        }

    public static ServiceProvider ConfigureServices() 
    {
        return new ServiceCollection()
                    .AddInfrastructureServices()
                    .AddApplicationServices()
                    .AddSingleton<IUrlShortener, UrlShortener>()
                    .AddMediatR(cfg =>
                    {
                        //Register the API Assembly, one IRequestHandler from the Application Assembly, one type from Infrastructure 
                        cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly, typeof(Application.AssemblyReference).Assembly);
                    })
                    .BuildServiceProvider();
    }

        private static async Task RunMenuAsync()
        {

            var cts = new CancellationTokenSource();
            var token = cts.Token;
            
            while (true)
            {
                Console.WriteLine("\nMenu:");
                Console.WriteLine("1. Generate Short URL");
                Console.WriteLine("2. Get Long URL from Short URL");
                Console.WriteLine("3. Delete Short URL");
                Console.WriteLine("4. Statistics");
                Console.WriteLine("5. Exit");
                Console.Write("Select an option: ");

                var input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        await GenerateShortUrlAsync(token);
                        break;
                    case "2":
                        await GetLongUrlAsync(token);
                        break;
                    case "3":
                        await DeleteShortUrlAsync(token);
                        break;
                    case "4":
                        await GetStatisticsAsync(token);
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

    private static async Task GenerateShortUrlAsync(CancellationToken cancellationToken)
    {
        try
        {
            Console.Write("Enter the long URL: ");
            var longUrl = Console.ReadLine();

            Console.Write("Enter a custom short URL (optional): ");
            var customUrl = Console.ReadLine();

            if (!Uri.IsWellFormedUriString(longUrl, UriKind.Absolute))
            {
                Console.WriteLine("Invalid long URL format.");
                return;
            }

            var shortUrl = await _urlShortener.GenerateShortUrlAsync(longUrl, customUrl, cancellationToken);
            Console.Write($"Generated Short URL: {shortUrl} \n\n");
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to generate short URL: {ex.Message}");
        }
    }

    private static async Task GetLongUrlAsync(CancellationToken cancellationToken)
    {
        try
        {
            Console.Write("Enter the short URL: ");
            var shortUrl = Console.ReadLine();

            if (!IsValidShortUrl(shortUrl))
            {
                Console.WriteLine("Invalid short URL format.");
                return;
            }

            var longUrl = await _urlShortener.GetLongUrlAsync(shortUrl, cancellationToken);
            Console.Write($"Found related Long URL: {longUrl} \n\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to retrieve long URL: {ex.Message}");
        }
    }      

    private static async Task DeleteShortUrlAsync(CancellationToken cancellationToken)
    {
        try
        {
            Console.Write("Enter the short URL to delete: ");
            var shortUrl = Console.ReadLine();

            if (!IsValidShortUrl(shortUrl))
            {
                Console.WriteLine("Invalid short URL format.");
                return;
            }

            var deleted = await _urlShortener.DeleteShortUrlAsync(shortUrl, cancellationToken);
            if(deleted)
            {
                Console.WriteLine($"Short URL deleted successfully.");
            }
            else
            {
                Console.WriteLine($"Short URL failed to delete.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to delete short URL: {ex.Message}");
        }
    }

    private static async Task GetStatisticsAsync(CancellationToken cancellationToken)
    {
        try
        {
            Console.Write("Enter the short URL to get statistics: ");
            var shortUrl = Console.ReadLine();

            if (!IsValidShortUrl(shortUrl))
            {
                Console.WriteLine("Invalid short URL format.");
                return;
            }

            var count = await _urlShortener.GetStatisticsAsync(shortUrl, cancellationToken);
            Console.WriteLine($"Long URL Clicked : {count}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to retrieve statistics: {ex.Message}");
        }
    }

    private static bool IsValidShortUrl(string shortUrl)
    {
        var regex = new Regex(@"^https:\/\/tiny-url\.com\/\w+$");
        return regex.IsMatch(shortUrl);
    }
}

