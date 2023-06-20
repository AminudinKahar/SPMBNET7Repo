using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace SPMBNET7.CoreBusiness.Models.Modules._02_Akaun
{
    public class AkPV2
    {
        //field
        public int Id { get; set; }
        public int AkPVId { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amaun { get; set; }
        //field end

        //flag
        public bool HavePO { get; set; }
        //flage end

        //Relationship
        [DisplayName("No Inbois Pembekal")]
        public int? AkBelianId { get; set; }
        public AkBelian? AkBelian { get; set; }
        public AkPV? AkPV { get; set; }
        //relationship end
    }
}