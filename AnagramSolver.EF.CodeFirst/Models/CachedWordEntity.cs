using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.EF.CodeFirst.Models
{
    public class CachedWordEntity
    {
        public int Id { get; set; }
        public string Word { get; set; } = null!;
        public ICollection<AnagramsEntity> Anagrams { get; set; }
    }
}
