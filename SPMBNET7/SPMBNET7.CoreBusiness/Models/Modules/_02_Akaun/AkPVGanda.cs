﻿using SPMBNET7.CoreBusiness._Enums;
using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.CoreBusiness.Models.Modules._03_Sumber;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using SPMBNET7.CoreBusiness.Models.Bases.BaseInterfaces;

namespace SPMBNET7.CoreBusiness.Models.Modules._02_Akaun
{
    public class AkPVGanda : IRecon
    {
        // note :
        // lama
        // FlKategoriPenerima = 1 -- pekerja
        // FlKategoriPenerima = 2 -- atlet
        // FlKategoriPenerima = 3 -- jurulatih
        // FlKategoriPenerima = 0 -- null

        // baru
        // FlKategoriPenerima = 0 ( Am / Null )
        // FlKategoriPenerima = 1 ( pembekal )
        // FlKategoriPenerima = 2 ( pekerja )
        // FlKategoriPenerima = 3 ( pemegang panjar )
        // FlKategoriPenerima = 4 ( jurulatih )
        // FlKategoriPenerima = 5 ( atlet )
        public int Id { get; set; }
        public int AkPVId { get; set; }
        public AkPV? AkPV { get; set; }
        public int Indek { get; set; }
        public KategoriPenerima FlKategoriPenerima { get; set; }
        [DisplayName("Anggota")]
        public int? SuPekerjaId { get; set; }
        public SuPekerja? SuPekerja { get; set; }
        [DisplayName("Atlet")]
        public int? SuAtletId { get; set; }
        public SuAtlet? SuAtlet { get; set; }
        [DisplayName("Jurulatih")]

        public int? SuJurulatihId { get; set; }

        public SuJurulatih? SuJurulatih { get; set; }

        public string Nama { get; set; } = string.Empty;
        [DisplayName("No KP")]
        public string? NoKp { get; set; }
        [DisplayName("No Akaun")]
        public string NoAkaun { get; set; } = string.Empty;
        [DisplayName("Bank")]
        public int? JBankId { get; set; }
        public JBank? JBank { get; set; }
        [DisplayName("Amaun RM")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amaun { get; set; }
        [DisplayName("No Cek / EFT")]
        public string? NoCekAtauEFT { get; set; }
        [DisplayName("Tar Cek / EFT")]
        public DateTime? TarCekAtauEFT { get; set; }
        [DisplayName("Cara Bayar")]
        public int? JCaraBayarId { get; set; }
        public JCaraBayar? JCaraBayar { get; set; }
        public int FlTunai { get; set; }
        public DateTime? TarTunai { get; set; }
        public ICollection<AkPadananPenyata>? AkPadananPenyata { get; set; }
    }
}