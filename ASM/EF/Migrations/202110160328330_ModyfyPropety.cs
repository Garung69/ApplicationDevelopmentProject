namespace ASM.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModyfyPropety : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "DoB", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "DoB", c => c.DateTime(nullable: false));
        }
    }
}
