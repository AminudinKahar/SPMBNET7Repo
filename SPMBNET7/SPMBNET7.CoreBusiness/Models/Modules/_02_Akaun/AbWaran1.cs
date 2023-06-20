using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SPMBNET7.CoreBusiness.Models.Modules._02_Akaun
{
    public class AbWaran1
    {
        //field
        public int Id { get; set; }
        public int AbWaranId { get; set; }
        [DisplayName("Amaun RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amaun { get; set; }
        [DisplayName("Tambah/Kurang")]
        [MaxLength(1)]
        public string TK { get; set; } = string.Empty;
        //field end

        //relationship
        [DisplayName("Kod Objek")]
        public int AkCartaId { get; set; }
        public AkCarta? AkCarta { get; set; }

        [DisplayName("Bahagian")]
        public int? JBahagianId { get; set; }
        public JBahagian? JBahagian { get; set; }

        //relationship end
    }
}