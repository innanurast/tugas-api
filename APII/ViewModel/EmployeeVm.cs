namespace APII.ViewModel
{
    public class EmployeeVm
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; } // sebagai tanda karyawan masih aktif atau tidak
        public string Department_id { get; set; } // sebagai foreign key untuk tabel departmen
    }
    public class GetEmployeeAndDepartmentVm
    {
        public string NIK { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
        public DepartmentWithIdVm Department { get; set; }
    }
}
