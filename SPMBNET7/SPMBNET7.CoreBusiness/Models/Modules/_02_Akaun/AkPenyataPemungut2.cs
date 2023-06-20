using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace SPMBNET7.CoreBusiness.Models.Modules._02_Akaun
{
    public class AkPenyataPemungut2
    {
        public int Id { get; set; }
        public int Indek { get; set; }
        public int AkPenyataPemungutId { get; set; }
        public AkPenyataPemungut? AkPenyataPemungut { get; set; }
        public int AkTerima2Id { get; set; }
        public AkTerima2? AkTerima2 { get; set; }
        [DisplayName("Amaun RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amaun { get; set; }
    }
}