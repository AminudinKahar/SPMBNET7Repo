using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.CoreBusiness.Models.Modules._91_Permohonan;
using SPMBNET7.CoreBusiness.Models.Bases.BaseInterfaces;
using SPMBNET7.CoreBusiness.Models.Bases.Helper;

namespace SPMBNET7.CoreBusiness.Models.Modules._03_Sumber
{
    public class SuPekerja :AppLogHelper, ISoftDelete
    {
        public int Id { get; set; }
        [DisplayName("No Gaji")]
        public string NoGaji { get; set; } = string.Empty;
        [Required(ErrorMessage = "No Kad Pengenalan diperlukan")]
        [DisplayName("No KP")]
        public string NoKp { get; set; } = string.Empty;
        [Required(ErrorMessage = "Nama diperlukan")]
        public string Nama { get; set; } = string.Empty;
        [DisplayName("Alamat")]
        [Required(ErrorMessage = "Alamat diperlukan")]
        public string Alamat1 { get; set; } = string.Empty;
        public string? Alamat2 { get; set; } = string.Empty;
        public string? Alamat3 { get; set; } = string.Empty;
        [MaxLength(5)]
        [RegularExpression(@"^[\d+]*$", ErrorMessage = "Nombor sahaja dibenarkan")]
        public string Poskod { get; set; } = string.Empty;
        public string Bandar { get; set; } = string.Empty;
        [DisplayName("Negeri")]
        [Required(ErrorMessage = "Negeri diperlukan")]
        //[RegularExpression("[^0]+", ErrorMessage = "Sila pilih Negeri")]
        public int JNegeriId { get; set; }
        [DisplayName("No Telefon Rumah")]
        public string TelefonRumah { get; set; } = string.Empty;
        [DisplayName("No Telefon Bimbit")]
        public string TelefonBimbit { get; set; } = string.Empty;
        [Required(ErrorMessage = "Emel diperlukan")]
        [EmailAddress(ErrorMessage = "Emel tidak sah"), MaxLength(100)]
        public string Emel { get; set; } = string.Empty;
        [DefaultValue("0")]
        [DisplayName("Status Perkahwinan")]
        public int StatusKahwin { get; set; }
        [DefaultValue("0")]
        [DisplayName("Bilangan Anak")]
        public int BilAnak { get; set; }
        [DisplayName("Gaji Pokok")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal GajiPokok { get; set; }
        [DisplayName("Tarikh Masuk Kerja")]
        public DateTime TarikhMasukKerja { get; set; }
        [DisplayName("Tarikh Berhenti Kerja")]
        public DateTime? TarikhBerhentiKerja { get; set; }
        [DisplayName("Tarikh Pencen")]
        public DateTime? TarikhPencen { get; set; }
        [DisplayName("Nama Bank")]
        //[RegularExpression("[^0]+", ErrorMessage = "Sila pilih Bank")]
        [Required(ErrorMessage = "Bank diperlukan")]
        public int JBankId { get; set; }
        [DisplayName("Agama")]
        public int? JAgamaId { get; set; }
        [DisplayName("Bangsa")]
        public int? JBangsaId { get; set; }
        [DisplayName("Jawatan")]
        public string Jawatan { get; set; } = string.Empty;
        [DisplayName("Cara Bayar")]
        public int? JCaraBayarId { get; set; }
        [DisplayName("No Akaun Bank")]
        [Required(ErrorMessage = "No Akaun Bank diperlukan")]
        public string NoAkaunBank { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }

        //relationship
        [DisplayName("Negeri")]
        public JNegeri? JNegeri { get; set; }
        [DisplayName("Agama")]
        public JAgama? JAgama { get; set; }
        [DisplayName("Nama Bank")]
        public JBank? JBank { get; set; }
        [DisplayName("Bangsa")]
        public JBangsa? JBangsa { get; set; }
        public ICollection<SuTanggunganPekerja>? SuTanggungan { get; set; }
        [DisplayName("Cara Bayar")]
        public JCaraBayar? JCaraBayar { get; set; }
        public ICollection<AkPV>? AkPV { get; set; }
        public ICollection<AkTunaiCV>? AkTunaiCV { get; set; }
        public ICollection<SpPendahuluanPelbagai>? SpPendahuluanPelbagai { get; set; }
        public ICollection<JPelulus>? JPelulus { get; set; }
        public ICollection<JPenyemak>? JPenyemak { get; set; }
        public ICollection<AkCimbEFT>? AkCimbEFT { get; set; }
        public ICollection<AkCimbEFT1>? AkCimbEFT1 { get; set; }
        public ICollection<AkPenyataPemungut>? AkPenyataPemungut { get; set; }
        public ICollection<AkPVGanda>? AkPVGanda { get; set; }
        //relationship end

        //soft delete
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        public string? SebabHapus { get; set; }
        //soft delete end
    }
}
