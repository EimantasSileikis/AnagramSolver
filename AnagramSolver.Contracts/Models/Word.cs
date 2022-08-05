using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Contracts.Models
{
    public class Word
    {
        public string BaseWord { get; set; } = string.Empty;
        public string? PartOfSpeech { get; set; }
        public int Number { get; set; }
    }
}
