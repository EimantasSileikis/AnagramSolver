namespace AnagramSolver.Contracts.Interfaces.Core
{
    public interface IAnagramSolver
    {
        Task<IEnumerable<string>> GetAnagramsAsync(string myWords);
        Task<List<string>> RequestAnagrams(string myWords);
    }
}
