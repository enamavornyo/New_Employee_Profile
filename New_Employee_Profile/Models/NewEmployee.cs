using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using New_Employee_Profile.Infrastructure.Validation;

namespace New_Employee_Profile.Models
{
    public class NewEmployee
    {
        public long Id { get; set; }

        [Display(Name = "First Name")]
        public string FName { get; set; } = string.Empty;


        [Display(Name = "Last Name")]
        public string LName { get; set; } = string.Empty;


        [Display(Name = "Other Name")]
        public string? OName { get; set; } = string.Empty;

        //slug comes from name  (check admin/items/controller)
        public string Slug { get; set; } = string.Empty;


        [Display(Name = "Department")]
        [Required, Range(1, int.MaxValue, ErrorMessage = "You must choose a Department")]
        public int DepartmentId { get; set; }

        public Department? Department { get; set; }

        [Display(Name = "Section")]
        [Required, Range(1, int.MaxValue, ErrorMessage = "You must choose a Section")]
        public int SectionId { get; set; }

        public Section? Section { get; set; }

        [Display(Name = "Role")]
        [Required, Range(1, int.MaxValue, ErrorMessage = "You must choose a Role")]
        public int RoleId { get; set; }

        public Role? Role { get; set; }


        [Display(Name = "Employment Type")]
        [Required, Range(1, int.MaxValue, ErrorMessage = "You must choose an Employment Type")]
        public int EmpTypeId { get; set; }

        public EmpType? EmpType { get; set; }


        [Display(Name = "Employment Date")]
        [Required(ErrorMessage = "Request Date is required")]
        public DateTime EmploymentDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime EmploymentEndDate { get; set; }

        public string ImageName { get; set; } = string.Empty;

        public string? ImageURL { get; set; } = string.Empty;

        [NotMapped]
        [FileExtension]
        [Required(ErrorMessage = "You must choose an Image")]
        public IFormFile? ImageUpload { get; set; }


    }
}

