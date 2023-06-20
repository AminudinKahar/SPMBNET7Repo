namespace SPMBNET7.CoreBusiness.Models.Modules._02_Akaun.ViewModels
{
    public class AkTunaiRuncitViewModel : AkTunaiRuncit
    {
        public string? KodKW { get; set; }
        public string? KodRujukan { get; set; }
        public decimal BakiAwal { get; set; }
        public DateTime? TarikhBakiAwal { get; set; }
        public string? KodAkaun { get; set; }
        public string? Perihal { get; set; }
        public decimal BakiLejarPanjar { get; set; }
        public string? Pemegang1 { get; set; }
        public bool IsPemegang { get; set; }
        public string? Pemegang2 { get; set; }
        public string? Pemegang3 { get; set; }
    }
}
