using APII.Model;
using APII.Repository.Interface;
using APII.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace APII.Repository
{
    public class EmployeesRepository : IEmployeeRepository
    {
        private readonly MyContext context;

        public EmployeesRepository(MyContext context)
        {
            this.context = context; //this.context sama seperti perintah _context
        }


        public IEnumerable<GetEmployeeAndDepartmentVm> Get()
        {
            return getEmployee().ToList();
        }

        public GetEmployeeAndDepartmentVm Get(string NIK)
        {
            var data = getEmployee().FirstOrDefault(e => e.NIK == NIK);
            return data;
        }

        public string generateNIK()
        {
            string date = DateTime.Now.ToString("ddMMyy"); //format tgl sekarang
            string uniqueNIK = "";

            var cekLastData = context.Employees.OrderBy(data => data.NIK).LastOrDefault(); //Cek data terakhir
            if (cekLastData == null) // data null adalah nilai pada suatu kolom yang berarti tidak mempunyai nilai.
            {
                uniqueNIK = date + "001";
            }
            else
            {
                var NIKLastData = cekLastData.NIK;            //dari cek data terakhir nik maka akan di
                string lastThree = NIKLastData.Substring(NIKLastData.Length - 3);       //substring menspesifikan karakter

                int kode = int.Parse(lastThree) + 1;        //increement
                uniqueNIK = date + kode.ToString("000"); // membuat kode unik nik dengan format tgl bulan tahun kemudian tambah dengan urutan
                                                         //  mengembalikan nilai string yang merupakan representasi obyek
            }
            return uniqueNIK;
        }

        public int Delete(string NIK)
        {
            var entity = context.Employees.Find(NIK);
            entity.IsActive = false;  // untuk menghapus nik yang non aktif sudah ditemukan
            var result = context.SaveChanges(); //kemudian perubahan tersebut akan disimpan
            return result;
        }

        public bool CheckEmailDuplicate(string email)
        {
            var duplicate = context.Employees.AsNoTracking().FirstOrDefault(e => e.Email == email); //mengambil data pertama  sesuai dengan data yang diberikan
            if (duplicate != null) //single memastikan data nya itu satu aja
            {
                return true;
            }
            return false;
        }

        public bool CheckPhoneDuplicate(string phone, string NIK)
        {
            var duplicate = context.Employees.AsNoTracking().FirstOrDefault(e => e.Phone == phone);
            if (duplicate == null || duplicate.NIK == NIK)
            {
                return false;
            } 
 
            return true;
        }

        public bool CheckNIKExist(string NIK)
        {
            var checkNIK = context.Employees.AsNoTracking().FirstOrDefault(e => e.NIK == NIK);
            if (checkNIK == null)
            {
                return false;
            }
            return true;
        }

        public bool CheckDepartmentExist(string id)
        {
            var data = context.Departments.Find(id);
            if (data == null)
            {
                return false;
            }
            return true;
        }

        public int Insert(EmployeeVm employee)
        {
            string date = DateTime.Now.ToString("ddMMyy");
            string newNIK = "";

            // cek data terakhir di database
            var lastData = context.Employees.OrderBy(data => data.NIK).LastOrDefault();
            if (lastData == null)
            {
                // kalau ternyata gak ada data di database, otomatis urutan 001
                newNIK = date + "001";
            }
            else
            {
                // ada data terakhir, ambil 3 karakter string dari NIK (nomor urut)
                var nikLastData = lastData.NIK;
                string lastThree = nikLastData.Substring(nikLastData.Length - 3);

                // convert jadi int terus tambah satu
                int nextSequence = int.Parse(lastThree) + 1;
                newNIK = date + nextSequence.ToString("000"); // convert jadi string
            }

            // generate email FirstNameLastName@berca.co.id (tambahin angka di belakangnya kalau ada nama yang sama)
            string domain = "@berca.co.id";
            string fullName = employee.FirstName + employee.LastName;
            string newEmail = "";

            // cek apakah email sudah digunakan
            var emailData = context.Employees.Where(e => e.Email.Contains(fullName)).OrderBy(data => data.NIK).LastOrDefault(); ;
            if (emailData == null)
            {
                newEmail = fullName + domain;
            }
            else
            {
                // pisahkan nama email dengan domainnya
                string[] emailSplit = Regex.Split(emailData.Email, "@");
                string emailName = (string)emailSplit[0];

                // ambil 3 karakter terakhir
                string lastThree = emailName.Substring(emailName.Length - 3);
                Console.WriteLine(lastThree);
                if (int.TryParse(lastThree, out int number))
                {
                    // jika 3 karakter terakhir adalah angka
                    int nextSequence = number + 1;
                    newEmail = fullName + nextSequence.ToString("000") + domain;
                }
                else
                {
                    newEmail = fullName + "001" + domain;
                }
            }

            Employee employeeData = new Employee
            {
                NIK = newNIK,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = newEmail.ToLower(), //email menggunakan huruf kecil
                Phone = employee.Phone,
                Address = employee.Address,
                IsActive = employee.IsActive,
                Department_id = employee.Department_id
            };
            context.Employees.Add(employeeData);
            var saveEmployee = context.SaveChanges();

            return saveEmployee;
        }

        public int Update(string NIK, EmployeeVm employee)
        {
            var data = context.Employees.Find(NIK);
            data.FirstName = employee.FirstName;
            data.LastName = employee.LastName;
            data.Phone = employee.Phone;
            data.Address = employee.Address;
            data.IsActive = employee.IsActive;
            data.Department_id = employee.Department_id;

            var result = context.SaveChanges();
            return result;
        }

        public IEnumerable<GetEmployeeAndDepartmentVm> getEmployee()
        {
            var data = context.Employees
                .Join(context.Departments,
                        ed => ed.Department_id,
                        dept => dept.DeptId,
                        (ed, dept) => new GetEmployeeAndDepartmentVm
                        {
                            NIK = ed.NIK,
                            FirstName = ed.FirstName,
                            LastName = ed.LastName,
                            Email = ed.Email,
                            Phone = ed.Phone,
                            Address = ed.Address,
                            IsActive = ed.IsActive,
                            Department = new DepartmentWithIdVm
                            {
                                DeptId = dept.DeptId,
                                Name = dept.Name
                            }
                        });
            return data;
        }

        public IEnumerable<GetEmployeeAndDepartmentVm> GetActiveEmployees()
        {
            var employees = getEmployee().Where(
                e => e.IsActive == true).ToList();  //untuk mengecek employee yang aktif pada tabel employee
            return employees; 
        }

        public IEnumerable<GetEmployeeAndDepartmentVm> GetInactiveEmployees()
        {
            var employees = getEmployee().Where(
               e => e.IsActive == false).ToList(); //untuk mengecek employee yang non-aktif/resign pada tabel employee
            return employees;
        }

        public IEnumerable<GetEmployeeAndDepartmentVm> GetActiveEmployeesByDepartment(string department_id)
        {
            //untuk mengecek employee yang masih aktif pada department
            var employees = getEmployee().Where(
               e => e.IsActive == true && e.Department.DeptId == department_id).ToList();
            return employees;
        }

        public IEnumerable<GetEmployeeAndDepartmentVm> GetInactiveEmployeesByDepartment(string department_id)
        {
            //untuk mengecek employee yang masih non-aktif/resign pada department
            var employees = getEmployee().Where(
               e => e.IsActive == false && e.Department.DeptId == department_id).ToList();
            return employees;
        }
    }
}
