using System.ComponentModel.DataAnnotations;

namespace MyAPISolution.SampleAPI.Models
{
    public class Pegawai
    {
        [Key]
        public int IdPegawai { get; set; }
        public string Nama { get; set; }
        public string Alamat { get; set; }
        public string Email { get; set; }
        public string NoTelp { get; set; }
        public string Status { get; set; }
    }
}
