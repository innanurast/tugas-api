using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APII.ViewModel
{
    public class DepartmentVm
    { 
        public string Name { get; set; }
    }
    public class DepartmentWithIdVm
    {
        public string DeptId { get; set; }
        public string Name { get; set; }
    }
    public class TotalEmployees
    {
        public string DepartmentName { get; set; }
        public int Total { get; set; }
    }

}
