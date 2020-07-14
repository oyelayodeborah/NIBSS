using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyNIBSS.Core.Models
{
    public class ApplicationDbContext :  DbContext
    {
        public ApplicationDbContext()
            : base("NIBSS")
        {
        }
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<SavingsAcctMgt>().Property(e => e.LoanAmountRemaining).HasPrecision(20, 10);
        //}
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties<decimal>().Configure(config => config.HasPrecision(20, 10));
            

        }



        public DbSet<Role> Roles { get; set; }

        public DbSet<User> Users { get; set; }

        
        public DbSet<CustomerAccount> CustomerAccounts { get; set; }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Node> Nodes { get; set; }

        public DbSet<FinancialInstitution> FinancialInstitutions { get; set; }

        /*
         * protected override void Seed(MyNIBSS.Core.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            var getBranch = context.Branches.ToList().Count();
            var getRole = context.Roles.ToList().Count();
            if (getBranch == 0)
            {
                context.Branches.AddOrUpdate(x => x.id, new Branch() { name = "Yaba" });

            }
            if (getRole == 0)
            {
                context.Roles.AddOrUpdate(x => x.id, new Role() { name = "Admin" });
            }
        }
         */
    }
}
