using System.ComponentModel.Design;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StaApi.Context;
using StaApi.Models;

namespace StaApi.Repository;

public class DictionaryStaService : IDictionarySTA
{
    private DictionaryContext _context;
    private ILogger<DictionaryStaService> _logger;

    public DictionaryStaService(DictionaryContext context, ILogger<DictionaryStaService> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task<List<DictionaryExplanationaryModel>> getAllWords()
    {
        try
        {
            _logger.LogInformation("Getting all words");
            int limitOfWords = 1000;
            return await _context.DictionaryExplanatoryItem.OrderByDescending(x => x.Id).Take(limitOfWords).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }

        return null;
    }

    public async Task<DictionaryExplanationaryModel?> getWordById(int id)
    {
        var result = await _context.DictionaryExplanatoryItem.FindAsync(id);
        try
        {
            _logger.LogInformation("Getting word by id");
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
        
        return null;
    }

    public async Task<List<DictionaryExplanationaryModel>> searchWord(string word)
    {
        var result = await _context.DictionaryExplanatoryItem.Where(i => i.Word == word).ToListAsync();
        try
        {
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
        
        return null;
    }

    public async Task addWord(string word, string defenition, string partOfSpeech)
    {
        try
        {
            var input = new DictionaryExplanationaryModel
            {
                Id = Guid.NewGuid(),
                Word = word,
                Definition = defenition,
                Pos = partOfSpeech
            };
        
            await _context.DictionaryExplanatoryItem.AddAsync(input);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
    }
    
    public async Task tempUploadData()
    {

        var json = await File.ReadAllTextAsync(
            @"Repository/srLexResult.json");


        var data = JsonConvert.DeserializeObject<List<DictionaryMorphologyModel>>(json);
        
        var entities = data.Select(x => new DictionaryMorphologyModel
        {
            Id = x.Id,
            Wordform = x.Wordform,
            Lemma = x.Lemma,
            Msd = x.Msd,
            Type = x.Type,
            Upos = x.Upos,
            Features = x.Features,
            Morph = x.Morph,
            Frequency = x.Frequency,
            PerMillion = x.PerMillion
        }).ToList();
        
        await _context.DictionaryMorphologyItem.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }
}