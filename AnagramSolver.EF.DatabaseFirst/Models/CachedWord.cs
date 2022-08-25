using System;
using System.Collections.Generic;

namespace AnagramSolver.EF.DatabaseFirst.Models
{
    public partial class CachedWord
    {
        public CachedWord()
        {
            Anagrams = new HashSet<Anagram>();
        }

        public int Id { get; set; }
        public string Word { get; set; } = null!;

        public virtual ICollection<Anagram> Anagrams { get; set; }
    }
}
