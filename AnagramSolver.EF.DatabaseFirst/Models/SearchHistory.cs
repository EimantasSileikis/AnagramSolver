using System;
using System.Collections.Generic;

namespace AnagramSolver.EF.DatabaseFirst.Models
{
    public partial class SearchHistory
    {
        public int Id { get; set; }
        public string IpAddress { get; set; } = null!;
        public int? TimeSpent { get; set; }
        public string SearchWord { get; set; } = null!;
        public string Anagrams { get; set; } = null!;
    }
}
