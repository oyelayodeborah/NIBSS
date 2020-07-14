using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyNIBSS.Core.Models;

namespace MyNIBSS.Data.Repositories
{
    public class FinancialInstitutionRepository
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        public BaseRepository<FinancialInstitution> Repo = new BaseRepository<FinancialInstitution>(new ApplicationDbContext());

        public FinancialInstitution GetByInstitutionCode(string institutionCode)
        {
            var transtype = _context.FinancialInstitutions.Where(c => c.InstitutionCode == institutionCode).FirstOrDefault();

            return transtype;
        }

        public bool isUniqueCode(string institutionCode)
        {
            bool flag = true;
            if (Repo.GetAll().Any(n => n.InstitutionCode.ToLower().Equals(institutionCode.ToLower())))
            {
                flag = false;
            }
            return flag;
        }
        public bool isUniqueCode(string oldCode, string newCode)
        {
            bool flag = true;
            if (!newCode.ToLower().Equals(newCode.ToLower()))
            {
                if (Repo.GetAll().Any(n => n.Name.ToLower().Equals(newCode.ToLower())))
                {
                    flag = false;
                }
            }
            return flag;
        }
        public bool isUniqueName(string name)
        {
            bool flag = true;
            if (Repo.GetAll().Any(n => n.Name.ToLower().Equals(name.ToLower())))
            {
                flag = false;
            }
            return flag;
        }
        public bool isUniqueName(string oldName, string newName)
        {
            bool flag = true;
            if (!oldName.ToLower().Equals(newName.ToLower()))
            {
                if (Repo.GetAll().Any(n => n.Name.ToLower().Equals(newName.ToLower())))
                {
                    flag = false;
                }
            }
            return flag;
        }
        public FinancialInstitution Get(int? id)
        {
            return Repo.Get(id);
        }
        public IEnumerable<FinancialInstitution> GetByName(string Name)
        {
            return _context.FinancialInstitutions.Where(c => c.Name == Name);
        }
        public IEnumerable<FinancialInstitution> GetAll()
        {
            return _context.FinancialInstitutions.ToList();
        }
        public void Update(FinancialInstitution financialinstitution)
        {
            Repo.Update(financialinstitution);
        }

        public void Save(FinancialInstitution financialinstitution)
        {
            Repo.Save(financialinstitution);
        }
    }
}