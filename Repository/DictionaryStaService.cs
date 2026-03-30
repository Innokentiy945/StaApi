using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    
    public async Task<List<DictionaryExplanatoryModel>> getAllWords()
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

    public async Task<DictionaryExplanatoryModel?> getWordById(int id)
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

    public async Task<List<DictionaryExplanatoryModel>> searchWord(string word)
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
            var input = new DictionaryExplanatoryModel
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
    
    
    // public async Task tempUploadData()
    // {
    //     const int batchSize = 1000; 
    //     var batch = new List<DictionaryMorphologyModel>();
    //
    //     using var stream = File.OpenText("");
    //     using var reader = new JsonTextReader(stream);
    //
    //     await reader.ReadAsync(); 
    //
    //     while (await reader.ReadAsync())
    //     {
    //         if (reader.TokenType == JsonToken.StartObject)
    //         {
    //             var obj = JObject.Load(reader);
    //
    //             var entity = new DictionaryMorphologyModel
    //             {
    //                 Id = Guid.Parse((string)obj["id"]),
    //                 Wordform = (string)obj["wordform"],
    //                 Lemma = (string)obj["lemma"],
    //                 Msd = (string)obj["msd"],
    //                 Type = (string)obj["type"],
    //                 Upos = (string)obj["upos"],
    //                 Features = obj["features"] != null ? obj["features"].ToString(Formatting.None) : "{}",
    //                 Morph = obj["morph"] != null ? obj["morph"].ToString(Formatting.None) : "{}",
    //                 Frequency = obj["frequency"] != null ? (double)obj["frequency"] : 0,
    //                 PerMillion = obj["per_million"] != null ? (double)obj["per_million"] : 0
    //             };
    //
    //             batch.Add(entity);
    //
    //             if (batch.Count >= batchSize)
    //             {
    //                 await _context.DictionaryMorphologyItem.AddRangeAsync(batch);
    //                 await _context.SaveChangesAsync();
    //                 _context.ChangeTracker.Clear();
    //                 batch.Clear();
    //             }
    //         }
    //         else if (reader.TokenType == JsonToken.EndArray)
    //         {
    //             break; 
    //         }
    //     }
    //
    //
    //     if (batch.Count > 0)
    //     {
    //         await _context.DictionaryMorphologyItem.AddRangeAsync(batch);
    //         await _context.SaveChangesAsync();
    //         _context.ChangeTracker.Clear();
    //     }
    // }
}