using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StaApi.Models.Dictionary;
using StaApi.Repository.Dictionary;

namespace StaApi.Controllers;

[ApiController]
// [Authorize]
[AllowAnonymous]
[Route("api/dictionaryApi")]
public class DictionaryController : ControllerBase
{
    private IDictionarySTA _dictionarySta;
    private ILogger<DictionaryController> _logger;

    public DictionaryController(IDictionarySTA dictionarySta, ILogger<DictionaryController> logger)
    {
        _dictionarySta = dictionarySta;
        _logger = logger;
    }

    [HttpGet]
    [Route("getAllExplanatoryWords")]
    public async Task<List<DictionaryExplanatoryModel>> Get1000WordsExplanatory()
    {
        _logger.LogInformation($"Getting explanatory words");
        return await _dictionarySta.GetAllExplanatoryWords();
    }

    [HttpGet]
    [Route("getAllExplanatoryWordsByLetter")]
    public async Task<List<DictionaryExplanatoryModel>> GetExplanatoryWordsByletter(string letter)
    {
        _logger.LogInformation($"Getting explanatory words by {letter}");
        return await _dictionarySta.GetExplanationaryWordsByLetter(letter);
    }

    [HttpGet]
    [Route("getWordById/{id}")]
    public async Task<DictionaryExplanatoryModel?> GetWordById(int id)
    {
        _logger.LogInformation($"Getting word by id {id}");
        return await _dictionarySta.GetWordById(id);
    }

    [HttpPost]
    [Route("searchWord")]
    public async Task<List<DictionaryExplanatoryModel>> SearchWord(string word)
    {
        _logger.LogInformation($"Searching word {word}");
        return await _dictionarySta.SearchWord(word);
    }
}