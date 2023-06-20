﻿using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.CoreBusiness.Models.Modules._91_Permohonan;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using SPMBNET7.CoreBusiness.Models.Modules._03_Sumber;
using SPMBNET7.CoreBusiness.Models.Bases.BaseInterfaces;
using SPMBNET7.CoreBusiness.Models.Bases.Helper;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SPMBNET7.CoreBusiness.Models.Modules._04_Aset;

namespace SPMBNET7.CoreBusiness.Models.Modules._01_Jadual
{
    public class JBahagian : AppLogHelper, ISoftDelete
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Kod Diperlukan")]
        public string Kod { get; set; } = string.Empty;
        [Required(ErrorMessage = "Perihal Diperlukan")]
        public string Perihal { get; set; } = string.Empty;
        [ValidateNever]
        public JKW? JKW { get; set; }
        [DisplayName("Kumpulan Wang")]
        [Required(ErrorMessage = "Kumpulan Wang Diperlukan")]
        public int JKWId { get; set; }
        [ValidateNever]
        public JPTJ? JPTJ { get; set; }
        [DisplayName("PTJ")]
        public int? JPTJId { get; set; }
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        public string? SebabHapus { get; set; }

        public ICollection<AbWaran>? AbWaran { get; set; }
        public ICollection<AbWaran1>? AbWaran1 { get; set; }
        public ICollection<AbBukuVot>? abBukuVot { get; set; }
        public ICollection<AkAkaun>? AkAkaun { get; set; }
        public ICollection<AkBelian>? AkBelian { get; set; }
        public ICollection<AkJurnal>? AkJurnal { get; set; }
        public ICollection<AkJurnal1>? AkJurnalDebit { get; set; }
        public ICollection<AkJurnal1>? AkJurnalKredit { get; set; }
        public ICollection<AkNotaMinta>? AkNotaMinta { get; set; }
        public ICollection<AkPO>? AkPO { get; set; }
        public ICollection<AkInden>? AkInden { get; set; }
        public ICollection<AkPOLaras>? AkPOLaras { get; set; }
        public ICollection<AkPV>? AkPV { get; set; }
        public ICollection<AkTerima>? AkTerima { get; set; }
        public ICollection<AkTunaiLejar>? AkTunaiLejar { get; set; }
        public ICollection<AkTunaiRuncit>? AkTunaiRuncit { get; set; }
        public ICollection<SpPendahuluanPelbagai>? SpPendahuluanPelbagai { get; set; }
        public ICollection<AkBank>? AkBank { get; set; }
        public ICollection<SuProfil>? SuProfil { get; set; }
        public ICollection<AkInvois>? AkInvois { get; set; }
        public ICollection<AkPenyataPemungut1>? AkPenyataPemungut1 { get; set; }
        public ICollection<AkNotaDebitKreditBelian>? AkNotaDebitKreditBelian { get; set; }
        public ICollection<AsAset>? AsAset { get; set; }
    }
}
