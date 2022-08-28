using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Contracts.Models
{
    public class SearchLimit
    {
        [Key]
        [Required]
        public string Ip { get; set; } = null!;
        [Required]
        public uint Limit { get; set; }
    }
}
