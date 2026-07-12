using System.Text.Json;
using StaApi.AutoGeneration.DTO;
using StaApi.AutoGeneration.MorphologyEngine.Verbs.Present;
using StaApi.AutoGeneration.Patterns.Registry;
using StaApi.AutoGeneration.Utils;
using StaApi.Repository.Dictionary;

namespace StaApi.AutoGeneration.Service.Generators;

public class SlotBasedGeneratorPresent
{
    private readonly PresentPatternRegistry _presentPatterns;
    private readonly IDictionarySTA _dictionary;

    private readonly PresentItiVerbRule _itiRule = new();
    private readonly PresentAtiVerbRule _atiRule = new();
    private readonly PresentEtiVerbRule _etiRule = new();

    public SlotBasedGeneratorPresent(PresentPatternRegistry presentPatterns, IDictionarySTA dictionary)
    {
        _presentPatterns = presentPatterns;
        _dictionary = dictionary;
    }

    public async Task<GeneratedExerciseDto> GeneratePresentPositiveFillTheBlank()
    {
        var pattern = await _presentPatterns.PresentPositiveRegistryGetRandom();
        var slots = new Dictionary<string, string>();
        string infinitiveForFrontend = null;

        foreach (var slot in pattern.Slots)
        {
            var rule = pattern.Rules[slot];

            var word = await _dictionary.GetRandomWordByPos(rule.Pos);

            if (word == null)
                throw new Exception($"No words found for POS {rule.Pos}");

            if (slot == "SUBJECT")
            {
                slots[slot] = word.Word;
                continue;
            }
            
            if (slot == "VERB")
            {
                var subject = slots["SUBJECT"].ToLower();
                
                infinitiveForFrontend = word.Word;

                if (word.Word.EndsWith("iti"))
                {
                    slots[slot] = _itiRule.Conjugate(word.Word, subject);
                    continue;
                }

                if (word.Word.EndsWith("ati"))
                {
                    slots[slot] = _atiRule.Conjugate(word.Word, subject);
                    continue;
                }

                if (word.Word.EndsWith("eti"))
                {
                    slots[slot] = _etiRule.Conjugate(word.Word, subject);
                    continue;
                }
            }

            slots[slot] = word.Word;
        }

        var missingSlot = pattern.Slots
                .Where(s => s != "SUBJECT")
                .OrderBy(_ => Guid.NewGuid())
                .First();

        var answer = slots[missingSlot];

        var displaySlots = new Dictionary<string, string>(slots);
        displaySlots[missingSlot] = "_____";

        var sentence = SentenceBuilder.Build(displaySlots, pattern);

        var payload = new
        {
            sentence,
            missingSlot,
            infinitiveForFrontend,
            answer,
            task = "fill_in_blank"
        };

        return new GeneratedExerciseDto
        {
            SubtopicId = 1,
            DataJson = JsonSerializer.Serialize(payload),
            XpReward = 10,
        };
    }

    public async Task<GeneratedExerciseDto> GeneratePresentNegativeFillTheBlank()
    {
        var pattern = await _presentPatterns.PresentNegativeRegistryGetRandom();

        var slots = new Dictionary<string, string>();

        string infinitiveForFrontend = null;
        
        foreach (var slot in pattern.Slots)
        {
            var rule = pattern.Rules[slot];

            var word = await _dictionary.GetRandomWordByPos(rule.Pos);

            if (word == null)
                throw new Exception($"No words found for POS {rule.Pos}");

            if (slot == "SUBJECT")
            {
                slots[slot] = word.Word;
                continue;
            }

            if (slot == "VERB")
            {
                var subject = slots["SUBJECT"].ToLower();
                infinitiveForFrontend = word.Word;
                slots[slot] = _itiRule.Conjugate(word.Word, subject);
                continue;
            }

            slots[slot] = word.Word;
        }

        var missingSlot =
            pattern.Slots
                .Where(s => s != "SUBJECT")
                .OrderBy(_ => Guid.NewGuid())
                .First();

        var answer = slots[missingSlot];

        var displaySlots = new Dictionary<string, string>(slots);
        displaySlots[missingSlot] = "_____";

        var sentence = SentenceBuilder.Build(displaySlots, pattern);

        var payload = new
        {
            sentence,
            missingSlot,
            infinitiveForFrontend,
            answer,
            task = "fill_in_blank"
        };

        return new GeneratedExerciseDto
        {
            SubtopicId = 1,
            DataJson = JsonSerializer.Serialize(payload),
            XpReward = 10
        };
    }
}