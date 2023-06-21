namespace SPMBNET7.App.Pages.PrintModels._02_Akaun
{
    public class RingkasanPrintModel
    {
        public RingkasanPrintModel() { }

        public string Bahagian { get; set; } = string.Empty;
        public string KodAkaun { get; set; } = string.Empty;
        public string Perihal { get; set; } = string.Empty;
        public string Debit { get; set; } = string.Empty;
        public string Kredit { get; set; } = string.Empty;
        public decimal DebitDecimal { get; set; }
        public decimal KreditDecimal { get; set; }
        public string Kuantiti { get; set; } = string.Empty;
    }
}
