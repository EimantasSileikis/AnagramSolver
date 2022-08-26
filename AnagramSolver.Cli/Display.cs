using AnagramSolver.Contracts.Interfaces.Core;

namespace AnagramSolver.Cli
{
    public class Display : IDisplay
    {
        //public delegate void Print(string message);
        //public Print GetPrint;
        private readonly Action<string> _displayAction;

        //public Display(Print del)
        //{
        //    GetPrint = new Print(del);
        //}

        //public void Write(string message)
        //{
        //    GetPrint.Invoke(message);
        //}

        //public void FormattedPrint(CapitalLetterHandler capitalLetterDelegate, string input)
        //{
        //    var capitalizedFirstLetter = capitalLetterDelegate.Invoke(input);
        //    _displayAction.Invoke(capitalizedFirstLetter);
        //}

        public Display(Action<string> displayAction)
        {
            _displayAction = displayAction;
        }

        public void Write(string message)
        {
            _displayAction(message);
        }

        public void FormattedPrint(Func<string, string> capitalizeMethod, string input)
        {
            var capitalizedFirstLetter = capitalizeMethod(input);
            _displayAction(capitalizedFirstLetter);
        }
    }
}
