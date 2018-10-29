using DCode.Data.DbContexts;
using DCode.Data.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using DCode.Models.Enums;
using System;
using static DCode.Models.Enums.Enums;
using System.Threading.Tasks;

namespace DCode.Data.ProficiencyRepository
{
    public class ProficiencyRepository : Repository<Task>, IProficiencyRepository
    {
        private readonly TaskDbContext _context;
        public ProficiencyRepository(TaskDbContext context)
            : base(context)
        {
            _context = context;
        }
        public string GetProficiencyfromId(int? proficiencyid)
        {
            var proficiencyList = Context.Set<proficiency>().ToList();
           return proficiencyList?.Where(x => x.ID == proficiencyid)?.FirstOrDefault()?.Proficiency;
        }

        public IEnumerable<proficiency> GetAllProficiencies()
        {
            var query = Context.Set<proficiency>();

            return query?.ToList();
        }

    }
}
