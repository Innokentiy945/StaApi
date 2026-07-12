using System.Text.Json.Serialization;

namespace StaApi.AutoGeneration.DTO.RequestResponseDTO;

public class ExerciseDataDto
{
    [JsonPropertyName("sentence")]
    public string Sentence { get; init; } = string.Empty;

    [JsonPropertyName("missingSlot")]
    public string MissingSlot { get; init; } = string.Empty;

    [JsonPropertyName("infinitiveForFrontend")]
    public string InfinitiveForFrontend { get; set; }

    [JsonPropertyName("answer")]
    public string Answer { get; init; } = string.Empty;

    [JsonPropertyName("task")]
    public string Task { get; init; } = string.Empty;

    [JsonPropertyName("xpReward")]
    public uint XpReward { get; init; }

    [JsonPropertyName("explanation")]
    public string? Explanation { get; init; }
}