﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SPMBNET7.CoreBusiness.Models.Bases.BaseInterfaces;
using SPMBNET7.CoreBusiness.Models.Bases.Helper;
using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SPMBNET7.CoreBusiness.Models.Modules._02_Akaun
{
    public class AbWaran : AppLogHelper, ISoftDelete
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "No Rujukan diperlukan")]
        [DisplayName("No Rujukan")]
        public string NoRujukan { get; set; } = string.Empty;
        [MaxLength(4)]
        [DisplayName("Tahun Bel.")]
        [Required(ErrorMessage = "Tahun diperlukan")]
        public string Tahun { get; set; } = string.Empty;
        [Required(ErrorMessage = "Tarikh diperlukan")]
        public DateTime Tarikh { get; set; }
        [DisplayName("Tarikh Posting")]
        public DateTime? TarikhPosting { get; set; }
        [DisplayName("Jumlah RM")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Jumlah { get; set; }

        //flag
        // WPA = 0(asal); WPT = 1 (tambah/tarik balik); WPP = 2(pindah);
        [DisplayName("Jenis Waran")]
        public int FlJenisWaran { get; set; }
        //flag
        // Dalam Bahagian = 0; Antara Bahagian = 1;
        [DisplayName("Jenis Pindahan")]
        public int FlJenisPindahan { get; set; }

        public string? Catatan { get; set; }
        [DisplayName("Hapus")]
        [DefaultValue("0")]
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        public string? SebabHapus { get; set; }
        [DisplayName("Posting")]
        [DefaultValue("0")]
        public int FlPosting { get; set; }
        [DisplayName("Cetak")]
        [DefaultValue("0")]
        public int FlCetak { get; set; }
        //flag end

        //relationship
        [Required(ErrorMessage = "Kumpulan Wang Diperlukan")]
        [DisplayName("Kumpulan Wang")]
        public int JKWId { get; set; }
        public JKW? JKW { get; set; }
        [DisplayName("Bahagian")]
        public int? JBahagianId { get; set; }
        public JBahagian? JBahagian { get; set; }
        public ICollection<AbWaran1>? AbWaran1 { get; set; }
        //relationship end
    }
}
