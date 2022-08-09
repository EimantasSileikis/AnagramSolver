using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Contracts.Models
{
    public class Word : IEquatable<Word>
    {
        public string BaseWord { get; set; } = string.Empty;
        public string PartOfSpeech { get; set; } = String.Empty;
        public int Number { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as Word);
        }

        public bool Equals(Word other)
        {
            return other != null &&
                   BaseWord == other.BaseWord &&
                   PartOfSpeech == other.PartOfSpeech;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(BaseWord, PartOfSpeech);
        }

        public override string? ToString()
        {
            return $"{BaseWord}\t{PartOfSpeech}\t{BaseWord}\t{Number}";
        }
    }
}
