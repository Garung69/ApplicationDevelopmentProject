using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASM.Models
{
    public class UserInfor: IdentityUser
    {
        public string Role { get; set; }
        public string Department { get; set; }
        public List<CourseEntity> listCourse { get; set; }
        public string Toeic { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Programming_Language { get; set; }
        public int Age { get; set; }
        public string Education { get; set; }
        public string Type { get; set; }
        public string WorkingPlace { get; set; }

        [DataType(DataType.Date)]
        public string DoB { get; set; }

        public string ProgrammingLanguage { get; set; }

        public string Experience { get; set; }

        public UserInfor()
        {
            listCourse = new List<CourseEntity>();
        }

        public string ToSeparatedString(string separator)
        {
            return $"{this.Id}{separator}" +
                $"{this.Name}{separator}"+
                $"{this.Education}{separator}";
        }

        

    }
}