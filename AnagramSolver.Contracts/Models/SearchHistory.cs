using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Contracts.Models
{
    public class SearchHistory
    {
        public string? IpAddress { get; set; }
        public int TimeSpent { get; set; }
        public string? SearchWord { get; set; }
        public string? Anagrams { get; set; }
    }
}
