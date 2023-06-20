using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SPMBNET7.CoreBusiness.Models.Modules._03_Sumber;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMBNET7.CoreBusiness.Models.Administrations
{
    public class AppLog
    {
        public int Id { get; set; }
        [DisplayName("User Id")]
        public string UserId { get; set; } = string.Empty;
        [DisplayName("Tarikh")]
        public DateTime LgDate { get; set; }
        [DisplayName("Modul")]
        public string LgModule { get; set; } = string.Empty;
        [DisplayName("Operasi")]
        public string LgOperation { get; set; } = string.Empty;
        [DisplayName("Nota")]
        public string LgNote { get; set; } = string.Empty;
        [DisplayName("No Rujukan")]
        public string NoRujukan { get; set; } = string.Empty;
        [DisplayName("Id Rujukan")]
        public int IdRujukan { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Jumlah { get; set; }
        public string SysCode { get; set; } = string.Empty;
        public int? SuPekerjaId { get; set; }
        public SuPekerja? SuPekerja { get; set; }
    }
}
