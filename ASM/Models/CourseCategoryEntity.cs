using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASM.Models
{
    public class CourseCategoryEntity
    {
        public CourseCategoryEntity()
        {
            list = new List<CourseEntity>();// Code first relationship One to Many 
        }
        
        public List<CourseEntity> list { get; set; }//One CourseCategoryEntitycan 
                                                    //have many Course
        public int Id { get; set; }
        [Required(ErrorMessage = "Need to input name")]
        public string Name { get; set; }

        public string Description { get; set; }

        public string ToSeparatedString(string separator)
        {
            return $"{this.Id}{separator}" +
                $"{this.Name}{separator}" +
                $"{this.Description}{separator}";

        }
    }
}