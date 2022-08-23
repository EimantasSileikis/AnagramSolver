using AnagramSolver.Contracts.Interfaces.Repositories;
using AnagramSolver.Contracts.Models;
using AnagramSolver.EF.CodeFirst.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.BusinessLogic.Repositories
{

    public class CachedWordRepository : Repository<CachedWord>, ICachedWordRepository
    {
        public CodeFirstContext CodeFirstContext { get { return (CodeFirstContext)Context; } }
        public CachedWordRepository(CodeFirstContext context) : base(context)
        {
        }

        public IEnumerable<string> GetCachedWordWithAnagrams(string word)
        {
            return CodeFirstContext.CachedWords.Include(c => c.Anagrams)
                .Where(w => w.Word == word)
                .SelectMany(w => w.Anagrams)
                .Select(w => w.Word);
        }
    }
}
