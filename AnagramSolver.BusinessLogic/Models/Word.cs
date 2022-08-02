using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.BusinessLogic.Models
{
    public class Word
    {
        public string BaseWord { get; set; }
        public string PartOfSpeech { get; set; }
        public string Form { get; set; }
        public int Number { get; set; }
    }
}
