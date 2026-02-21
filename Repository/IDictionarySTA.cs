using StaApi.Models;

namespace StaApi.Repository;

public interface IDictionarySTA
{
    public Task<List<DictionaryModel>> getAllWords();
    
    public Task<DictionaryModel?> getWordById(int id);
    
    public Task<List<DictionaryModel>> searchWord(string word);
    
    public Task addWord(string word, string defenition, string partOfSpeech);
    
    public Task tempUploadData();
}