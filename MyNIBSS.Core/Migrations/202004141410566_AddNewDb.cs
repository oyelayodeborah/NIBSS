namespace MyNIBSS.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerAccounts",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        acctName = c.String(nullable: false, maxLength: 60),
                        acctNumber = c.Long(nullable: false),
                        accType = c.String(nullable: false),
                        status = c.String(),
                        InstitutionCode = c.String(),
                        acctbalance = c.Decimal(nullable: false, precision: 20, scale: 10),
                        createdAt = c.DateTime(nullable: false),
                        isLinked = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.FinancialInstitutions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IPAddress = c.String(),
                        Port = c.Int(nullable: false),
                        InstitutionCode = c.String(),
                        Status = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Nodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        HostName = c.String(nullable: false),
                        IPAddress = c.String(nullable: false),
                        Port = c.String(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Amount = c.Decimal(nullable: false, precision: 20, scale: 10),
                        Date = c.DateTime(nullable: false),
                        FromAccountNumber = c.String(),
                        ToAccountNumber = c.String(),
                        CardPAN = c.String(),
                        STAN = c.String(),
                        ChannelCode = c.String(),
                        InstitutionCode = c.String(),
                        TransactionTypeCode = c.String(),
                        ResponseCode = c.String(),
                        ResponseDescription = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        fullName = c.String(nullable: false, maxLength: 225),
                        email = c.String(nullable: false, maxLength: 225),
                        username = c.String(nullable: false, maxLength: 25),
                        passwordHash = c.String(nullable: false, maxLength: 225),
                        phoneNumber = c.String(nullable: false, maxLength: 11),
                        roleId = c.Int(nullable: false),
                        LoggedIn = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Roles", t => t.roleId, cascadeDelete: true)
                .Index(t => t.roleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "roleId", "dbo.Roles");
            DropIndex("dbo.Users", new[] { "roleId" });
            DropTable("dbo.Users");
            DropTable("dbo.Transactions");
            DropTable("dbo.Roles");
            DropTable("dbo.Nodes");
            DropTable("dbo.FinancialInstitutions");
            DropTable("dbo.CustomerAccounts");
        }
    }
}
