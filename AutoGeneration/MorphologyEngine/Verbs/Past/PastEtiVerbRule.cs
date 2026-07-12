namespace StaApi.AutoGeneration.MorphologyEngine.Verbs.Past;

public class PastEtiVerbRule
{
    public string Conjugate(string infinitive, Person person, Number number, Gender gender)
    {
        var stem = infinitive[..^2]; // voleti -> vole

        return $"{Endings.GetAuxiliary(person, number)} {stem}{Endings.GetEnding(number, gender)}";
    }
}