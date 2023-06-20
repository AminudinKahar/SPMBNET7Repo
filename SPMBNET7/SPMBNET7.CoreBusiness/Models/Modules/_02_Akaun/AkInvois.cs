using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using SPMBNET7.CoreBusiness.Models.Bases.BaseInterfaces;
using SPMBNET7.CoreBusiness.Models.Bases.Helper;
using Microsoft.AspNetCore.Mvc;

namespace SPMBNET7.CoreBusiness.Models.Modules._02_Akaun
{
    public class AkInvois : AppLogHelper, ISoftDelete
    {
        //field
        public int Id { get; set; }
        [Required(ErrorMessage = "Tahun diperlukan")]
        [RegularExpression(@"^[\d+]*$", ErrorMessage = "Nombor sahaja dibenarkan")]
        [MaxLength(4)]
        public string Tahun { get; set; } = string.Empty;
        [Required(ErrorMessage = "Tarikh diperlukan")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Tarikh { get; set; }
        [DisplayName("No Rujukan")]
        [Required(ErrorMessage = "No Rujukan diperlukan")]
        public string NoInbois { get; set; } = string.Empty;
        [BindProperty]
        [DisplayName("Jumlah RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Jumlah { get; set; }
        //field end

        //flag
        //note :
        // FlJenis = 0 -> Pesanan Tempatan
        // FlJenis = 1 -> Inden Kerja
        [DisplayName("Posting")]
        [DefaultValue("0")]
        public int FlPosting { get; set; }
        public DateTime? TarikhPosting { get; set; }
        [DisplayName("Cetak")]
        [DefaultValue("0")]
        public int FlCetak { get; set; }
        //flag end

        //untuk kelulusan
        [DisplayName("Penyemak")]
        public int? JPenyemakId { get; set; }
        public JPenyemak? JPenyemak { get; set; }
        [DisplayName("Status Semak")]
        public int FlStatusSemak { get; set; }
        public DateTime? TarSemak { get; set; }
        [DisplayName("Pelulus")]
        public int? JPelulusId { get; set; }
        public JPelulus? JPelulus { get; set; }
        [DisplayName("Status Lulus")]
        public int FlStatusLulus { get; set; }
        public DateTime? TarLulus { get; set; }
        //untuk kelulusan end 

        //Relationship
        [Required(ErrorMessage = "Kump. Wang diperlukan")]
        [DisplayName("Kumpulan Wang")]
        public int JKWId { get; set; }
        [DisplayName("Bahagian")]
        public int? JBahagianId { get; set; }
        public JBahagian? JBahagian { get; set; }
        [DisplayName("No Pesanan Tempatan")]
        public int? AkPOId { get; set; }
        [Required(ErrorMessage = "Kod Akaun Penghutang diperlukan")]
        [DisplayName("Kod Akaun Penghutang")]
        public int KodObjekAPId { get; set; }
        [Required(ErrorMessage = "Kod Penghutang diperlukan")]
        //[RegularExpression("[^0]+", ErrorMessage = "Sila pilih Kod Penghutang")]
        [DisplayName("Kod Penghutang")]
        public int AkPenghutangId { get; set; }
        public JKW? JKW { get; set; }
        public AkPO? AkPO { get; set; }
        public AkCarta? KodObjekAP { get; set; }
        public AkPenghutang? AkPenghutang { get; set; }
        public ICollection<AkInvois1>? AkInvois1 { get; set; }
        public ICollection<AkInvois2>? AkInvois2 { get; set; }
        public ICollection<AkTerima3>? AkTerima3 { get; set; }

        //relationship end

        [DisplayName("Hapus")]
        [DefaultValue("0")]
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        public string? SebabHapus { get; set; }
    }
}