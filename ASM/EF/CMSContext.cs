using ASM.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ASM.EF
{
    public class CMSContext: IdentityDbContext<UserInfor>
    {
        public CMSContext() : base("OOO")
        {

        }
        public DbSet<CourseCategoryEntity> courseCategoryEntities { get; set; }
        public DbSet<CourseEntity> Courses { get; set; }
    }
}