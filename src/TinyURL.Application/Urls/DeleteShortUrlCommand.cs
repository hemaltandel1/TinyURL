using MediatR;
using Microsoft.Extensions.Logging;
using TinyURL.Domain.Urls;

namespace TinyURL.Application.Urls;

public sealed class DeleteShortUrlCommand : IRequest<bool>
{
    public required string ShortUrl { get; set; }
}

public sealed class DeleteShortUrlCommandHandler(IUrlRepository urlRepository) : IRequestHandler<DeleteShortUrlCommand, bool>
{
    public async Task<bool> Handle(DeleteShortUrlCommand request, CancellationToken cancellationToken)
    {
        var code = ShortenedUrl.GetCode(request.ShortUrl);

        var shortendUrl = await urlRepository.GetShortenedUrlByCode(code, cancellationToken);

        if (shortendUrl == null)
        {
            throw new ArgumentNullException("Short URL not found.");
        }

        return await urlRepository.DeleteAsync(shortendUrl.Id, cancellationToken);
    }
}
