using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Contracts.Interfaces.Core
{
    //public delegate string CapitalLetterHandler(string input);

    public interface IDisplay
    {
        public void Write(string message);
        void FormattedPrint(Func<string, string> capitalizeMethod, string input);
    }
}
