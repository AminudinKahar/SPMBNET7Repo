
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SPMBNET7.CoreBusiness.Models.Bases.Helper
{
    public class AppLogHelper
    {
        // log
        public int? SuPekerjaMasukId { get; set; }
        [ValidateNever]
        public string UserId { get; set; } = string.Empty;
        [DisplayName("Tarikh Masuk")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime TarMasuk { get; set; }
        public int? SuPekerjaKemaskiniId { get; set; }
        [ValidateNever]
        public string UserIdKemaskini { get; set; } = string.Empty;
        [DisplayName("Tarikh Kemaskini")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? TarKemaskini { get; set; } = DateTime.Now;
        //log end
    }
}
