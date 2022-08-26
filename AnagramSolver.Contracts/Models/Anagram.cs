using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Contracts.Models
{
    public class Anagram
    {
        public int Id { get; set; }
        public string Word { get; set; } = null!;
        public int CachedWordId { get; set; }
        public virtual CachedWord? CachedWord { get; set; }
    }
}
