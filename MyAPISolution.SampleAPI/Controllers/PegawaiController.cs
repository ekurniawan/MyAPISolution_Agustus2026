using Microsoft.AspNetCore.Mvc;
using MyAPISolution.SampleAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyAPISolution.SampleAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PegawaiController : ControllerBase
    {
        List<Pegawai> listPegawai = new List<Pegawai>()
        {
            new Pegawai { IdPegawai = 1, Nama = "Bambang", Alamat = "Alamat 1", Email = "bambang@banksulselbar.com" },
            new Pegawai { IdPegawai = 2, Nama = "Erick", Alamat = "Alamat 2", Email = "erick@banksulselbar.com" },
            new Pegawai { IdPegawai = 3, Nama = "Siti", Alamat = "Alamat 3", Email = "siti@banksulselbar.com" },
            new Pegawai { IdPegawai = 4, Nama = "Agus", Alamat = "Alamat 4", Email = "agus@banksulselbar.com" },
            new Pegawai { IdPegawai = 5, Nama = "Rina", Alamat = "Alamat 5", Email = "rina@banksulselbar.com" }
        };


        // GET: api/<PegawaiController>
        [HttpGet]
        public IEnumerable<Pegawai> Get()
        {
            return listPegawai;
        }

        // GET api/<PegawaiController>/5
        [HttpGet("{Id}")]
        public IActionResult Get(int Id)
        {
            Pegawai result = listPegawai.FirstOrDefault(x => x.IdPegawai == Id);

            if (result == null)
                return NotFound(new CustomError { ResponseCode = "404", Message = "Data tidak ditemukan" });

            return Ok(result);
        }

        // POST api/<PegawaiController>
        [HttpPost]
        public IActionResult Post([FromBody] Pegawai pegawai)
        {
            listPegawai.Add(pegawai);
            return CreatedAtAction(nameof(Get), new { Id = pegawai.IdPegawai }, pegawai);
        }

        // PUT api/<PegawaiController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Pegawai pegawai)
        {
            Pegawai existingPegawai = listPegawai.FirstOrDefault(x => x.IdPegawai == id);
            if (existingPegawai == null)
                return NotFound(new CustomError { ResponseCode = "404", Message = "Data tidak ditemukan" });

            existingPegawai.Nama = pegawai.Nama;
            existingPegawai.Alamat = pegawai.Alamat;
            existingPegawai.Email = pegawai.Email;

            return NoContent();
        }

        // DELETE api/<PegawaiController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Pegawai existingPegawai = listPegawai.FirstOrDefault(x => x.IdPegawai == id);
            if (existingPegawai == null)
                return NotFound(new CustomError { ResponseCode = "404", Message = "Data tidak ditemukan" });
            listPegawai.Remove(existingPegawai);
            return NoContent();
        }
    }
}
