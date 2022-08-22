using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Contracts.Models
{
    public class CachedWord
    {
        public int Id { get; set; }
        public string Word { get; set; } = null!;
        public virtual ICollection<Anagram> Anagrams { get; set; } = new HashSet<Anagram>();
    }
}
