using MediatR;
using TinyURL.Application.Urls;

namespace TinyURL.Presentation;

public sealed class UrlShortener : IUrlShortener
{
    private readonly IMediator _mediator;

    public UrlShortener(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<string> GenerateShortUrlAsync(string longUrl, string? customUrl = null, CancellationToken cancellationToken = default)
    {
        var command = new CreateShortUrlCommand()
        {
            LongUrl = longUrl,
            CustomUrl = customUrl,
        };

        var result = await _mediator.Send(command, cancellationToken);

        return result;
    }

    public async Task<bool> DeleteShortUrlAsync(string shortUrl, CancellationToken cancellationToken = default)
    {
        var command = new DeleteShortUrlCommand()
        {
            ShortUrl = shortUrl
        };

        var result = await _mediator.Send(command, cancellationToken);

        return result;
    }


    public async Task<string> GetLongUrlAsync(string shortUrl, CancellationToken cancellationToken = default)
    {
        var query = new GetLongUrlQuery()
        {
            ShortUrl = shortUrl
        };

        var result = await _mediator.Send(query, cancellationToken);

        return result;
    }

    public async Task<int> GetStatisticsAsync(string shortUrl, CancellationToken cancellationToken = default)
    {
        var query = new GetShortUrlStatisticsQuery()
        {
            ShortUrl = shortUrl
        };

        var result = await _mediator.Send(query, cancellationToken);

        return result;
    }
}
