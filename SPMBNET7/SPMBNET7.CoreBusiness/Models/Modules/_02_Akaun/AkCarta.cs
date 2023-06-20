﻿using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.CoreBusiness.Models.Modules._91_Permohonan;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SPMBNET7.CoreBusiness.Models.Modules._03_Sumber;
using SPMBNET7.CoreBusiness.Models.Bases.BaseInterfaces;
using SPMBNET7.CoreBusiness.Models.Bases.Helper;

namespace SPMBNET7.CoreBusiness.Models.Modules._02_Akaun
{
    public class AkCarta : AppLogHelper, ISoftDelete
    {
        //field
        public int Id { get; set; }
        [MaxLength(6)]
        [Required(ErrorMessage = "Kod Carta Diperlukan.")]
        [RegularExpression(@"^(([A-Z])\d{5})*$", ErrorMessage = "Contoh A99999.")]
        public string Kod { get; set; } = string.Empty;
        [MaxLength(100)]
        [Required(ErrorMessage = "Perihal Diperlukan.")]
        public string Perihal { get; set; } = string.Empty;
        [MaxLength(1)]
        [Display(Name = "Debit / Kredit")]
        [Required(ErrorMessage = "Pilih Debit atau Kredit.")]
        public string DebitKredit { get; set; } = string.Empty;
        [MaxLength(1)]
        [Display(Name = "Umum / Detail")]
        [Required(ErrorMessage = "Pilih Umum atau Detail.")]
        public string UmumDetail { get; set; } = string.Empty;
        [MaxLength(100)]
        [Display(Name = "Catatan 1")]
        public string? Catatan1 { get; set; }
        [MaxLength(100)]
        [Display(Name = "Catatan 2")]
        public string? Catatan2 { get; set; }
        [Display(Name = "Baki RM (Setakat 31 Dis 2021)")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Baki { get; set; }
        // cek if kod akaun is in bajet or not
        [Display(Name = "Tidak Dikira Dalam Peruntukan")]
        public bool IsBajet { get; set; } = true;
        //field end

        //Relationship
        [Display(Name = "KW")]
        [Required(ErrorMessage = "Kumpulan Wang Diperlukan.")]
        public int JKWId { get; set; }
        [Display(Name = "KW")]
        public JKW? JKW { get; set; }
        [Display(Name = "Jenis")]
        [Required(ErrorMessage = "Jenis Diperlukan.")]
        public int JJenisId { get; set; }
        [Display(Name = "Jenis")]
        public JJenis? JJenis { get; set; }
        [Display(Name = "Paras")]
        [Required(ErrorMessage = "Paras Diperlukan.")]
        public int JParasId { get; set; }
        [Display(Name = "Paras")] 
        public JParas? JParas { get; set; }
        public ICollection<AkTerima1>? AkTerima1 { get; set; }
        public ICollection<AkBank>? AkBank { get; set; }
        public virtual ICollection<AkAkaun>? AkAkaun1 { get; set; }
        public virtual ICollection<AkAkaun>? AkAkaun2 { get; set; }
        public ICollection<AkPO1>? AkPO1 { get; set; }
        public ICollection<AkPOLaras1>? AkPOLaras1 { get; set; }
        public ICollection<AkJurnal1>? AkJurnalDebit { get; set; }
        public ICollection<AkJurnal1>? AkJurnalKredit { get; set; }
        public ICollection<AkBelian>? AkBelian { get; set; }
        public ICollection<AkBelian1>? AkBelian1 { get; set; }
        public ICollection<AkPV1>? AkPV1 { get; set; }
        public ICollection<AbBukuVot>? Vot { get; set; }
        public ICollection<SpPendahuluanPelbagai>? SpPendahuluanPelbagai { get; set; }
        public ICollection<AkTunaiRuncit>? AkTunaiRuncit { get; set; }
        public ICollection<AkTunaiCV1>? AkTunaiCV1 { get; set; }
        public ICollection<AkNotaMinta1>? AkNotaMinta1 { get; set; }
        public ICollection<AbWaran1>? AbWaran1 { get; set; }
        public ICollection<SuProfil>? SuProfil { get; set; }
        public ICollection<AkInvois1>? AkInvois1 { get; set; }
        public ICollection<AkInvois>? AkInvois { get; set; }
        public ICollection<AkInden1>? AkInden1 { get; set; }
        public ICollection<AkPenyataPemungut1>? AkPenyataPemungut1 { get; set; }
        public ICollection<AkNotaDebitKreditBelian1>? AkNotaDebitKreditBelian1 { get; set; }
        public ICollection<AkTunaiLejar>? AkTunaiLejar { get; set; }
        public ICollection<AkNotaDebitKreditBelian>? AkNotaDebitKreditBelian { get; set; }

        //relationship end

        //soft delete
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        public string? SebabHapus { get; set; }
        //soft delete end
    }
}