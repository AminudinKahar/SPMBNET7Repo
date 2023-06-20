using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace SPMBNET7.CoreBusiness.Models.Modules._02_Akaun
{
    public class AkPenyataPemungut1
    {
        public int Id { get; set; }
        public int Indek { get; set; }
        public int AkPenyataPemungutId { get; set; }
        public AkPenyataPemungut? AkPenyataPemungut { get; set; }
        public int JBahagianId { get; set; }
        public JBahagian? JBahagian { get; set; }
        public int AkCartaId { get; set; }
        public AkCarta? AkCarta { get; set; }
        [DisplayName("Amaun RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amaun { get; set; }
    }
}