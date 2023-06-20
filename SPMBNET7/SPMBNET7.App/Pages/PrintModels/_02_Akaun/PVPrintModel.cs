using SPMBNET7.CoreBusiness.Models.Administrations;
using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;

namespace SPMBNET7.App.Pages.PrintModels._02_Akaun
{
    public class PVPrintModel : AkPV
    {
        public string JumlahDalamPerkataan { get; set; }
        public string TarikhCekAtauEFT { get; set; }
        public string Username { get; set; }
        public string KodPenerima { get; set; }
        public string Penerima { get; set; }
        public string Poskod { get; set; }
        public string NamaBankPenerima { get; set; }
        //public string NoAkaunBank { get; set; }
        public string NoAkaunBankPenerima { get; set; }
        public decimal jumlahPOInden { get; set; }
        public decimal jumlahInbois { get; set; }
        public AkPV AkPV { get; set; }
        public new ICollection<AkPV2> AkPV2 { get; set; }
        public CompanyDetails CompanyDetail { get; set; }
        public JPenyemak Penyemak { get; set; }
        public JPelulus Pelulus { get; set; }
    }
}
