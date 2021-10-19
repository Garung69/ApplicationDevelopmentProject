namespace ASM.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPassTemp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "PassTemp", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "PassTemp");
        }
    }
}
