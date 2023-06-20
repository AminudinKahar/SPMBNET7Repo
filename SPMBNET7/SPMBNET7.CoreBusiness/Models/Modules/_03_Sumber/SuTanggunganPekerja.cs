using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMBNET7.CoreBusiness.Models.Modules._03_Sumber
{
    public class SuTanggunganPekerja
    {
        public int Id { get; set; }
        public string Nama { get; set; } = string.Empty;
        public string Hubungan { get; set; } = string.Empty;
        public string NoKP { get; set; } = string.Empty;
        public int SuPekerjaId { get; set; }

        //relationship
        public SuPekerja? SuPekerja { get; set; }
        //relationship end
    }
}
