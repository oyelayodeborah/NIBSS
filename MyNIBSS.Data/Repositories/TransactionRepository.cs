using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyNIBSS.Core.Models;

namespace MyNIBSS.Data.Repositories
{
    public class TransactionRepository
    {
        BaseRepository<Transaction> baseRepo = new BaseRepository<Transaction>(new ApplicationDbContext());

        public void Save(Transaction transaction)
        {
            baseRepo.Save(transaction);
        }
        public IEnumerable<Transaction> GetAll()
        {
            return baseRepo.GetAll();
        }

    }
}
