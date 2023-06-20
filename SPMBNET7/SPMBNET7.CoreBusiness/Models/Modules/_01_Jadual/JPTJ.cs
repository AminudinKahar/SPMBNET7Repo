using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SPMBNET7.CoreBusiness.Models.Bases.BaseInterfaces;
using SPMBNET7.CoreBusiness.Models.Bases.Helper;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SPMBNET7.CoreBusiness.Models.Modules._01_Jadual
{
    public class JPTJ : AppLogHelper, ISoftDelete
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(3)]
        public string Kod { get; set; } = string.Empty;
        [Required]
        [MaxLength(100)]
        public string Perihal { get; set; } = string.Empty;
        [DisplayName("Kump. Wang")]
        public int? JKWId { get; set; }
        public JKW? JKW { get; set; }
        public ICollection<JBahagian>? JBahagian { get; set; }

        //soft delete
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        public string? SebabHapus { get; set; }
        //soft delete end
    }
}
