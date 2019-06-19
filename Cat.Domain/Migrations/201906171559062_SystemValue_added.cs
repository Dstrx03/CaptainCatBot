namespace Cat.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SystemValue_added : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SystemValues",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Data = c.String(),
                        Descriminator = c.String(),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SystemValues");
        }
    }
}
