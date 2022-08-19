using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Contracts.Models
{
    public class SearchHistory
    {
        public int Id { get; set; }
        public string IpAddress { get; set; } = null!;
        public int? TimeSpent { get; set; }
        public string SearchWord { get; set; } = null!;
        public string Anagrams { get; set; } = null!;
    }
}
