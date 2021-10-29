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
            listTrainer = new List<UserInfor>();//Code first Many to Many relationship
                                                //between class UserInfor and CourseEntity
        }
        [Display(Name = "Category")]
        public virtual int CategoryId { get; set; }// Set the foreign key acrroding
                                                   //to CategoryId
        [ForeignKey("CategoryId")]
        public virtual CourseCategoryEntity abc { get; set; }//Connect to CourseCategoryEntity 
                                                             // by relationship Many To Ome
        public List<UserInfor> listTrainer { get; set; }

        [Key]
        public int Id { get; set; } //set the primary key for this class
        [Required(ErrorMessage = "Need to input name")]
        public string Name { get; set; }//property

        public string Description { get; set; }//property
    }
}