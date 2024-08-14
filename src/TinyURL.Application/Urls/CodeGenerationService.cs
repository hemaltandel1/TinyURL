using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyURL.Domain.Urls;

namespace TinyURL.Application.Urls;

internal sealed class CodeGenerationService : ICodeGenerationService
{
    public const int Length = 7;
    public const string Alphabet =
        "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    private readonly Random _random = new();

    private readonly IUrlRepository _urlRepository;

    public CodeGenerationService(IUrlRepository urlRepository)
    {
        _urlRepository = urlRepository;
    }

    public async Task<string> GenerateUniqueCodeAsync()
    {
        var codeChars = new char[Length];
        
        // todo add const
        int maxValue = Alphabet.Length;

        while (true)
        {
            for (var i = 0; i < Length; i++)
            {
                var randomIndex = _random.Next(maxValue);

                codeChars[i] = Alphabet[randomIndex];
            }

            var code = new string(codeChars);

            if (!await _urlRepository.CodeExistAsync(code))
            {
                return code;
            }
        }
    }
}
