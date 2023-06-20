using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMBNET7.CoreBusiness.Models.Modules._02_Akaun.ViewModels
{
    public class AkPenyataPemungut1ViewModel
    {
        public int Id { get; set; }
        public int AkTerimaId { get; set; }
        public int Indek { get; set; }
        public int JBahagianId { get; set; }
        public JBahagian? JBahagian { get; set; }
        public int AkCartaId { get; set; }
        public AkCarta? AkCarta { get; set; }
        public decimal Amaun { get; set; }
    }
}
