using DCode.Data.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCode.Data.ProficiencyRepository
{
    public interface IProficiencyRepository
    {
               string GetProficiencyfromId(int? proficiencyId);

               IEnumerable<proficiency> GetAllProficiencies();
    }
}
