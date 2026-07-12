using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using StaApi.AutoGeneration.Context;
using StaApi.AutoGeneration.DTO;
using StaApi.AutoGeneration.DTO.RequestResponseDTO;
using StaApi.AutoGeneration.Service.Generators;
using StaApi.AutoGeneration.Validator;
using StaApi.Context;

namespace StaApi.AutoGeneration.Service;

public class ExerciseGenerationService
{
    private readonly CoreContext _db;
    private readonly ExerciseValidator  _exerciseValidator;
    private readonly SlotBasedGeneratorPresent _generatorPresent;

    public ExerciseGenerationService(CoreContext db, ExerciseValidator exerciseValidator, SlotBasedGeneratorPresent generatorPresent)
    {
        _db = db;
        _exerciseValidator = exerciseValidator;
        _generatorPresent = generatorPresent;
    }
    
    public async Task<SubmitAnswerResponseDto> SubmitAnswer(ulong userId, SubmitAnswerRequestDto requestDto)
    {
        var data = JsonSerializer.Deserialize<ExerciseDataDto>(requestDto.DataJson);

        if (data is null)
        {
            return new SubmitAnswerResponseDto
            {
                IsCorrect = false,
                ScorePercent = 0m,
                XpEarned = 0,
                Explanation = "Invalid exercise data"
            };
        }
        
        var isCorrect = _exerciseValidator.Validate(data, requestDto.Answer);
        
        var scorePercent = isCorrect ? 100m : 0m;
        
        var xpEarned = isCorrect ? data.XpReward : 0u;

        return new SubmitAnswerResponseDto
        {
            IsCorrect = isCorrect,
            ScorePercent = scorePercent,
            XpEarned = xpEarned,
            Explanation = data.Explanation
        };
    }

    public async Task<GenerateExercisesResponseDto> GeneratePresentPositiveFillTheBlank()
    {
        
        var list = new List<GeneratedExerciseDto>();

        for (int i = 0; i < 10; i++)
        {
            var data = await _generatorPresent.GeneratePresentPositiveFillTheBlank();

            list.Add(new GeneratedExerciseDto
            {
                // CHANGED: instance identity (not persisted, only runtime tracking)
                ExerciseInstanceId = Guid.NewGuid(),

                SubtopicId = data.SubtopicId,

                // CHANGED: XP must come from DataJson (single source of truth rule)
                XpReward = JsonSerializer.Deserialize<ExerciseDataDto>(data.DataJson)!.XpReward,

                // unchanged: DataJson remains canonical payload
                DataJson = data.DataJson
            });
        }

        return new GenerateExercisesResponseDto
        {
            GeneratedCount = list.Count,

            // CHANGED: direct assignment (no intermediate mapping needed)
            Exercises = list
        };
    }

    public async Task<GenerateExercisesResponseDto> GeneratePresentNegativeFillTheBlank()
    {
        // CHANGED: DTO-only collection
        var list = new List<GeneratedExerciseDto>();

        for (int i = 0; i < 10; i++)
        {
            var data = await _generatorPresent.GeneratePresentNegativeFillTheBlank();

            list.Add(new GeneratedExerciseDto
            {
                // CHANGED: runtime-only identifier
                ExerciseInstanceId = Guid.NewGuid(),

                SubtopicId = data.SubtopicId,

                // CHANGED: XP extracted from DataJson (not from external generatorPresent contract)
                XpReward = JsonSerializer.Deserialize<ExerciseDataDto>(data.DataJson)!.XpReward,

                DataJson = data.DataJson
            });
        }

        return new GenerateExercisesResponseDto
        {
            GeneratedCount = list.Count,
            Exercises = list
        };
    }
    
}