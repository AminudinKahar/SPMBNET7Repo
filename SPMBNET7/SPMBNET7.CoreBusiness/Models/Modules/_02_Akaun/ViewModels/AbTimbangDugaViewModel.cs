using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMBNET7.CoreBusiness.Models.Modules._02_Akaun.ViewModels
{
    public class AbTimbangDugaViewModel
    {
        public string NoAkaun { get; set; } = string.Empty;
        public string NamaAkaun { get; set; } = string.Empty;
        public string DebitKredit { get; set; } = string.Empty;
        public string Jenis { get; set; } = string.Empty;
        public decimal Debit { get; set; }
        public decimal Kredit { get; set; }
    }
}
