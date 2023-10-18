using APII.Model;
using APII.ViewModel;

namespace APII.Repository.Interface
{
    public interface IDepartmentRepository
    {
        IEnumerable<Department> Get();
        Department Get(string id);
        int Insert(DepartmentWithIdVm department);
        int Update(DepartmentWithIdVm department);
        int Delete(string id);
    }
}
