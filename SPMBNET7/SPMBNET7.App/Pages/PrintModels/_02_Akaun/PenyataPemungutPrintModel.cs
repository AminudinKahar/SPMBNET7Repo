using SPMBNET7.CoreBusiness.Models.Administrations;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;

namespace SPMBNET7.App.Pages.PrintModels._02_Akaun
{
    public class PenyataPemungutPrintModel
    {
        public string JumlahDalamPerkataan { get; set; }
        public string Username { get; set; }
        public AkPenyataPemungut AkPenyataPemungut { get; set; }
        public CompanyDetails CompanyDetail { get; set; }
    }
}
