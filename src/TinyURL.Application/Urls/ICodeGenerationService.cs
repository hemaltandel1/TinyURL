namespace TinyURL.Application.Urls;

public interface ICodeGenerationService
{
    Task<string> GenerateUniqueCodeAsync();
}
