using StaApi.Models;

namespace StaApi.Repository;

public interface IDictionarySTA
{
    public Task<List<DictionaryExplanatoryModel>> getExplanationaryWordsByLetter(string letter);
    
    public Task<List<DictionaryMorphologyModel>> getMorphologyWordsByLetter(string letter);
    
    public Task<List<DictionaryExplanatoryModel>> getAllExplanatoryWords();
    
    public Task<List<DictionaryMorphologyModel>> getAllMorphologyWords();
    
    public Task<DictionaryExplanatoryModel?> getWordById(int id);
    
    public Task<List<DictionaryExplanatoryModel>> searchWord(string word);

    public Task addWord(string word, string defenition, string partOfSpeech);
}