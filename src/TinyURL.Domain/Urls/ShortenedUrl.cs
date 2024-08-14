namespace TinyURL.Domain.Urls;

public class ShortenedUrl
{
    private const string Host = "https://tiny-url.com/";
    public ShortenedUrl(Guid id, string longUrl, string shortUrl, string code, DateTime createdOnUtc)
    {
        Id = id;
        LongUrl = longUrl;
        ShortUrl = shortUrl;
        Code = code;
        CreatedOnUtc = createdOnUtc;
    }

    public Guid Id { get; init; }

    public string LongUrl { get; init; } 

    public string ShortUrl { get; init; }

    public string Code { get; init; } 

    public DateTime CreatedOnUtc { get; init; }

    public int ClickCount { get; private set; }

    public static ShortenedUrl Create(string longUrl, string code) {
        return new ShortenedUrl(Guid.NewGuid(), longUrl, Host+code, code, DateTime.UtcNow );
    }

    public static string GetCode(string shortUrl)
    {
        // Assume that the code is the part after the last '/'
        var uri = new Uri(shortUrl);
        var code = uri.Segments.Last(); // Get the last segment of the URL

        return code;
    }

    public void IncreaseClickCount() {
        this.ClickCount++;
    }

}