using SPMBNET7.CoreBusiness._Enums;
using SPMBNET7.CoreBusiness.Models.Bases.BaseInterfaces;
using SPMBNET7.CoreBusiness.Models.Bases.Helper;
using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMBNET7.CoreBusiness.Models.Modules._04_Aset
{
    public class AsAset : AppLogHelper, ISoftDelete
    {
        public int Id { get; set; }
        public int JKWId { get; set; }
        public JKW? JKW { get; set; }
        public int JBahagianId { get; set; }
        public JBahagian? JBahagian { get; set; }
        public JenisAset JenisAset { get; set; }
        public string NoAset { get; set; } = string.Empty;
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        public string? SebabHapus { get; set; }
        public int AkInvois2Id { get; set; }

        public AkInvois2? AkInvois2 { get; set;}
    }
}
