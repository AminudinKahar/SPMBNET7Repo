using SPMBNET7.CoreBusiness.Models.Bases.BaseInterfaces;
using SPMBNET7.CoreBusiness.Models.Bases.Helper;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.CoreBusiness.Models.Modules._03_Sumber;
using SPMBNET7.CoreBusiness.Models.Modules._91_Permohonan;
using System.ComponentModel.DataAnnotations;

namespace SPMBNET7.CoreBusiness.Models.Modules._01_Jadual
{
    public class JNegeri : AppLogHelper, ISoftDelete
    {
        //field
        public int Id { get; set; }
        [Required(ErrorMessage = "Kod diperlukan")]
        [MaxLength(2, ErrorMessage = "Input tidak boleh melebihi 2 aksara")]
        public string Kod { get; set; } = string.Empty;
        [Required(ErrorMessage = "Perihal diperlukan")]
        [MaxLength(100, ErrorMessage = "Input tidak boleh melebihi 100 aksara")]
        public string Perihal { get; set; } = string.Empty;
        //field end

        //Relationship
        public ICollection<AkTerima>? AkTerima { get; set; }
        public ICollection<AkPembekal>? AkPembekal { get; set; }
        public ICollection<SuPekerja>? SuPekerja { get; set; }
        public ICollection<SpPendahuluanPelbagai>? SpPermohonanAktiviti { get; set; }
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
