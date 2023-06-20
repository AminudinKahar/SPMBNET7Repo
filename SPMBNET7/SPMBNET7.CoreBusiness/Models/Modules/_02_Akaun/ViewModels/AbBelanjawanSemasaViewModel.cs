using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SPMBNET7.CoreBusiness.Models.Modules._02_Akaun.ViewModels
{
    public class AbBelanjawanSemasaViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Kumpulan Wang")]
        public int JKWId { get; set; }
        [Display(Name = "Bahagian")]
        public int JBahagianId { get; set; }
        [Required(ErrorMessage = "Tahun Diperlukan")]
        public string Tahun { get; set; } = string.Empty;
        [Required(ErrorMessage = "Tarikh Diperlukan")]
        [Display(Name = "Tarikh Hingga")]
        public DateTime TarHingga { get; set; }
        public string Objek { get; set; } = string.Empty;
        public string Perihalan { get; set; } = string.Empty;
        [Display(Name = "Paras")]
        public string Paras { get; set; } = string.Empty;
        [Display(Name = "Asal RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Asal { get; set; }
        [Display(Name = "Tambah RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Tambah { get; set; }
        [Display(Name = "Pindah RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Pindah { get; set; }
        [Display(Name = "Jumlah RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Jumlah { get; set; }
        [Display(Name = "Belanja RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Belanja { get; set; }
        // TBS - Tanggungan belum bayar
        [Display(Name = "TBS RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TBS { get; set; }
        [Display(Name = "Telah Guna RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TelahGuna { get; set; }
        [Display(Name = "Baki RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Baki { get; set; }
    }
}
