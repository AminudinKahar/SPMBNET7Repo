using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMBNET7.CoreBusiness.Models.Graphs
{
    public class KutipanBayaranMarkDetails
    {
        public string NumMonth { get; set; } = string.Empty;
        public string Month { get; set; } = string.Empty;
        public decimal Kutipan { get; set; }
        public decimal Bayaran { get; set; }
    }
}
