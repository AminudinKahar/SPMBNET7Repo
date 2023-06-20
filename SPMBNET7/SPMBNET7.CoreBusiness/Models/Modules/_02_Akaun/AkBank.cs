using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Xml.Linq;
using SPMBNET7.CoreBusiness.Models.Bases.Helper;
using SPMBNET7.CoreBusiness.Models.Bases.BaseInterfaces;

namespace SPMBNET7.CoreBusiness.Models.Modules._02_Akaun
{
    public class AkBank : AppLogHelper, ISoftDelete
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Kod Diperlukan")]
        [MaxLength(6)]
        public string Kod { get; set; } = string.Empty;
        [DisplayName("No Akaun")]
        [Required(ErrorMessage = "No Akaun Diperlukan")]
        [MaxLength(20)]
        public string NoAkaun { get; set; } = string.Empty;

        //Relationship
        [Required(ErrorMessage = "Kumpulan Wang Diperlukan")]
        //[RegularExpression("[^0]+", ErrorMessage = "Sila pilih Kumpulan Wang")]
        [DisplayName("Kumpulan Wang")]
        public int JKWId { get; set; }
        public JKW? JKW { get; set; }

        [DisplayName("Bahagian")]
        public int? JBahagianId { get; set; }
        public JBahagian? JBahagian { get; set; }

        [Required(ErrorMessage = "Bank Diperlukan")]
        [RegularExpression("[^0]+", ErrorMessage = "Sila pilih Bank")]
        [DisplayName("Nama Bank")]
        public int JBankId { get; set; }
        public JBank? JBank { get; set; }

        [Required(ErrorMessage = "Kod Akaun Diperlukan")]
        //[RegularExpression("[^0]+", ErrorMessage = "Sila pilih Kod Akaun")]
        [DisplayName("Kod Akaun")]
        public int AkCartaId { get; set; }
        public AkCarta? AkCarta { get; set; }
        // cek if kod bank using bajet or not
        [Display(Name = "Dikira Dalam Peruntukan")]
        public bool IsBajet { get; set; } = true;
        public ICollection<AkTerima>? AkTerima { get; set; }
        public ICollection<AkPV>? AkPV { get; set; }
        public ICollection<AkCimbEFT>? AkCimbEFT { get; set; }
        public ICollection<AkCimbEFT1>? AkCimbEFT1 { get; set; }
        public ICollection<AkPenyataPemungut>? AkPenyataPemungut { get; set; }
        public ICollection<AkBankRecon>? AkBankRecon { get; set; }

        //soft delete
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        public string? SebabHapus { get; set; }
        //soft delete end
    }
}