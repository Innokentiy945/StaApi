namespace StaApi.AutoGeneration.MorphologyEngine.Verbs.Past;

public static class Endings
{
    public static string GetEnding(Number number, Gender gender)
    {
        if (number == Number.Plural)
        {
            return gender switch
            {
                Gender.Masculine => "li",
                Gender.Feminine => "le",
                Gender.Neuter => "la",
                _ => throw new ArgumentOutOfRangeException(nameof(gender))
            };
        }

        return gender switch
        {
            Gender.Masculine => "o",
            Gender.Feminine => "la",
            Gender.Neuter => "lo",
            _ => throw new ArgumentOutOfRangeException(nameof(gender))
        };
    }

    public static string GetAuxiliary(Person person, Number number)
    {
        return (person, number) switch
        {
            (Person.First, Number.Singular) => "sam",
            (Person.Second, Number.Singular) => "si",
            (Person.Third, Number.Singular) => "je",

            (Person.First, Number.Plural) => "smo",
            (Person.Second, Number.Plural) => "ste",
            (Person.Third, Number.Plural) => "su",

            _ => throw new ArgumentOutOfRangeException()
        };
    }
}