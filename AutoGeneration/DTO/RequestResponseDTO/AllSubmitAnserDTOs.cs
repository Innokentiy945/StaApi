namespace StaApi.AutoGeneration.DTO.RequestResponseDTO;

public sealed class SubmitAnswerRequestDto
{
    public Guid ExerciseInstanceId { get; init; }
    public ulong SubtopicId { get; init; }
    public string DataJson { get; init; } = string.Empty;
    
    public string Answer { get; init; } = string.Empty;
    
    public uint TimeSpentSeconds { get; init; }
}

public class SubmitAnswerResponseDto
{
    public bool IsCorrect { get; set; }

    public decimal ScorePercent { get; set; }

    public uint XpEarned { get; set; }

    public string? Explanation { get; set; }
}