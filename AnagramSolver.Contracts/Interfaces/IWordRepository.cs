using AnagramSolver.Contracts.Models;

namespace AnagramSolver.Contracts.Interfaces
{
    public interface IWordRepository
    {
        HashSet<WordModel> Words { get; set; }
        HashSet<WordModel> LoadDictionary();
        bool WordExists(WordModel word);
        void AddWord(WordModel word);
    }
}
