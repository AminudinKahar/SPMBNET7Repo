using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using SPMBNET7.CoreBusiness.Models.Modules._04_Aset;

namespace SPMBNET7.CoreBusiness.Models.Modules._02_Akaun
{
    public class AkNotaDebitKreditBelian2
    {
        //field
        public int Id { get; set; }
        public int AkNotaDebitKreditBelianId { get; set; }
        public int Indek { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Bil { get; set; }
        [MaxLength(20)]
        public string? NoStok { get; set; }
        public ICollection<AsAset>? AsAset { get; set; }
        [MaxLength(100)]
        public string Perihal { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Kuantiti { get; set; }
        [MaxLength(100)]
        public string? Unit { get; set; }
        [DisplayName("Harga RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Harga { get; set; }
        [DisplayName("Amaun RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amaun { get; set; }
        //field end

        //Relationship
        //Relationship end
    }
}