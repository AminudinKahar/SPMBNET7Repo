using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMBNET7.CoreBusiness.Models.Modules._02_Akaun.ViewModels
{
    public class AbUntungRugiViewModel
    {
        public string Jenis { get; set; } = string.Empty;
        public string NoAkaun { get; set; } = string.Empty;
        public string NamaAkaun { get; set; } = string.Empty;
        public decimal Amaun { get; set; }
    }
}
