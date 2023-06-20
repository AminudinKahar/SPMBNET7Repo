using SPMBNET7.CoreBusiness.Models.Administrations;
using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;

namespace SPMBNET7.App.Pages.PrintModels._02_Akaun
{
    public class TerimaanPrintModel : AkTerima
    {
        public string JumlahDalamPerkataan { get; set; }
        public string username { get; set; }
        public string penyedia { get; set; }
        public JNegeri Negeri { get; set; }
        public AkTerima AkTerima { get; set; }
        public CompanyDetails CompanyDetail { get; set; }
    }
}
