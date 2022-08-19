using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.EF.CodeFirst.Models
{
    public class SearchHistoryEntity
    {
        public int Id { get; set; }
        public string IpAddress { get; set; } = null!;
        public int TimeSpent { get; set; }
        public string SearchWord { get; set; } = null!;
        public string? Anagrams { get; set; }
    }
}
