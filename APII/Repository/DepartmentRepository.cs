using APII.Model;
using APII.Repository.Interface;
using APII.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace APII.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly MyContext context;

        public DepartmentRepository(MyContext context)
        {
            this.context = context; //this.context sama seperti perintah _context
        }

        public int Delete(string DeptId)
        {
            var findData = context.Departments.Find(DeptId);
            if (findData != null)
            {
                context.Departments.Remove(findData);
            }
            var save = context.SaveChanges();
            return save;
        }

        public IEnumerable<Department> Get()
        {
            return context.Departments.ToList();
        }

        public Department Get(string DeptId)
        {
            var data = context.Departments.Find(DeptId);
            return data;
        }

        public bool CheckIDExist(string DeptId)
        {
            var cekID = context.Departments.AsNoTracking().FirstOrDefault(e => e.DeptId == DeptId);
            if (cekID == null)
            {
                return false;
            }
            return true;
        }

        public int Insert(DepartmentWithIdVm department)
        {
            string Id = "";
            var lastData = context.Departments.OrderBy(data => data.DeptId).LastOrDefault();
            if (lastData == null)
            {
                // kalau ternyata gak ada data di database, otomatis urutan 001
                Id = "D" + "001";
            }
            else
            {
                // ada data terakhir, ambil 3 karakter string dari NIK (nomor urut)
                var lastId = lastData.DeptId;
                string lastThree = lastId.Substring(lastId.Length - 3);

                // convert jadi int terus tambah satu
                int nextSequence = int.Parse(lastThree) + 1;
                Id = "D" + nextSequence.ToString("000"); // convert jadi string
            }

            var departmentData = new Department
            {
                DeptId = Id,
                Name = department.Name
            };

            context.Departments.Add(departmentData);
            var saveDepartment = context.SaveChanges();

            return saveDepartment;
        }

        public int Update(DepartmentWithIdVm department)
        {
            var data = context.Departments.Find(department.DeptId);
            data.Name = department.Name;
            var result = context.SaveChanges();
            return result;
        }
    }
}
        