using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMBNET7.CoreBusiness.Models.Modules._02_Akaun.ViewModels
{
    public class AkPOLarasViewModel : AkPOLaras
    {
        public string? NamaSykt { get; set; }
        public string? Alamat1 { get; set; }
        public decimal JumlahPerihal { get; set; }
    }
}
