using APII.Model;
using APII.Repository;
using APII.Repository.Interface;
using APII.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net;

namespace APII.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly DepartmentRepository repository;
        public DepartmentController(DepartmentRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public ActionResult Get()
        {
            var data = repository.Get(); // mengambil fungsi dari repositori
            if (data.Count() != 0)
            {
                return Ok(new { status = HttpStatusCode.OK, message = "Data berhasil ditemukan.", data });
            }
            else
            {
                return NotFound(new { status = HttpStatusCode.NotFound, message = "Data tidak ditemukan." });
            }
        }

        [HttpGet("{id}")]
        public virtual ActionResult Get(string id)
        {
            var data = repository.Get(id);
            if (data != null)
            {
                return Ok(new { status = HttpStatusCode.OK, message = "Data berhasil ditemukan.", data });
            }
            else
            {
                return NotFound(new { status = HttpStatusCode.NotFound, message = "Data tidak ditemukan." });
            }
        }

        [HttpPost]
        public virtual ActionResult Insert(DepartmentWithIdVm department)
        {
            repository.Insert(department);
            return Ok(new { status = HttpStatusCode.OK, message = "Data berhasil ditambahkan." });
        }

        [HttpDelete("{id}")]
        public virtual ActionResult Delete(string id)
        {
            var delete = repository.Delete(id);
            if (delete >= 1)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data Berhasil Dihapus", Data = delete });
            }
            else if (delete == 0)
            {
                return StatusCode(404, new { status = HttpStatusCode.NotFound, message = "Data dengan Id " + id + "Tidak Ditemukan", Data = delete });
            }
            else
            {
                return StatusCode(500, new { status = HttpStatusCode.InternalServerError, message = "Terjadi Kesalahan", Data = delete });
            }
        }

        [HttpPut]
        public virtual ActionResult Update(DepartmentWithIdVm department)
        {
            repository.Update(department);
            return Ok(new { status = HttpStatusCode.OK, message = "Data berhasil diubah." });
        }

        [HttpGet("TestCORS")]
        public ActionResult TestCORS()
        {
            return Ok("Test CORS berhasil");
        }
    }
}
