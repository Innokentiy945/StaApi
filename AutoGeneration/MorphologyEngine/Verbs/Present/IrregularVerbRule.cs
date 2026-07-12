namespace StaApi.AutoGeneration.MorphologyEngine.Verbs.Present;

public class IrregularVerbRule : IVerbRule
{
    private readonly Dictionary<string, Dictionary<string, string>>
        _irregulars =
            new()
            {
                ["biti"] = new()
                {
                    ["ja"] = "sam",
                    ["ti"] = "si",
                    ["on"] = "je",
                    ["ona"] = "je",
                    ["ono"] = "je",
                    ["mi"] = "smo",
                    ["vi"] = "ste",
                    ["oni"] = "su"
                },

                ["imati"] = new()
                {
                    ["ja"] = "imam",
                    ["ti"] = "imaš",
                    ["on"] = "ima",
                    ["ona"] = "ima",
                    ["ono"] = "ima",
                    ["mi"] = "imamo",
                    ["vi"] = "imate",
                    ["oni"] = "imaju"
                }
            };

    public bool CanHandle(string infinitive)
    {
        return _irregulars.ContainsKey(
            infinitive.ToLowerInvariant());
    }

    public string Conjugate(
        string infinitive,
        string subject)
    {
        infinitive =
            infinitive.ToLowerInvariant();

        subject =
            subject.ToLowerInvariant();

        return _irregulars[infinitive][subject];
    }

}