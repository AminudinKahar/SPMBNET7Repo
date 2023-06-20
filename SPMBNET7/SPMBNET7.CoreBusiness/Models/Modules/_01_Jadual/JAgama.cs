using SPMBNET7.CoreBusiness.Models.Bases.BaseInterfaces;
using SPMBNET7.CoreBusiness.Models.Bases.Helper;
using SPMBNET7.CoreBusiness.Models.Modules._03_Sumber;
using System.ComponentModel.DataAnnotations;

namespace SPMBNET7.CoreBusiness.Models.Modules._01_Jadual
{
    public class JAgama  : AppLogHelper, ISoftDelete
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Perihal diperlukan")]
        public string Perihal { get; set; } = string.Empty;

        //relationship
        public ICollection<SuPekerja>? SuPekerja { get; set; }
        //relationship end

        //soft delete
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        public string? SebabHapus  { get; set; }
        //soft delete end
    }
}
