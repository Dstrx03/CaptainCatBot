namespace Cat.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SystemLogEntry_added : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SystemLogEntries",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        EntryDescriptor = c.String(),
                        EntryDate = c.DateTime(nullable: false),
                        Entry = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.SystemValues", "DataDescriptor", c => c.String());
            AddColumn("dbo.SystemValues", "DataType", c => c.Int(nullable: false));
            DropColumn("dbo.SystemValues", "Descriminator");
            DropColumn("dbo.SystemValues", "Type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SystemValues", "Type", c => c.Int(nullable: false));
            AddColumn("dbo.SystemValues", "Descriminator", c => c.String());
            DropColumn("dbo.SystemValues", "DataType");
            DropColumn("dbo.SystemValues", "DataDescriptor");
            DropTable("dbo.SystemLogEntries");
        }
    }
}
