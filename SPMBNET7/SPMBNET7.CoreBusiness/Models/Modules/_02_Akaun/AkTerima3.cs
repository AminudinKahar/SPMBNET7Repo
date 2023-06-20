using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace SPMBNET7.CoreBusiness.Models.Modules._02_Akaun
{
    public class AkTerima3
    {
        //field
        public int Id { get; set; }
        public int AkTerimaId { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amaun { get; set; }
        //field end

        //Relationship
        [DisplayName("No Inbois Dikeluarkan")]
        public int? AkInvoisId { get; set; }
        public AkInvois? AkInvois { get; set; }
        public AkTerima? AkTerima { get; set; }
        //relationship end
    }
}