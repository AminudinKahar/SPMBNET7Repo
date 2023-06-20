using SPMBNET7.CoreBusiness.Models.Administrations;
using SPMBNET7.CoreBusiness.Models.Modules._03_Sumber;

namespace SPMBNET7.App.Pages.PrintModels._02_Akaun
{
    public class SuProfilAtletPrintModel
    {
        public string JumlahDalamPerkataan { get; set; }
        public string Username { get; set; }
        public SuProfil SuProfil { get; set; }
        public CompanyDetails CompanyDetail { get; set; }
    }
}
