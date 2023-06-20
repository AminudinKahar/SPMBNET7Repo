using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPMBNET7.CoreBusiness.Models.Modules._91_Permohonan
{
    public class SpPendahuluanPelbagai2
    {
        public int Id { get; set; }
        public int SpPendahuluanPelbagaiId { get; set; }
        public int Indek { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Baris { get; set; }
        public string? Perihal { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Kadar { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Bil { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Bulan { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Jumlah { get; set; }
    }
}