using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ASM.Models
{
    public class CourseEntity
    {
        public CourseEntity()
        {
            listTrainer = new List<UserInfor>();
        }
        [Display(Name = "Category")]
        public virtual int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual CourseCategoryEntity abc { get; set; }
        public List<UserInfor> listTrainer { get; set; }

        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Need to input name")]
        public string Name { get; set; }

        

        public string Description { get; set; }
    }
}