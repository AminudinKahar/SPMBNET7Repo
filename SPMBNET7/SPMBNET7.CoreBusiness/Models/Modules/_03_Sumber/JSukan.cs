using SPMBNET7.CoreBusiness.Models.Modules._91_Permohonan;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using SPMBNET7.CoreBusiness.Models.Bases.BaseInterfaces;
using SPMBNET7.CoreBusiness.Models.Bases.Helper;

namespace SPMBNET7.CoreBusiness.Models.Modules._03_Sumber
{
    public class JSukan : AppLogHelper, ISoftDelete
    {
        public int Id { get; set; }
        public string Kod { get; set; } = string.Empty;
        [Required(ErrorMessage = "Perihal diperlukan")]
        public string Perihal { get; set; } = string.Empty;
        [DisplayName("Elit")]
        public bool IsElit { get; set; }
        [DisplayName("Pembangunan")]
        public bool IsPembangunan { get; set; }

        //relationship
        public ICollection<SpPendahuluanPelbagai>? SpPermohonanAktiviti { get; set; }
        public ICollection<SuProfil1>? SuProfil1 { get; set; }
        public ICollection<SuJurulatih>? SuJurulatih { get; set; }
        public ICollection<SuAtlet>? SuAtlet { get; set; }

        //soft delete
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        public string? SebabHapus { get; set; }
        //soft delete end
    }
}