using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.EF.CodeFirst.Models
{
    public class WordEntity
    {
        public int Id { get; set; }
        public string Word { get; set; } = null!;
        public string PartOfSpeech { get; set; } = null!;
        public int Number { get; set; }
    }
}
