using System.ComponentModel.DataAnnotations;

namespace APII.Model
{
    public class Employee
    {
        [Key]
        public string NIK { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public string Address { get; set; }

        public bool IsActive { get; set; }
        public string Department_id { get; set; }  //enggak perlu pakai ini karena 

        public Department department { get; set; }
    }
}
