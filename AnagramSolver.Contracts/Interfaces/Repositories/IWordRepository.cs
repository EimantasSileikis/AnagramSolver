using AnagramSolver.Contracts.Models;

namespace AnagramSolver.Contracts.Interfaces.Repositories
{
    public interface IWordRepository
    {
        HashSet<WordModel> LoadDictionary();
        bool WordExists(WordModel word);
        void SaveWord(WordModel word);
    }
}
