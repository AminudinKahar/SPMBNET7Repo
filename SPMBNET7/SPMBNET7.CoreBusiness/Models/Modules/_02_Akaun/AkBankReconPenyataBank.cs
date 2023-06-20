using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace SPMBNET7.CoreBusiness.Models.Modules._02_Akaun
{
    public class AkBankReconPenyataBank
    {
        public int Id { get; set; }
        public int Indek { get; set; }
        public int AkBankReconId { get; set; }
        public AkBankRecon? AkBankRecon { get; set; }
        public string NoAkaunBank { get; set; } = string.Empty;
        public DateTime Tarikh { get; set; }
        public string KodTransaksi { get; set; } = string.Empty;
        public string PerihalTransaksi { get; set; } = string.Empty;
        public string NoDokumen { get; set; } = string.Empty;
        [DisplayName("Debit RM")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Debit { get; set; }
        [DisplayName("Kredit RM")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Kredit { get; set; }
        [DisplayName("Baki RM")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Baki { get; set; }
        public bool IsPadan { get; set; }
        public ICollection<AkPadananPenyata>? AkPadananPenyata { get; set; }
    }
}