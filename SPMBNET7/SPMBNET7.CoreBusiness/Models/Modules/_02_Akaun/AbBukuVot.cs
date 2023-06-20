using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace SPMBNET7.CoreBusiness.Models.Modules._02_Akaun
{
    public class AbBukuVot
    {
        public int Id { get; set; }
        public string Tahun { get; set; } = string.Empty;
        [DisplayName("KW")]
        public int JKWId { get; set; }
        [DisplayName("Bahagian")]
        public int? JBahagianId { get; set; }
        public JBahagian? JBahagian { get; set; }
        public DateTime Tarikh { get; set; }
        public string Kod { get; set; } = string.Empty;
        public string Penerima { get; set; } = string.Empty;
        [DisplayName("Vot")]
        public int VotId { get; set; }
        public string Rujukan { get; set; } = string.Empty;
        [DisplayName("Debit RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Debit { get; set; }
        [DisplayName("Kredit RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Kredit { get; set; }
        [DisplayName("Tanggungan RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Tanggungan { get; set; }
        [DisplayName("TBS RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Tbs { get; set; }
        [DisplayName("Baki RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Baki { get; set; }
        [DisplayName("Rizab RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Rizab { get; set; }
        [DisplayName("Liabiliti RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Liabiliti { get; set; }
        [DisplayName("Belanja RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Belanja { get; set; }

        //relationship
        public JKW? JKW { get; set; }
        public AkCarta? Vot { get; set; }
    }
}