using StaApi.AutoGeneration.DTO;

namespace StaApi.AutoGeneration.Utils;

public static class SentenceBuilder
{
    public static string Build(Dictionary<string, string> slots, SlotPatternDto patternDto)
    {
        var result = patternDto.Template;

        foreach (var slot in slots)
        {
            result = result.Replace(
                $"{{{{{slot.Key}}}}}",
                slot.Value
            );
        }

        return result;
    }
}