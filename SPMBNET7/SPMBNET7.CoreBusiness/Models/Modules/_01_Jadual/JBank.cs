using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using SPMBNET7.CoreBusiness.Models.Modules._03_Sumber;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.CoreBusiness.Models.Bases.BaseInterfaces;
using SPMBNET7.CoreBusiness.Models.Bases.Helper;

namespace SPMBNET7.CoreBusiness.Models.Modules._01_Jadual
{
    public class JBank : AppLogHelper, ISoftDelete
    {
        //field
        public int Id { get; set; }
        [Required(ErrorMessage = "Kod Diperlukan")]
        [MaxLength(12, ErrorMessage = "Input tidak boleh melebihi 12 aksara")]
        public string Kod { get; set; } = string.Empty;
        [Required(ErrorMessage = "Nama Bank Diperlukan")]
        [DisplayName("Nama Bank")]
        [MaxLength(100, ErrorMessage = "Input tidak boleh melebihi 100 aksara")]
        public string Nama { get; set; } = string.Empty;
        [DisplayName("Kod EFT")]
        [MaxLength(3, ErrorMessage = "Input tidak boleh melebihi 3 aksara")]
        [RegularExpression(@"^[\d+]*$", ErrorMessage = "Nombor sahaja dibenarkan")]
        //[Required(ErrorMessage = "Kod EFT Diperlukan")]
        public string KodEFT { get; set; } = string.Empty;
        public ICollection<SuPekerja>? SuPekerja { get; set; }
        public ICollection<AkBank>? AkBank { get; set; }
        public ICollection<AkPembekal>? AkPembekal { get; set; }
        public ICollection<AkCimbEFT1>? AkCimbEFT1 { get; set; }
        public ICollection<AkPV>? AkPV { get; set; }
        public ICollection<AkPVGanda>? AkPVGanda { get; set; }
        public ICollection<SuJurulatih>? SuJurulatih { get; set; }
        public ICollection<SuAtlet>? SuAtlet { get; set; }
        //field end

        //soft delete
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        public string? SebabHapus { get; set; }
        //soft delete end
    }
}
