using StaApi.AutoGeneration.DTO;
using StaApi.Repository.Dictionary;

namespace StaApi.AutoGeneration.Patterns.PresentPatterns;

public class PresentNegativePatterns
{
    private readonly IDictionarySTA _dictionary;

    public PresentNegativePatterns(IDictionarySTA dictionary)
    {
        _dictionary = dictionary;
    }

    public async Task<SlotPatternDto> BuildSubjectNegativeVerb()
    {
        var subject = await _dictionary.GetRandomWordByPos("Subject");

        if (subject == null)
            throw new Exception("No subject words found");

        return new SlotPatternDto
        {
            Code = "subject_negative_verb",
            Slots = ["SUBJECT", "VERB"],
            Template = "{{SUBJECT}} ne {{VERB}}",
            Rules =
            {
                ["SUBJECT"] = new() { Pos = "Subject" },
                ["VERB"] = new() { Pos = "Verb" }
            }
        };
    }
}