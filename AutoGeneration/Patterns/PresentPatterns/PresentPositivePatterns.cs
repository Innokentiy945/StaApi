using StaApi.AutoGeneration.DTO;
using StaApi.Repository.Dictionary;

namespace StaApi.AutoGeneration.Patterns.PresentPatterns;

public class PresentPositivePatterns
{
    private readonly IDictionarySTA _dictionary;

    public PresentPositivePatterns(IDictionarySTA dictionary)
    {
        _dictionary = dictionary;
    }

    public async Task<SlotPatternDto> BuildSubjectVerb()
    {
        var subject = await _dictionary.GetRandomWordByPos("Subject");

        if (subject == null)
        {
            throw new Exception("No subject words found");
        }

        return new SlotPatternDto
        {
            Code = "subject_verb",
            Slots = ["SUBJECT", "VERB"],
            Template = "{{SUBJECT}} {{VERB}}",
            Rules =
            {
                ["SUBJECT"] = new() { Pos = "Subject" },
                ["VERB"] = new() { Pos = "Verb" }
            }
        };
    }
}