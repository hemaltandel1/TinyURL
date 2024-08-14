using MediatR;
using Microsoft.Extensions.Logging;
using TinyURL.Domain.Urls;

namespace TinyURL.Application.Urls;

public sealed class GetLongUrlQuery : IRequest<string>
{
    public string ShortUrl { get; set; }
}

public sealed class GetLongUrlQuerydHandler(IUrlRepository urlRepository) : IRequestHandler<GetLongUrlQuery, string>
{
    public async Task<string> Handle(GetLongUrlQuery request, CancellationToken cancellationToken)
    {

        var code = ShortenedUrl.GetCode(request.ShortUrl);

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
