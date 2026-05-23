using Microsoft.EntityFrameworkCore;
using StaApi.Context;
using StaApi.Models;
using StaApi.Models.Dictionary;

namespace StaApi.Repository;

public class DictionaryStaService : IDictionarySTA
{
    private DictionaryContext _context;
    private ILogger<DictionaryStaService> _logger;
    private int limitOfWords = 1000;

    public DictionaryStaService(DictionaryContext context, ILogger<DictionaryStaService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<DictionaryExplanatoryModel>> getExplanationaryWordsByLetter(string letter)
    {
        
        var result = _context.DictionaryExplanatoryItem
            .OrderByDescending(x => x.Id)
            .Where(x => x.Word.StartsWith(letter)); 
        //.Take(limitOfWords)
        try
        {
            _logger.LogInformation("Getting explanatory words by letter");
            return await result.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }

        return null;
    }
    
    public async Task<List<DictionaryMorphologyModel>> GetMorphologyWordsByLetter(string letter, DateTime? lastCreatedAt = null, Guid? lastId = null, int pageSize = 100)
    {
        var query = _context.DictionaryMorphologyItem
            .AsNoTracking()
            .Where(x => x.Lemma.StartsWith(letter));

        if (lastCreatedAt.HasValue && lastId.HasValue)
        {
            query = query.Where(x =>
                x.CreatedAt < lastCreatedAt ||
                (x.CreatedAt == lastCreatedAt && x.Id.CompareTo(lastId) < 0));
        }

        return await query
            .OrderByDescending(x => x.CreatedAt)
            .ThenByDescending(x => x.Id)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<List<DictionaryExplanatoryModel>> getAllExplanatoryWords()
    {
        try
        {
            _logger.LogInformation("Getting 1000 words from Explanatory Dictionary");
            return await _context.DictionaryExplanatoryItem.OrderByDescending(x => x.Id).Take(limitOfWords).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }

        return null;
    }
    
    public async Task<List<DictionaryMorphologyModel>> getAllMorphologyWords()
    {
        try
        {
            _logger.LogInformation("Getting 1000 words from Morphology Dictionary");
            return await _context.DictionaryMorphologyItem.OrderByDescending(x => x.Id).Take(limitOfWords).ToListAsync();
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
}