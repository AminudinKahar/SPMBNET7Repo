using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMBNET7.CoreBusiness.Models.Modules._02_Akaun.ViewModels
{
    public class AkInvoisViewModel : AkInvois
    {
        public string? NamaSykt { get; set; }
        public string? Alamat1 { get; set; }
        public decimal JumlahPerihal { get; set; }
        public decimal JumlahTerimaan { get; set; }
    }
}
