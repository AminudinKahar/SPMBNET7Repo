using SPMBNET7.CoreBusiness.Models.Administrations;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.CoreBusiness.Models.Modules._03_Sumber;

namespace SPMBNET7.App.Pages.PrintModels._02_Akaun
{
    public class JurnalPrintModel
    {
        public string Username { get; set; }
        public SuPekerja SuPekerja { get; set; }
        public AkJurnal AkJurnal { get; set; }
        public List<RingkasanPrintModel> Ringkasan { get; set; } = new List<RingkasanPrintModel>();
        public CompanyDetails CompanyDetail { get; set; }
        public string JumlahDebitDalamPerkataan { get; set; }
        public string JumlahKreditDalamPerkataan { get; set; }
    }
}
