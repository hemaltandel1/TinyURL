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

    // Improvement: It will increase latency as we are checking each generated code agains database.
    // We can improve it by pre generating unique code in database.
    // Another improvement point would be use fixed number of iteration instead of infinite loop. Throw exception after a few collission in a row.
    public async Task<string> GenerateUniqueCodeAsync()
    {
        var codeChars = new char[Length];
        
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