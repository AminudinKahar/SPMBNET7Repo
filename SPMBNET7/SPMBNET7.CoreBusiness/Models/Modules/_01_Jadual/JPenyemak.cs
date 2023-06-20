﻿using SPMBNET7.CoreBusiness.Models.Modules._03_Sumber;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using SPMBNET7.CoreBusiness.Models.Modules._91_Permohonan;
using SPMBNET7.CoreBusiness.Models.Bases.BaseInterfaces;
using SPMBNET7.CoreBusiness.Models.Bases.Helper;

namespace SPMBNET7.CoreBusiness.Models.Modules._01_Jadual
{
    public class JPenyemak : AppLogHelper, ISoftDelete
    {
        public int Id { get; set; }
        [DisplayName("Anggota")]
        [RegularExpression("[^0]+", ErrorMessage = "Sila pilih Anggota")]
        [Required(ErrorMessage = "Anggota Diperlukan")]
        public int SuPekerjaId { get; set; }
        public SuPekerja? SuPekerja { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayName("Amaun Diluluskan RM")]
        [Required(ErrorMessage = "Minimum Amaun Diperlukan")]
        public decimal MinAmaun { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        [Required(ErrorMessage = "Maksimum Amaun Diperlukan")]
        public decimal MaksAmaun { get; set; }
        [DisplayName("Nota Minta")]
        public bool IsNotaMinta { get; set; }
        [DisplayName("Pesanan Tempatan")]
        public bool IsPO { get; set; }
        [DisplayName("Inbois Pembekal")]
        public bool IsBelian { get; set; }
        [DisplayName("Baucer Pembayaran")]
        public bool IsPV { get; set; }
        [DisplayName("Pendahuluan Pelbagai")]
        public bool IsPendahuluan { get; set; }

        [DisplayName("Invois Dikeluarkan")]
        public bool IsInvois { get; set; }
        // laporan
        [DisplayName("Laporan Buku Vot")]
        public bool IsLaporanBukuVot { get; set; }
        [DisplayName("Hapus")]
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        public string? SebabHapus { get; set; }
        public ICollection<SpPendahuluanPelbagai>? SpPendahuluanPelbagai { get; set; } 
    }
}
