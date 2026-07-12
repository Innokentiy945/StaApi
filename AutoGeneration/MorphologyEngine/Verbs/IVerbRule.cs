namespace StaApi.AutoGeneration.MorphologyEngine.Verbs;

public interface IVerbRule
{

    string Conjugate(string infinitive, string subject);
}