using StaApi.AutoGeneration.DTO;
using StaApi.AutoGeneration.Patterns.PastPatterns;

namespace StaApi.AutoGeneration.Patterns.Registry;

public class PastPatternRegistry
{
    private readonly PastPositivePatterns _positivePatterns;

    public PastPatternRegistry(PastPositivePatterns positivePatterns)
    {
        _positivePatterns = positivePatterns;
    }
    
    public async Task<SlotPatternDto> PastPositiveRegistryGetRandom()
    {
        var patterns = new List<Func<Task<SlotPatternDto>>>
        {
            _positivePatterns.BuildSubjectVerb
        };

        return await patterns
            .OrderBy(_ => Guid.NewGuid())
            .First()();
    }
}