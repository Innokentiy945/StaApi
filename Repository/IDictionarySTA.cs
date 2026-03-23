using StaApi.Models;

namespace StaApi.Repository;

public interface IDictionarySTA
{
    public Task<List<DictionaryExplanationaryModel>> getAllWords();
    
    public Task<DictionaryExplanationaryModel?> getWordById(int id);
    
    public Task<List<DictionaryExplanationaryModel>> searchWord(string word);
    
    public Task addWord(string word, string defenition, string partOfSpeech);
    
    // public Task tempUploadData();
}