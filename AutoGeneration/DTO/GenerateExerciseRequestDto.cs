namespace StaApi.AutoGeneration.DTO;

public class GenerateExerciseRequestDto
{
    public int NumberOfExercises { get; set; }
}

public class GenerateExercisesResponseDto
{
    public int GeneratedCount { get; set; }

    // CHANGED: unified DTO used instead of legacy item wrapper
    public List<GeneratedExerciseDto> Exercises { get; set; } = [];
}

public class GeneratedExerciseDto
{
    public Guid ExerciseInstanceId { get; init; }
    public ulong SubtopicId { get; init; }
    public uint XpReward { get; init; }
    public string DataJson { get; init; } = string.Empty;
}