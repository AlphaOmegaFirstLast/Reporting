using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bussiness.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        
        // Navigation property
        public ICollection<Employee> Employees { get; set; }
    }
}
