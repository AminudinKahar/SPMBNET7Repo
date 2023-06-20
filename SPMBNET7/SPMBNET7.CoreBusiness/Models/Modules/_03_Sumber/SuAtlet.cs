using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMBNET7.CoreBusiness.Models.Bases.BaseInterfaces;
using SPMBNET7.CoreBusiness.Models.Bases.Helper;

namespace SPMBNET7.CoreBusiness.Models.Modules._03_Sumber
{
    public class SuAtlet : AppLogHelper, ISoftDelete
    {
        public int Id { get; set; }
        [DisplayName("Kod Atlet")] 
        public string KodAtlet { get; set; } = string.Empty;
        [Required(ErrorMessage = "No Kad Pengenalan diperlukan")]
        [DisplayName("No KP")]
        public string NoKp { get; set; } = string.Empty;
        [Required(ErrorMessage = "Nama diperlukan")]
        public string Nama { get; set; } = string.Empty;
        [DisplayName("Alamat")]
        public string Alamat1 { get; set; } = string.Empty;
        public string? Alamat2 { get; set; }
        public string? Alamat3 { get; set; }
        [MaxLength(5)]
        [RegularExpression(@"^[\d+]*$", ErrorMessage = "Nombor sahaja dibenarkan")]
        public string Poskod { get; set; } = string.Empty;
        public string Bandar { get; set; } = string.Empty;
        [DisplayName("Negeri")]
        [Required(ErrorMessage = "Negeri diperlukan")]
        [RegularExpression("[^0]+", ErrorMessage = "Sila pilih Negeri")]
        public int JNegeriId { get; set; }
        [DisplayName("No Telefon")]
        public string Telefon { get; set; } = string.Empty;
        [EmailAddress(ErrorMessage = "Emel tidak sah"), MaxLength(100)]
        public string? Emel { get; set; }
        //FlStatus
        //  0 = Tidak Aktif 
        //  1 = Aktif
        [DisplayName("Status Aktif")]
        public int FlStatus { get; set; }
        [DisplayName("Tarikh Aktif")]
        [Required(ErrorMessage = "Tarikh Aktif diperlukan")]
        public DateTime TarikhAktif { get; set; }
        [DisplayName("Tarikh Berhenti")]
        public DateTime? TarikhBerhenti { get; set; }
        [DisplayName("Nama Bank")]
        [RegularExpression("[^0]+", ErrorMessage = "Sila pilih Bank")]
        [Required(ErrorMessage = "Bank diperlukan")]
        public int JBankId { get; set; }
        [DisplayName("Agama")]
        public int? JAgamaId { get; set; }
        [DisplayName("Bangsa")]
        public int? JBangsaId { get; set; }
        [DisplayName("Jawatan")]
        public string? Jawatan { get; set; }
        [DisplayName("Cara Bayar")]
        [Required(ErrorMessage = "Sila pilih Cara Bayar")]
        public int? JCaraBayarId { get; set; }
        [DisplayName("No Akaun Bank")]
        public string NoAkaunBank { get; set; } = string.Empty;

        //relationship
        [DisplayName("Cara Bayar")]
        public JCaraBayar? JCaraBayar { get; set; }
        [DisplayName("Sukan")]
        public JSukan? JSukan { get; set; }
        [DisplayName("Sukan")]
        [Required(ErrorMessage = "Sukan diperlukan")]
        [RegularExpression("[^0]+", ErrorMessage = "Sila pilih Sukan")]
        public int JSukanId { get; set; }
        [DisplayName("Negeri")]
        public JNegeri? JNegeri { get; set; }
        [DisplayName("Agama")]
        public JAgama? JAgama { get; set; }
        [DisplayName("Nama Bank")]
        public JBank? JBank { get; set; }
        [DisplayName("Bangsa")] 
        public JBangsa? JBangsa { get; set; }
        public ICollection<AkCimbEFT1>? AkCimbEFT1 { get; set; }
        public ICollection<AkPVGanda>? AkPVGanda { get; set; }
        //relationship end

        //soft delete
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        public string? SebabHapus { get; set; }
        //soft delete end
    }
}
