using AnagramSolver.Contracts.Interfaces.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Cli
{
    public class DisplayWithEvents : IDisplay
    {
        public event EventHandler<DisplayEventArgs> Print;

        public void Write(string message)
        {
            if(Print != null)
                Print(this, new DisplayEventArgs { Message = message });
        }

        public void FormattedPrint(Func<string, string> capitalizeMethod, string input)
        {
            var capitalizedFirstLetter = capitalizeMethod(input);
            Write(capitalizedFirstLetter);
        }
    }
}
