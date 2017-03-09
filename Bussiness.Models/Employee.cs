using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bussiness.Models
{
    public class Employee
    {

        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }   // Auth. save it hashed

        [DataType(DataType.PostalCode)]
        [StringLength(maximumLength: 10, ErrorMessage = "maximum length is 10 char ya gameel")]
        public string Address { get; set; }    //verify with google maps

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }      // welcome to database email

        [DataType(DataType.PhoneNumber)]       // click here to call   
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered phone format is not valid.")]
        public string Phone { get; set; }

        [DataType(DataType.Url)]               // read html
        public string WebSite { get; set; }

        [Range(500, 4000, ErrorMessage = "Please enter a value between 500 and 4000")]
        public int Salary { get; set; } = 700;

        [Display(Name = "Level Of Expertise")]
        [Range(1, 5)]
        public int Level { get; set; } = 2;

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime GraduationDate { get; set; }

        [DataType(DataType.MultilineText)]
        public string Experience { get; set; }

        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        //Navigation property
        public Department Department { get; set; }
    }


}
