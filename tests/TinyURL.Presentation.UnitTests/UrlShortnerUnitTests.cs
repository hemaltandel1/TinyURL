using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyURL.Presentation.UnitTests;


public class UrlShortnerUnitTests : UnitTestsBase
{
    private readonly IUrlShortener _urlShortener;

    public UrlShortnerUnitTests()
    {
        _urlShortener = _serviceProvider.GetRequiredService<IUrlShortener>();
    }

    [Fact]
    public async Task GenerateShortUrlAsync_ShouldReturnShortUrl()
    {
        // Arrange
        var longUrl = "https://example.com";
        
        // Act
        var result = await _urlShortener.GenerateShortUrlAsync(longUrl);

        // Assert
        result.ShouldNotBeNullOrWhiteSpace(result);
        result.ShouldStartWith("https://tiny-url.com/");
    }

    [Fact]
    public async Task GenerateShortUrlAsync_ShouldReturnCustomShortUrl_WhenProvided()
    {
        // Arrange
        var longUrl = "https://example.com";
        var customUrl = "abc123";
        var expectedUrl = "https://tiny-url.com/abc123";

        // Act
        var result = await _urlShortener.GenerateShortUrlAsync(longUrl, customUrl);

        // Assert
        result.ShouldBe(expectedUrl);
    }

    [Fact]
    public async Task DeleteShortUrlAsync_ShouldReturnTrue_WhenExists()
    {
        // Arrange
        var longUrl = "https://example.com";
        

        // Act
        var shortUrl = await _urlShortener.GenerateShortUrlAsync(longUrl);
        var result = await _urlShortener.DeleteShortUrlAsync(shortUrl);

        // Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public async Task GetLongUrlAsync_ShouldReturnLongUrl()
    {
        // Arrange
        
        var longUrl = "https://example.com";

        // Act
        var shortUrl = await _urlShortener.GenerateShortUrlAsync(longUrl);
        var result = await _urlShortener.GetLongUrlAsync(shortUrl);

        // Assert
        result.ShouldBe(longUrl);
    }

    [Fact]
    public async Task GetStatisticsAsync_ShouldReturnStatistics()
    {
        // Arrange
        var longUrl = "https://example.com";
        var expectedStatistics = 2;

        // Act
        var shortUrl = await _urlShortener.GenerateShortUrlAsync(longUrl);
        await _urlShortener.GetLongUrlAsync(shortUrl);
        await _urlShortener.GetLongUrlAsync(shortUrl);
        var result = await _urlShortener.GetStatisticsAsync(shortUrl);

        // Assert
        result.ShouldBe(expectedStatistics);
    }
}
