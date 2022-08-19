using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.EF.CodeFirst.Models
{
    public class AnagramsEntity
    {
        public int Id { get; set; }
        public string Anagram { get; set; } = null!;
        public int WordId { get; set; }
        public CachedWordEntity? Word { get; set; }

    }
}
