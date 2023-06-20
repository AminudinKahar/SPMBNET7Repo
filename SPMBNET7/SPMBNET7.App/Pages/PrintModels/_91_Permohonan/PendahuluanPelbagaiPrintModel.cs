using SPMBNET7.CoreBusiness.Models.Administrations;
using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.CoreBusiness.Models.Modules._91_Permohonan;

namespace SPMBNET7.App.Pages.PrintModels._91_Permohonan
{
    public class PendahuluanPelbagaiPrintModel
    {
        public string JumlahDalamPerkataan { get; set; }
        public string Username { get; set; }
        public SpPendahuluanPelbagai SpPendahuluanPelbagai { get; set; }
        public List<JTahapAktiviti> Tahap { get; set; }
        public CompanyDetails CompanyDetail { get; set; }
        public int BilAtl { get; set; }
        public int BilJul { get; set; }
        public int BilPeg { get; set; }
        public int BilTek { get; set; }
        public int BilUru { get; set; }
        public int Jumlah { get; set; }
        public decimal JumlahPerihal { get; set; }
    }
}
