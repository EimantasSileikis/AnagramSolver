using System;
using System.Collections.Generic;

namespace AnagramSolver.EF.DatabaseFirst.Models
{
    public partial class Anagram
    {
        public int Id { get; set; }
        public string Anagram1 { get; set; } = null!;
        public int? WordId { get; set; }

        public virtual CachedWord? Word { get; set; }
    }
}
