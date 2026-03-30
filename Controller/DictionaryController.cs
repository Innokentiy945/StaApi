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
    [Route("getAllWords")]
    public async Task<List<DictionaryExplanatoryModel>> getAllWords()
    {
        return await _dictionarySta.getAllWords();
    }

    [HttpGet]
    [Route("getWordById/{id}")]
    public async Task<DictionaryExplanatoryModel?> getWordById(int id)
    {
        return await _dictionarySta.getWordById(id);
    }

    [HttpPost]
    [Route("searchWord")]
    public async Task<List<DictionaryExplanatoryModel>> searchWord(string word)
    {
        return await _dictionarySta.searchWord(word);
    }

    // [HttpPost]
    // [Route("tempAddData")]
    // public async Task tempUploadData()
    // {
    //     await _dictionarySta.tempUploadData();
    // }

    // [HttpPost]
    // [Route("addUsersWord")]
    // public async Task addWord(string word, string description)
    // {
    //     await _dictionarySta.addWord(word, description);
    // }
}