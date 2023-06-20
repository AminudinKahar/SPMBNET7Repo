using SPMBNET7.CoreBusiness.Models.Bases.BaseInterfaces;
using SPMBNET7.CoreBusiness.Models.Bases.Helper;
using SPMBNET7.CoreBusiness.Models.Modules._91_Permohonan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMBNET7.CoreBusiness.Models.Modules._01_Jadual
{
    public class JJantina : AppLogHelper, ISoftDelete
    {
        public int Id { get; set; }
        public string Perihal { get; set; } = string.Empty;
        public string Perihal2 { get; set; } = string.Empty;

        //soft delete
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        public string? SebabHapus { get; set; }

        public ICollection<SpPendahuluanPelbagai1> SpPendahuluanPelbagai1 { get; set; } = new List<SpPendahuluanPelbagai1>();
        //soft delete end
    }
}
