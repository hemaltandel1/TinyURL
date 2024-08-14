using MediatR;
using Microsoft.Extensions.Logging;
using TinyURL.Domain.Urls;

namespace TinyURL.Application.Urls;

public sealed class GetShortUrlStatisticsQuery : IRequest<int>
{
    public required string ShortUrl { get; set; }
}

public sealed class GetShortUrlStatisticsQuerydHandler(IUrlRepository urlRepository) : IRequestHandler<GetShortUrlStatisticsQuery, int>
{
    public async Task<int> Handle(GetShortUrlStatisticsQuery request, CancellationToken cancellationToken)
    {

        var code = ShortenedUrl.GetCode(request.ShortUrl);

        var shortendUrl = await urlRepository.GetShortenedUrlByCode(code, cancellationToken);

        if(shortendUrl == null) 
        {
            throw new ArgumentNullException("Short URL not found.");
        }               
        
        return shortendUrl.ClickCount;
    }
}
