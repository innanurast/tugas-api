using APII.Model;
using APII.ViewModel;

namespace APII.Repository.Interface
{
    public interface IEmployeeRepository
    {
        IEnumerable<GetEmployeeAndDepartmentVm> Get(); // read semua data (select * from)       cocok powerfull untuk get, better daripada IList)

        GetEmployeeAndDepartmentVm Get(string NIK);
        int Insert(EmployeeVm employee);
        int Update(string NIK, EmployeeVm employee);
        int Delete(string NIK);
        IEnumerable<GetEmployeeAndDepartmentVm> GetActiveEmployees();
        IEnumerable<GetEmployeeAndDepartmentVm> GetInactiveEmployees();
        IEnumerable<GetEmployeeAndDepartmentVm> GetActiveEmployeesByDepartment(string department_id);
        IEnumerable<GetEmployeeAndDepartmentVm> GetInactiveEmployeesByDepartment(string department_id);

    }
}
