using SPMBNET7.CoreBusiness.Models.Bases.BaseInterfaces;
using SPMBNET7.CoreBusiness.Models.Bases.Helper;
using SPMBNET7.CoreBusiness.Models.Modules._91_Permohonan;
using System.ComponentModel.DataAnnotations;

namespace SPMBNET7.CoreBusiness.Models.Modules._01_Jadual
{
    public class JTahapAktiviti : AppLogHelper, ISoftDelete
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Perihal diperlukan")]
        public string Perihal { get; set; } = string.Empty;

        //relationship
        public ICollection<SpPendahuluanPelbagai>? SpPermohonanAktiviti { get; set; }

        //soft delete
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        public string? SebabHapus { get; set; }
        //soft delete end
    }
}
