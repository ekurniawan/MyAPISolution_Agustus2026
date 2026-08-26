using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyAPISolution.SampleAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PegawaiController : ControllerBase
    {
        List<string> listPegawai = new List<string>()
        {
            "Bambang", "Erick", "Siti", "Agus", "Rina"
        };


        // GET: api/<PegawaiController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return listPegawai;
        }

        // GET api/<PegawaiController>/5
        [HttpGet("{nama}")]
        public string Get(string nama)
        {
            string result = listPegawai.FirstOrDefault(x => x.ToLower() == nama.ToLower());
            return result;
        }

        // POST api/<PegawaiController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<PegawaiController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PegawaiController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
