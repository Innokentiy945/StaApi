using System.Text.Json;
using StaApi.AutoGeneration.DTO.RequestResponseDTO;

namespace StaApi.AutoGeneration.Validator;

public class ExerciseValidator 
{
    
    public bool Validate(ExerciseDataDto data, string userAnswer)
    {
        var correct = Normalize(data.Answer);
        var user = Normalize(userAnswer);
    
        Console.WriteLine($"[VALIDATION]");
        Console.WriteLine($"EXPECTED: '{correct}'");
        Console.WriteLine($"USER:     '{user}'");
        
        return string.Equals(
            data.Answer?.Trim(),
            userAnswer?.Trim(),
            StringComparison.OrdinalIgnoreCase
        );
    }
    
    private static string Normalize(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }
        
        return input.Trim().ToLowerInvariant().Replace("  ", " ");
    }
}