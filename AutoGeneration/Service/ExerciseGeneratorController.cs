using Microsoft.AspNetCore.Mvc;
using StaApi.AutoGeneration.DTO;
using StaApi.AutoGeneration.DTO.RequestResponseDTO;
using StaApi.AutoGeneration.Validator;

namespace StaApi.AutoGeneration.Service;

[ApiController]
[Route("api/generator")]
public class ExerciseGeneratorController : ControllerBase
{
    private readonly ExerciseGenerationService _service;
    private readonly ILogger<ExerciseGeneratorController> _logger;

    public ExerciseGeneratorController(ExerciseGenerationService service,  ILogger<ExerciseGeneratorController> logger)
    {
        _service = service;
        _logger = logger;
    }
    
    [HttpPost]
    [Route("/submitAnswer")]
    public async Task<SubmitAnswerResponseDto> SubmitAnswer(ulong userId, SubmitAnswerRequestDto requestDto)
    {
        _logger.LogInformation($"Submitting answer for {userId}");
        return await _service.SubmitAnswer(userId, requestDto);
    }

    [HttpPost("presentPositiveFillTheBlank")]
    public async Task<IActionResult> GeneratePresentPositiveFillTheBlank()
    {
        var result = await _service.GeneratePresentPositiveFillTheBlank();
        return Ok(result);
    }

    [HttpPost("presentNegativeFillTheBlank")]
    public async Task<IActionResult> GeneratePresentNegativeFillTheBlank()
    {
        var result = await _service.GeneratePresentNegativeFillTheBlank();
        return Ok(result);
    }
    
    
}