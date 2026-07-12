namespace StaApi.AutoGeneration.MorphologyEngine.Verbs.Present;

public class PresentEtiVerbRule : IVerbRule
{
    private static readonly Dictionary<string, string> Endings = new()
    {
        ["ja"] = "em",
        ["ti"] = "eš",
        ["on"] = "e",
        ["ona"] = "e",
        ["ono"] = "e",
        ["mi"] = "emo",
        ["vi"] = "ete",
        ["oni"] = "u"
    };

    public string Conjugate(string infinitive, string subject)
    {
        var stem = infinitive[..^3];

        return stem + Endings[subject];
    }
}