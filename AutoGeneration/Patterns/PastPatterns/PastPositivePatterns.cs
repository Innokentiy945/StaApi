using StaApi.AutoGeneration.DTO;
using StaApi.Repository.Dictionary;

namespace StaApi.AutoGeneration.Patterns.PastPatterns;

public class PastPositivePatterns
{
    private readonly IDictionarySTA _dictionary;

    public PastPositivePatterns(IDictionarySTA dictionary)
    {
        _dictionary = dictionary;
    }

    public async Task<SlotPatternDto> BuildSubjectVerb()
    {
        //call not from DB
        var subject = await _dictionary.GetRandomWordByPos("Subject");

        if (subject == null)
        {
            throw new Exception("No subject words found");
        }

        return new SlotPatternDto
        {
            Code = "subject_verb",
            Slots = ["SUBJECT", "AUXILIARY", "VERB"],
            Template = "{{SUBJECT}} {{AUXILIARY}} {{VERB}}",
            Rules =
            {
                ["SUBJECT"] = new() { Pos = "Subject" },
                ["AUXILIARY"] = new() { Pos = "Auxiliary" },
                ["VERB"] = new() { Pos = "Verb" }
            }
        };
    }
}