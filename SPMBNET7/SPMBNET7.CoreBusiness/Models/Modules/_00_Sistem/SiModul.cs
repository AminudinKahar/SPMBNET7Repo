using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMBNET7.CoreBusiness.Models.Modules._00_Sistem
{
    public class SiModul
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(10)]
        [DisplayName("Id")]
        public string FuncId { get; set; } = string.Empty;
        [Required]
        [DisplayName("Perihal")]
        public string FuncName { get; set; } = string.Empty;
        [DisplayName("Model")]
        public string Model { get; set; } = string.Empty;
        [DisplayName("Model Asal")]
        public string FuncIdOld { get; set; } = string.Empty;
    }
}
