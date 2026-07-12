namespace StaApi.AutoGeneration.MorphologyEngine.Verbs.Past;

public class PastAtiVerbRule 
{

    public string Conjugate(string infinitive, Person person, Number number, Gender gender)
    {
        var stem = infinitive[..^2]; // pisati -> pisa

        return $"{Endings.GetAuxiliary(person, number)} {stem}{Endings.GetEnding(number, gender)}";
    }
}