using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMBNET7.CoreBusiness.Models.Bases.Helper;
using SPMBNET7.CoreBusiness.Models.Bases.BaseInterfaces;

namespace SPMBNET7.CoreBusiness.Models.Modules._01_Jadual
{
    public class JProfilKategori : AppLogHelper, ISoftDelete
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Kod diperlukan")]
        [MaxLength(2, ErrorMessage = "Input tidak boleh melebihi 2 aksara")]
        public string Kod { get; set; } = string.Empty;
        [Required(ErrorMessage = "Perihal diperlukan")]
        [MaxLength(100, ErrorMessage = "Input tidak boleh melebihi 100 aksara")]
        public string Perihal { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayName("Kadar Geran RM")]
        public decimal KadarGeran { get; set; }
        //public ICollection<SuJurulatih> SuJurulatih { get; set; }
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        public string? SebabHapus { get; set; }
    }
}
