using StaApi.Models;
using StaApi.Models.Dictionary;

namespace StaApi.Repository;

public interface IDictionarySTA
{
    public Task<List<DictionaryExplanatoryModel>> getExplanationaryWordsByLetter(string letter);

    public Task<List<DictionaryMorphologyModel>> GetMorphologyWordsByLetter(string letter, DateTime? lastCreatedAt = null, Guid? lastId = null, int pageSize = 100);
    
    public Task<List<DictionaryExplanatoryModel>> getAllExplanatoryWords();
    
    public Task<List<DictionaryMorphologyModel>> getAllMorphologyWords();
    
    public Task<DictionaryExplanatoryModel?> getWordById(int id);
    
    public Task<List<DictionaryExplanatoryModel>> searchWord(string word);

    public Task addWord(string word, string defenition, string partOfSpeech);
}