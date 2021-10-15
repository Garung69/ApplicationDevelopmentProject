using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASM.Models
{
    public class UserInfor: IdentityUser
    {
        public string Department { get; set; }
        public List<CourseEntity> listCourse { get; set; }
        public string Toeic { get; set; }
        public string Name { get; set; }
        public string DoB { get; set; }
        public string Location { get; set; }
        public string Programming_Language { get; set; }

        public UserInfor()
        {
            listCourse = new List<CourseEntity>();
        }
    }
}