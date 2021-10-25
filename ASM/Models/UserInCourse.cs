using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASM.Models
{
    public class UserInCourse
    {
        public string UserId { get; set; }
        public string Username { get; set;}
        public string Email { get; set; }
        public string Role { get; set; }
        public List<CourseEntity> listCourseAssign { get; set; }
        public string CourseCategory { get; set; }
    }
}