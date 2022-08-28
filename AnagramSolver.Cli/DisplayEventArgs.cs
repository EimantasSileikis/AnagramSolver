using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Cli
{
    public class DisplayEventArgs : EventArgs
    {
        public string Message { get; set; }
    }
}
