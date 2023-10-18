using System.ComponentModel.DataAnnotations;

namespace APII.Model
{
    public class Department
    {
        [Key]
        public string DeptId { get; set; }
        public string Name { get; set; }

        public ICollection<Employee> employees { get; set; }
    }
}
