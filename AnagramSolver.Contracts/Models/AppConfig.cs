using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Contracts.Models
{
    public class AppConfig
    {
        public int MinWordLength { get; set; }
        public int MaxAnagrams { get; set; }
    }
}
