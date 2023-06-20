using SPMBNET7.CoreBusiness.Models.Administrations;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;

namespace SPMBNET7.App.Pages.PrintModels._02_Akaun
{
    public class NotaMintaPrintModel
    {
        public string JumlahDalamPerkataan { get; set; }
        public decimal JumlahPerihal { get; set; }
        public string TarikhKewangan { get; set; }
        public string username { get; set; }
        public AkNotaMinta AkNotaMinta { get; set; }
        public CompanyDetails CompanyDetail { get; set; }
    }
}
