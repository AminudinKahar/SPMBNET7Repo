using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMBNET7.CoreBusiness.Models.Modules._02_Akaun.ViewModels
{
    public class AkPVViewModel : AkPV
    {
        public string? KodPenerima { get; set; }
        public string? Penerima { get; set; }
        public string? CaraBayar { get; set; }
        public string? BankPenerima { get; set; }
        public decimal JumlahInbois { get; set; }
        public decimal JumlahGanda { get; set; }
    }
}
