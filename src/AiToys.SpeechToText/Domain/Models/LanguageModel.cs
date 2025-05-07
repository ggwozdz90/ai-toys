namespace AiToys.SpeechToText.Domain.Models;

internal sealed record LanguageModel(string Code, string Name)
{
    public override string ToString() => Name;
}
