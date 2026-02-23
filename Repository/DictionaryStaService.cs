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
    
    public async Task<List<DictionaryModel>> getAllWords()
    {
        try
        {
            _logger.LogInformation("Getting all words");
            return await _context.DictionaryItem.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }

        return null;
    }

    public async Task<DictionaryModel?> getWordById(int id)
    {
        var result = await _context.DictionaryItem.FindAsync(id);
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

    public async Task<List<DictionaryModel>> searchWord(string word)
    {
        var result = await _context.DictionaryItem.Where(i => i.Word == word).ToListAsync();
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
            var input = new DictionaryModel
            {
                Id = Guid.NewGuid(),
                Word = word,
                Definition = defenition,
                PartOfSpeech = partOfSpeech
            };
        
            await _context.DictionaryItem.AddAsync(input);
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
            @"Utils/recnik_clean.json");


        var data = JsonConvert.DeserializeObject<List<DictionaryModel>>(json);
        
        var entities = data.Select(x => new DictionaryModel
        {
            Id = Guid.NewGuid(),
            Word = x.Word,
            Definition = x.Definition,
            PartOfSpeech = x.PartOfSpeech
        }).ToList();
        
        await _context.DictionaryItem.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }
}