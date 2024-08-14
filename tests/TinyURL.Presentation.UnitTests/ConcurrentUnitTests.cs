using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace TinyURL.Presentation.UnitTests;


public class ConcurrentUnitTests : UnitTestsBase
{
    private readonly IUrlShortener _urlShortener;

    public ConcurrentUnitTests()
    {
        _urlShortener = _serviceProvider.GetRequiredService<IUrlShortener>();
    }

    [Fact]
    public async Task ConcurrentGenerateAndGet_ShouldBeThreadSafe()
    {
        // Arrange
        var longUrls = Enumerable.Range(1, 100000).Select(i => $"https://example.com/{i}").ToList();
        var tasks = new List<Task>();
        var expectedClickedCount = 2;
        
        // Act & Assert
        foreach (var longUrl in longUrls)
        {
            tasks.Add(Task.Run(async () =>
            {
                var shortUrl = await _urlShortener.GenerateShortUrlAsync(longUrl);

                var retrievedUrl = await _urlShortener.GetLongUrlAsync(shortUrl);
                var retrievedUrl2 = await _urlShortener.GetLongUrlAsync(shortUrl);
                var clickedCount = await _urlShortener.GetStatisticsAsync(shortUrl);

                retrievedUrl.ShouldNotBeNullOrWhiteSpace(); ;
                longUrl.ShouldBe(retrievedUrl);
                longUrl.ShouldBe(retrievedUrl2);
                clickedCount.ShouldBe(expectedClickedCount);

            }));
        }

        await Task.WhenAll(tasks);                
    }    
}
