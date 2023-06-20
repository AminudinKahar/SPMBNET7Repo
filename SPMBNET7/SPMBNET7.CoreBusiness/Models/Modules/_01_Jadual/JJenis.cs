using SPMBNET7.CoreBusiness.Models.Bases.BaseInterfaces;
using SPMBNET7.CoreBusiness.Models.Bases.Helper;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;

namespace SPMBNET7.CoreBusiness.Models.Modules._01_Jadual
{
    public class JJenis : AppLogHelper, ISoftDelete
    {
        public int Id { get; set; }
        public string Kod { get; set; } = string.Empty;
        public string Nama { get; set; } = string.Empty;

        //Relationship
        public ICollection<AkCarta> AkCarta { get; set; } = new List<AkCarta>();

        //soft delete
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        public string? SebabHapus { get; set; }
        //soft delete end
    }
}
