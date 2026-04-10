using Microsoft.AspNetCore.Mvc;
using StaApi.Models;
using StaApi.Repository;

namespace StaApi.Controller;

[ApiController]
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
        return await _dictionarySta.getAllExplanatoryWords();
    }
    
    [HttpGet]
    [Route("getAllMorphologyWords")]
    public async Task<List<DictionaryMorphologyModel>> Get1000WordsMorphology()
    {
        return await _dictionarySta.getAllMorphologyWords();
    }

    [HttpGet]
    [Route("getAllExplanatoryWordsByLetter")]
    public async Task<List<DictionaryExplanatoryModel>> GetExplanatoryWordsByletter(string letter)
    {
        return await _dictionarySta.getExplanationaryWordsByLetter(letter);
    }
    
    [HttpGet]
    [Route("getAllMorphologyWordsByLetter")]
    public async Task<List<DictionaryMorphologyModel>> GetMorphologyWordsByletter(string letter)
    {
        return await _dictionarySta.getMorphologyWordsByLetter(letter);
    }

    [HttpGet]
    [Route("getWordById/{id}")]
    public async Task<DictionaryExplanatoryModel?> GetWordById(int id)
    {
        return await _dictionarySta.getWordById(id);
    }

    [HttpPost]
    [Route("searchWord")]
    public async Task<List<DictionaryExplanatoryModel>> SearchWord(string word)
    {
        return await _dictionarySta.searchWord(word);
    }
}