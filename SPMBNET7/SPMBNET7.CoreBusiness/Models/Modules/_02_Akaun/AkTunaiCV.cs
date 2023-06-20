using SPMBNET7.CoreBusiness.Models.Modules._03_Sumber;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using SPMBNET7.CoreBusiness.Models.Bases.BaseInterfaces;
using SPMBNET7.CoreBusiness.Models.Bases.Helper;

namespace SPMBNET7.CoreBusiness.Models.Modules._02_Akaun
{
    public class AkTunaiCV : AppLogHelper, ISoftDelete
    {
        public int Id { get; set; }
        [DisplayName("Kod Kaunter Panjar")]
        [Required(ErrorMessage = "Kod Kaunter Panjar diperlukan")]
        public int AkTunaiRuncitId { get; set; }
        public AkTunaiRuncit? AkTunaiRuncit { get; set; }
        public int KategoriPenerima { get; set; }
        [Required(ErrorMessage = "Tahun diperlukan")]
        [RegularExpression(@"^[\d+]*$", ErrorMessage = "Nombor sahaja dibenarkan")]
        [MaxLength(4)]
        public string Tahun { get; set; } = string.Empty;
        [DisplayName("No CV")]
        public string NoCV { get; set; } = string.Empty;
        [DisplayName("Tarikh")]
        [Required(ErrorMessage = "Tarikh diperlukan")]
        public DateTime Tarikh { get; set; }
        [DisplayName("Kod Anggota")]
        public int? SuPekerjaId { get; set; }
        public SuPekerja? SuPekerja { get; set; }
        [DisplayName("Kod Pembekal")]
        public int? AkPembekalId { get; set; }
        public AkPembekal? AkPembekal { get; set; }
        [DisplayName("No KP")]
        public string NoKP { get; set; } = string.Empty;
        [Required(ErrorMessage = "Penerima diperlukan")]
        public string Penerima { get; set; } = string.Empty;
        [DisplayName("Alamat")]
        public string Alamat1 { get; set; } = string.Empty;
        public string Alamat2 { get; set; } = string.Empty;
        public string Alamat3 { get; set; } = string.Empty;
        [Required(ErrorMessage = "Catatan diperlukan")]
        public string Catatan { get; set; } = string.Empty;
        [DisplayName("Jumlah RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Jumlah { get; set; }
        public ICollection<AkTunaiCV1>? AkTunaiCV1 { get; set; }
        public int FlPosting { get; set; }
        public DateTime? TarikhPosting { get; set; }
        public int FlCetak { get; set; }
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        public string? SebabHapus { get; set; }
    }
}