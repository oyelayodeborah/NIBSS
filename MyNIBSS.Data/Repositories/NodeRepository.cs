using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyNIBSS.Core.Models;

namespace MyNIBSS.Data.Repositories
{
    public class NodeRepository
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        public BaseRepository<Node> Repo = new BaseRepository<Node>(new ApplicationDbContext());

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
        public Node Get(int? id)
        {
            return Repo.Get(id);
        }
        public IEnumerable<Node> GetByName(string Name)
        {
            return _context.Nodes.Where(c => c.Name == Name);
        }
        public IEnumerable<Node> GetByStatus(Status Status)
        {
            return _context.Nodes.Where(c => c.Status == Status);
        }
        public IEnumerable<Node> GetAll()
        {
            return _context.Nodes.ToList();
        }
        public void Update(Node node)
        {
            Repo.Update(node);
        }

        public void Save(Node node)
        {
            Repo.Save(node);
        }
    }
}


