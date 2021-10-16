namespace ASM.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPropety : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Role", c => c.String());
            AddColumn("dbo.AspNetUsers", "Age", c => c.Int(nullable: true));
            AddColumn("dbo.AspNetUsers", "Education", c => c.String());
            AddColumn("dbo.AspNetUsers", "Type", c => c.String());
            AddColumn("dbo.AspNetUsers", "WorkingPlace", c => c.String());
            AddColumn("dbo.AspNetUsers", "ProgrammingLanguage", c => c.String());
            AddColumn("dbo.AspNetUsers", "Experience", c => c.String());
            AlterColumn("dbo.AspNetUsers", "DoB", c => c.DateTime(nullable: true));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "DoB", c => c.String());
            DropColumn("dbo.AspNetUsers", "Experience");
            DropColumn("dbo.AspNetUsers", "ProgrammingLanguage");
            DropColumn("dbo.AspNetUsers", "WorkingPlace");
            DropColumn("dbo.AspNetUsers", "Type");
            DropColumn("dbo.AspNetUsers", "Education");
            DropColumn("dbo.AspNetUsers", "Age");
            DropColumn("dbo.AspNetUsers", "Role");
        }
    }
}
