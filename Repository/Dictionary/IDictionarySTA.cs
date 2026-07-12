using StaApi.Models.Dictionary;

namespace StaApi.Repository.Dictionary;

public interface IDictionarySTA
{
    public Task<List<DictionaryExplanatoryModel>> GetExplanationaryWordsByLetter(string letter);

    public Task<DictionaryExplanatoryModel?> GetRandomWordByPos(string pos);
    
    public Task<List<DictionaryExplanatoryModel>> GetAllExplanatoryWords();
    
    public Task<DictionaryExplanatoryModel?> GetWordById(int id);
    
    public Task<List<DictionaryExplanatoryModel>> SearchWord(string word);

    public Task AddWord(string word, string defenition, string partOfSpeech);
}