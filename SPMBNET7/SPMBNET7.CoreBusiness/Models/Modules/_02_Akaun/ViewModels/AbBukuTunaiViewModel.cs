using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMBNET7.CoreBusiness.Models.Modules._02_Akaun.ViewModels
{
    public class AbBukuTunaiViewModel
    {
        public DateTime? TarMasuk { get; set; }
        public string NamaAkaunMasuk { get; set; } = string.Empty;
        public string NoRujukanMasuk { get; set; } = string.Empty;
        public decimal AmaunMasuk { get; set; }
        public decimal JumlahMasuk { get; set; }
        public DateTime? TarKeluar { get; set; }
        public string NamaAkaunKeluar { get; set; } = string.Empty;
        public string NoRujukanKeluar { get; set; } = string.Empty;
        public decimal AmaunKeluar { get; set; }
        public decimal JumlahKeluar { get; set; }
        public int KeluarMasuk { get; set; }
    }
}
