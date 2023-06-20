using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.CoreBusiness.Models.Modules._03_Sumber;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using SPMBNET7.CoreBusiness.Models.Bases.BaseInterfaces;
using SPMBNET7.CoreBusiness.Models.Bases.Helper;

namespace SPMBNET7.CoreBusiness.Models.Modules._02_Akaun
{
    public class AkPenyataPemungut : AppLogHelper, ISoftDelete
    {
        // note:
        // FlJenisCek = 0 || null ( Lain - lain )
        // FlJenisCek = 1 ( Cek Cawangan Ini )
        // FlJenisCek = 2 ( Cek Tempatan )
        // FlJenisCek = 3 ( Cek Luar )
        // FlJenisCek = 4 ( Cek Antarabangsa )
        // ..
        public int Id { get; set; }
        [DisplayName("No Dokumen")]
        public string NoDokumen { get; set; } = string.Empty;
        [DisplayName("No Slip")]
        public string NoSlip { get; set; } = string.Empty;
        [DisplayName("Tarikh Slip")]
        public DateTime? TarSlip { get; set; }
        public DateTime Tarikh { get; set; }
        [DisplayName("Cara Bayar")]
        public int JCaraBayarId { get; set; }
        public JCaraBayar? JCaraBayar { get; set; }
        [DisplayName("Akaun Bank Penerima")]
        public int AkBankId { get; set; }
        public AkBank? AkBank { get; set; }
        public string Tahun { get; set; } = string.Empty;
        [DisplayName("Jenis Cek")]
        public int FlJenisCek { get; set; }
        [DisplayName("Bil Resit")]
        public string BilTerima { get; set; } = string.Empty;
        [DisplayName("Jumlah RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Jumlah { get; set; }
        [DisplayName("Penjana")]
        public int? SuPekerjaId { get; set; }
        public SuPekerja? SuPekerja { get; set; }
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        public string? SebabHapus { get; set; }
        public ICollection<AkPenyataPemungut1>? AkPenyataPemungut1 { get; set; }
        public ICollection<AkPenyataPemungut2>? AkPenyataPemungut2 { get; set; }
    }
}