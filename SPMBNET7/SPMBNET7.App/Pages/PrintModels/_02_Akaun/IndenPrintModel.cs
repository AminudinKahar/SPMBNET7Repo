using SPMBNET7.CoreBusiness.Models.Administrations;
using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;

namespace SPMBNET7.App.Pages.PrintModels._02_Akaun
{
    public class IndenPrintModel
    {
        public string JumlahDalamPerkataan { get; set; }
        public string Username { get; set; }
        public string Jawatan { get; set; }
        public JNegeri Negeri { get; set; }
        public AkInden AkInden { get; set; }
        public AkInden1 AkInden1 { get; set; }
        public AkInden2 AkInden2 { get; set; }
        public AkPembekal AkPembekal { get; set; }
        public CompanyDetails CompanyDetail { get; set; }
    }
}
