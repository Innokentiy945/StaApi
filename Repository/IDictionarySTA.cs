using StaApi.Models;

namespace StaApi.Repository;

public interface IDictionarySTA
{
    public Task<List<DictionaryExplanatoryModel>> getAllWords();
    
    public Task<DictionaryExplanatoryModel?> getWordById(int id);
    
    public Task<List<DictionaryExplanatoryModel>> searchWord(string word);

    public Task addWord(string word, string defenition, string partOfSpeech);
}