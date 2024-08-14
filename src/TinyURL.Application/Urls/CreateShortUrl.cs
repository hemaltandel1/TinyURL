using MediatR;
using TinyURL.Domain.Urls;

namespace TinyURL.Application.Urls;

public sealed class CreateShortUrlCommand : IRequest<string>
{
    public string Url { get; set; }
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
        // Check if custom code exits, if yes check if it's exist in db. if yes, return error, if not create and save to db
        // generate unique Code
        var code = await _urlShortningService.GenerateUniqueCodeAsync();

        var shortendUrl = ShortenedUrl.Create(request.Url, code);

        await _urlRepository.InsertAsync(shortendUrl);

        return shortendUrl.ShortUrl;
    }
}


