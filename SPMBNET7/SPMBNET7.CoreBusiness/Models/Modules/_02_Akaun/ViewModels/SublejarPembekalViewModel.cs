using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace SPMBNET7.CoreBusiness.Models.Modules._02_Akaun.ViewModels
{
    public class SublejarPembekalViewModel
    {
        public int Id { get; set; }
        public DateTime Tarikh { get; set; }
        public string? Rujukan { get; set; }
        [DisplayName("Bayaran RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Bayaran { get; set; }
        [DisplayName("Hutang RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Hutang { get; set; }
        [DisplayName("Baki RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Baki { get; set; }
    }
}