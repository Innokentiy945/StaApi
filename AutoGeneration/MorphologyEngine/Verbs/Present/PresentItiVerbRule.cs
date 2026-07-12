namespace StaApi.AutoGeneration.MorphologyEngine.Verbs.Present;

public class PresentItiVerbRule : IVerbRule
{
    private static readonly Dictionary<string, string> Endings = new()
    {
        ["ja"] = "im",
        ["ti"] = "iš",
        ["on"] = "i",
        ["ona"] = "i",
        ["ono"] = "i",
        ["mi"] = "imo",
        ["vi"] = "ite",
        ["oni"] = "e"
    };

    public bool CanHandle(string infinitive)
    {
        return infinitive.EndsWith("iti");
    }

    public string Conjugate(string infinitive, string subject)
    {
        var stem = infinitive[..^3];

        return stem + Endings[subject];
    }
}