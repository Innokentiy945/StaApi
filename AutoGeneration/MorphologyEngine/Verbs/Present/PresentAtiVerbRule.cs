namespace StaApi.AutoGeneration.MorphologyEngine.Verbs.Present;

public class PresentAtiVerbRule : IVerbRule
{
    private static readonly Dictionary<string, string> Endings = new()
    {
        ["ja"] = "am",
        ["ti"] = "aš",
        ["on"] = "a",
        ["ona"] = "a",
        ["ono"] = "a",
        ["mi"] = "amo",
        ["vi"] = "ate",
        ["oni"] = "aju"
    };

    public string Conjugate(string infinitive, string subject)
    {
        var stem = infinitive[..^3];

        return stem + Endings[subject];
    }
}