using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMBNET7.CoreBusiness.Models.Modules._02_Akaun.ViewModels
{
    public class AbBukuVotDetailViewModel
    {
        public int Id { get; set; }
        public DateTime Tarikh { get; set; }
        public string? JKW { get; set; }
        public string? JBahagian { get; set; }
        public string? Vot { get; set; }
        [DisplayName("Kod")]
        public string? Kod { get; set; }
        public string? Nama { get; set; }
        [DisplayName("No Rujukan")]
        public string? NoRujukan { get; set; }
        [DisplayName("Debit RM")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Debit { get; set; }
        [DisplayName("Kredit RM")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Kredit { get; set; }
        [DisplayName("Tanggungan RM")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Tanggungan { get; set; }
        [DisplayName("Liabiliti RM")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Liabiliti { get; set; }
        [DisplayName("Baki RM")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Baki { get; set; }
    }
}
