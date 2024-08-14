using MediatR;
using TinyURL.Domain.Urls;

namespace TinyURL.Application.Urls;

public sealed class CreateShortUrlCommand : IRequest<string>
{
    public required string LongUrl { get; set; }
    public string? CustomUrl { get; set; }
}

public sealed class CreateShortUrlCommandHandler : IRequestHandler<CreateShortUrlCommand, string>
{
    private readonly ICodeGenerationService _urlShortningService;
    private readonly IUrlRepository _urlRepository;

    public CreateShortUrlCommandHandler(ICodeGenerationService urlShortningService, IUrlRepository urlRepository)
    {
        _urlShortningService = urlShortningService;
        _urlRepository = urlRepository;
    }

    public async Task<string> Handle(CreateShortUrlCommand request, CancellationToken cancellationToken)
    {
        string? customUrl = null;
        if (!string.IsNullOrWhiteSpace(request.CustomUrl))
        {
            var exist = await _urlRepository.CodeExistAsync(request.CustomUrl);
            if (exist)
            {
                throw new ArgumentException("Custom URL already exists.");
            }
            else
            {
                customUrl = request.CustomUrl;
            }
        }

        // Improvements: There is a minor race condition here, as we generate a unique code first and then insert it into the database.
        // A concurrent request could generate the same unique code and insert it into the database before we complete our transaction.
        // However, the chances of this happening are low, so I decided not to handle that case.
        // We can also solve this problem using optimistic concurrency with EF Core.

        var code = customUrl ?? await _urlShortningService.GenerateUniqueCodeAsync();

        var shortendUrl = ShortenedUrl.Create(request.LongUrl, code);

        await _urlRepository.InsertAsync(shortendUrl);

        return shortendUrl.ShortUrl;
    }
}
