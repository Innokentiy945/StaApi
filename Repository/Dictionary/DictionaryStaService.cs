using Microsoft.EntityFrameworkCore;
using StaApi.Context;
using StaApi.Models.Dictionary;

namespace StaApi.Repository.Dictionary;

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

    public async Task<List<DictionaryExplanatoryModel>> GetExplanationaryWordsByLetter(string letter)
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
    
    public async Task<DictionaryExplanatoryModel?> GetRandomWordByPos(string pos)
    {
        var words = await _context.DictionaryExplanatoryItem
            .Where(x => x.Pos == pos)
            .ToListAsync();

        if (words.Count == 0)
            return null;

        return words[
            Random.Shared.Next(words.Count)
        ];
    }
    
    public async Task<List<DictionaryExplanatoryModel>> GetAllExplanatoryWords()
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

    public async Task<DictionaryExplanatoryModel?> GetWordById(int id)
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

    public async Task<List<DictionaryExplanatoryModel>> SearchWord(string word)
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

    public async Task AddWord(string word, string defenition, string partOfSpeech)
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

    // public async Task TempExplWithFrequency()
    // {
    //     try
    //     {
    //         var explList = await _context.DictionaryExplanatoryItem.ToListAsync();
    //         var morphList = await _context.DictionaryMorphologyItem.ToListAsync();
    //
    //         var morphLookup = morphList
    //             .GroupBy(x => x.Lemma)
    //             .ToDictionary(
    //                 x => x.Key,
    //                 x => x.First().Frequency
    //             );
    //
    //         var resultList = new List<DictionaryExplanatoryWithFrequencyModel>();
    //
    //         foreach (var e in explList)
    //         {
    //             double? frequency = 0;
    //
    //             if (morphLookup.ContainsKey(e.Word))
    //             {
    //                 frequency = morphLookup[e.Word];
    //             }
    //
    //             resultList.Add(new DictionaryExplanatoryWithFrequencyModel
    //             {
    //                 Id = Guid.NewGuid(),
    //                 Word = e.Word,
    //                 Definition = e.Definition,
    //                 Pos = e.Pos,
    //                 Frequency = frequency
    //             });
    //         }
    //
    //         resultList = resultList
    //             .OrderBy(x => x.Word)
    //             .ToList();
    //
    //         // await _context.DictionaryExplanatoryWithFrequencyItem.AddRangeAsync(resultList);
    //         // await _context.SaveChangesAsync();
    //     }
    //     catch (Exception e)
    //     {
    //         _logger.LogError(e.Message);
    //         throw;
    //     }
    // }
}