namespace Cat.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TestEntity_removed : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.TestEntities");
        }
        
        public override void Down()
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
    }
}
