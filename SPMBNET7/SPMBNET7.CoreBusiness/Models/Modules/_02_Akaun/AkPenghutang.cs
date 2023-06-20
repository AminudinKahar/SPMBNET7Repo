﻿using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SPMBNET7.CoreBusiness.Models.Bases.BaseInterfaces;
using SPMBNET7.CoreBusiness.Models.Bases.Helper;

namespace SPMBNET7.CoreBusiness.Models.Modules._02_Akaun
{
    public class AkPenghutang : AppLogHelper, ISoftDelete
    {
        //field
        public int Id { get; set; }
        [MaxLength(5)]
        [Display(Name = "Kod Syarikat")]
        public string KodSykt { get; set; } = string.Empty;//A0000
        [Required(ErrorMessage = "Nama Syarikat diperlukan"), MaxLength(100)]
        [Display(Name = "Nama Syarikat")]
        public string NamaSykt { get; set; } = string.Empty;
        [Required(ErrorMessage = "Nombor Pendaftaran Syarikat diperlukan"), MaxLength(20)]
        [Display(Name = "No Pendaftaran")]
        public string NoPendaftaran { get; set; } = string.Empty;
        [Required(ErrorMessage = "Alamat diperlukan"), MaxLength(100)]
        [Display(Name = "Alamat 1")]
        public string Alamat1 { get; set; } = string.Empty;
        [MaxLength(100)]
        [Display(Name = "Alamat 2")]
        public string? Alamat2 { get; set; }
        [MaxLength(100)]
        [Display(Name = "Alamat 3")]
        public string? Alamat3 { get; set; }
        [Required(ErrorMessage = "Poskod diperlukan")]
        [MaxLength(5)]
        [RegularExpression(@"^[\d+]*$", ErrorMessage = "Nombor sahaja dibenarkan")]
        public string Poskod { get; set; } = string.Empty;//nvarchar
        [Required(ErrorMessage = "Bandar diperlukan"), MaxLength(100)]
        public string Bandar { get; set; } = string.Empty;

        [Display(Name = "No Telefon")]
        public string? Telefon1 { get; set; }
        [EmailAddress(ErrorMessage = "Emel tidak sah"), MaxLength(100)]
        public string Emel { get; set; } = string.Empty;
        [Required(ErrorMessage = "Nombor Akaun Bank diperlukan"), MaxLength(20)]
        [Display(Name = "No Akaun Bank")]
        public string AkaunBank { get; set; } = string.Empty;
        [Display(Name = "Baki Awal RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal BakiAwal { get; set; }
        //field end

        //Relationship
        [Required(ErrorMessage = "Negeri diperlukan")]
        //[RegularExpression("[^0]+", ErrorMessage = "Sila pilih Negeri")]
        [Display(Name = "Negeri")]
        public int JNegeriId { get; set; }
        [Display(Name = "Negeri")]
        public JNegeri? JNegeri { get; set; }
        [Required(ErrorMessage = "Nama Bank diperlukan")]
        //[RegularExpression("[^0]+", ErrorMessage = "Sila pilih Bank")]
        [Display(Name = "Bank")]
        public int JBankId { get; set; }
        [Display(Name = "Bank")]
        public JBank? JBank { get; set; }
        public ICollection<AkInvois>? AkInvois { get; set; }
        public ICollection<AkTerima>? AkTerima { get; set; }
        //relationship end

        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        public string? SebabHapus { get; set; }
    }
}