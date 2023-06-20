using SPMBNET7.CoreBusiness.Models.Modules._03_Sumber;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using SPMBNET7.CoreBusiness.Models.Bases.BaseInterfaces;
using SPMBNET7.CoreBusiness.Models.Bases.Helper;

namespace SPMBNET7.CoreBusiness.Models.Modules._02_Akaun
{
    public class AkCimbEFT : AppLogHelper, ISoftDelete
    {
        // note:
        // FlKategori = 0 ( Tanpa emel )
        // FlKategori = 1 ( dengan emel )
        // ..
        public int Id { get; set; }
        [DisplayName("No PBI")]
        public string NoPBI { get; set; } = string.Empty;
        [DisplayName("Tarikh Jana")]
        public DateTime TarJana { get; set; }
        [DisplayName("Tarikh Bayar")]
        public DateTime TarBayar { get; set; }
        [DisplayName("Jumlah RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Jumlah { get; set; }
        [DisplayName("Nama Fail")]
        public string NamaFail { get; set; } = string.Empty;
        [DisplayName("Bil Baucer")]
        public string BilPV { get; set; } = string.Empty;
        public string FlKategori { get; set; } = string.Empty;
        [DisplayName("Penjana")]
        public int? SuPekerjaId { get; set; }
        public SuPekerja? SuPekerja { get; set; }

        // note:
        // AkBank - Akaun Bank Pembayar
        [DisplayName("Akaun Bank Pembayar")]
        public int AkBankId { get; set; }
        public AkBank? AkBank { get; set; }

        // FlStatus = 0 -> Tolak / Gagal keseluruhan
        // FlStatus = 1 -> Berjaya keseluruhan
        // FlStatus = 2 -> Ada yang ditolak, ada yang berjaya
        [DisplayName("Status")]
        public int FlStatus { get; set; }
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        public string? SebabHapus { get; set; }
        public ICollection<AkCimbEFT1>? AkCimbEFT1 { get; set; }
    }
}