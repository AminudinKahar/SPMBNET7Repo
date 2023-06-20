using SPMBNET7.CoreBusiness.Models.Administrations;
using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;

namespace SPMBNET7.App.Pages.PrintModels._02_Akaun
{
    public class POPrintModel
    {
        public string JumlahDalamPerkataan { get; set; }
        public string Username { get; set; }
        public string Jawatan { get; set; }
        public JNegeri Negeri { get; set; }
        public AkPO AkPO { get; set; }
        public AkPO1 AkPO1 { get; set; }
        public AkPO2 AkPO2 { get; set; }
        public AkPembekal AkPembekal { get; set; }
        public CompanyDetails CompanyDetail { get; set; }
    }
}
