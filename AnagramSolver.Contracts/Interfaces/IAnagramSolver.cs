namespace AnagramSolver.Contracts.Interfaces
{
    public interface IAnagramSolver
    {
        IList<string> GetAnagrams(string myWords);
        Task<List<string>> RequestAnagrams(string myWords);
    }
}
