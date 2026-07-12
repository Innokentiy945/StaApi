using StaApi.AutoGeneration.DTO;
using StaApi.AutoGeneration.Patterns.PresentPatterns;

namespace StaApi.AutoGeneration.Patterns.Registry;

public class PresentPatternRegistry
{
    private readonly PresentPositivePatterns _positivePatterns;
    private readonly PresentNegativePatterns _negativePatterns;

    public PresentPatternRegistry(PresentPositivePatterns positivePatterns, PresentNegativePatterns negativePatterns)
    {
        _positivePatterns = positivePatterns;
        _negativePatterns = negativePatterns;
    }

    public async Task<SlotPatternDto> PresentPositiveRegistryGetRandom()
    {
        var patterns = new List<Func<Task<SlotPatternDto>>>
        {
            _positivePatterns.BuildSubjectVerb
        };

        return await patterns
            .OrderBy(_ => Guid.NewGuid())
            .First()();
    }

    public async Task<SlotPatternDto> PresentNegativeRegistryGetRandom()
    {
        var patterns = new List<Func<Task<SlotPatternDto>>>
        {
            _negativePatterns.BuildSubjectNegativeVerb
        };

        return await patterns
            .OrderBy(_ => Guid.NewGuid())
            .First()();
    }
}