namespace Cat.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TestEntities_added : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TestEntities",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TestEntities");
        }
    }
}
