namespace ASM.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPassConfirm : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "PassTempConfirm", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "PassTempConfirm");
        }
    }
}
