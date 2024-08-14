using MediatR;
using TinyURL.Domain.Urls;

namespace TinyURL.Application.Urls;

public sealed class GetLongUrlQuery : IRequest<string>
{
    public required string ShortUrl { get; set; }
}

public sealed class GetLongUrlQuerydHandler(IUrlRepository urlRepository) : IRequestHandler<GetLongUrlQuery, string>
{
    public async Task<string> Handle(GetLongUrlQuery request, CancellationToken cancellationToken)
    {

        var code = ShortenedUrl.GetCode(request.ShortUrl);

        // Improvement: We should be using caching to improve performace and reduce database calls
        var shortendUrl = await urlRepository.GetShortenedUrlByCode(code, cancellationToken);

        if(shortendUrl == null) 
        {
            throw new ArgumentNullException("Short URL not found.");
        }

        shortendUrl.IncreaseClickCount();

        await urlRepository.UpdateAsync(shortendUrl, cancellationToken);
        
        return shortendUrl.LongUrl;
    }
}
