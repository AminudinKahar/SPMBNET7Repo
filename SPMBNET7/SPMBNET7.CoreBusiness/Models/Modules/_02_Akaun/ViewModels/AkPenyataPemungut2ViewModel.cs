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
    public class AkPenyataPemungut2ViewModel
    {
        public int Id { get; set; }
        public int Indek { get; set; }
        public int AkPenyataPemungutId { get; set; }
        public AkPenyataPemungut? AkPenyataPemungut { get; set; }
        public DateTime Tarikh { get; set; }
        public string? NoResit { get; set; }
        public int AkTerima2Id { get; set; }
        public AkTerima2? AkTerima2 { get; set; }
        [DisplayName("Amaun RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amaun { get; set; }
    }
}
