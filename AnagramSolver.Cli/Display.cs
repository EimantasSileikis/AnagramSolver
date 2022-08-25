using AnagramSolver.Contracts.Interfaces.Core;

namespace AnagramSolver.Cli
{
    public class Display : IDisplay
    {
        public delegate void Print(string message);
        public Print GetPrint;

        public Display(Print del)
        {
            GetPrint = new Print(del);
        }

        public void Write(string message)
        {
            GetPrint.Invoke(message);
        }
    }
}
