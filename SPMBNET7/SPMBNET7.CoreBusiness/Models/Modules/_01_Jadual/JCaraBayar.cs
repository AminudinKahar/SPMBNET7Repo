using SPMBNET7.CoreBusiness.Models.Bases.BaseInterfaces;
using SPMBNET7.CoreBusiness.Models.Bases.Helper;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.CoreBusiness.Models.Modules._03_Sumber;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMBNET7.CoreBusiness.Models.Modules._01_Jadual
{
    public class JCaraBayar : AppLogHelper, ISoftDelete
    {
        //field
        public int Id { get; set; }
        [Required(ErrorMessage = "Kod Diperlukan")]
        [MaxLength(2, ErrorMessage = "Input tidak boleh melebihi 2 aksara")]
        public string Kod { get; set; } = string.Empty;
        [Required(ErrorMessage = "Perihal Diperlukan")]
        [MaxLength(100, ErrorMessage = "Input tidak boleh melebihi 100 aksara")]
        public string Perihal { get; set; } = string.Empty;
        //field end

        //relationship
        public ICollection<AkTerima2>? AkTerima2 { get; set; }
        public ICollection<AkPV>? AkPV { get; set; }
        public ICollection<SuPekerja>? SuPekerja { get; set; }
        public ICollection<SuProfil1>? SuProfil1 { get; set; }
        public ICollection<AkPenyataPemungut>? AkPenyataPemungut { get; set; }
        public ICollection<AkPVGanda>? AkPVGanda { get; set; }

        public ICollection<SuJurulatih>? SuJurulatih { get; set; }
        public ICollection<SuAtlet>? SuAtlet { get; set; }
        //relationship end

        //soft delete
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        public string? SebabHapus { get; set; }
        //soft delete end
    }
}
