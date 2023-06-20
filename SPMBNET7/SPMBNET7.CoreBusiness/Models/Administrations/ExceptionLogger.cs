using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMBNET7.CoreBusiness.Models.Administrations
{
    public class ExceptionLogger
    {
        public int Id { get; set; }
        public string UrlRequest { get; set; } = string.Empty;
        public string ControllerName { get; set; } = string.Empty;
        public DateTime LogTime { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string ExceptionType { get; set; } = string.Empty;
        public string ExceptionMessage { get; set; } = string.Empty;
        public string ExceptionStackTrace { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public string TraceIdentifier { get; set; } = string.Empty;
    }
}
