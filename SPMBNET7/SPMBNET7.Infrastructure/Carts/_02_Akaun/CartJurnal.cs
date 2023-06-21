using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;

namespace SPMBNET7.Infrastructure.Carts._02_Akaun
{
    public class CartJurnal
    {
        private List<AkJurnal1> collection1 = new List<AkJurnal1>();
        public virtual void AddItem1(
            int AkJurnalId,
            int Indeks,
            int JBahagianDebitId,
            JBahagian? jBahagianDebit,
            int AkCartaDebitId,
            AkCarta? akCartaDebit,
            int JBahagianKreditId,
            JBahagian? jBahagianKredit,
            int AkCartaKreditId,
            AkCarta? akCartaKredit,
            decimal Amaun
            )
        {
            AkJurnal1 line = collection1.FirstOrDefault(p => p.JBahagianDebitId == p.JBahagianDebitId
                    && p.AkCartaDebitId == AkCartaDebitId
                    && p.JBahagianKreditId == JBahagianKreditId
                    && p.AkCartaKreditId == AkCartaKreditId
                    && p.Indeks == Indeks)!;

            if (line == null)
            {
                collection1.Add(new AkJurnal1
                {
                    AkJurnalId = AkJurnalId,
                    Indeks = Indeks,
                    JBahagianDebitId = JBahagianDebitId,
                    JBahagianDebit = jBahagianDebit,
                    AkCartaDebitId = AkCartaDebitId,
                    AkCartaDebit = akCartaDebit,
                    JBahagianKreditId = JBahagianKreditId,
                    JBahagianKredit = jBahagianKredit,
                    AkCartaKreditId = AkCartaKreditId,
                    AkCartaKredit = akCartaKredit,
                    Amaun = Amaun
                });
            }
        }

        public virtual void RemoveItem1(int JBahagianDebitId, int AkCartaDebitId, int JBahagianKreditId, int AkCartaKreditId, int IndeksLama) =>
            collection1.RemoveAll(l => l.JBahagianDebitId == JBahagianDebitId && l.AkCartaDebitId == AkCartaDebitId
                                    && l.JBahagianKreditId == JBahagianKreditId && l.AkCartaKreditId == AkCartaKreditId
                                    && l.Indeks == IndeksLama);
        //public virtual void RemoveItem1(int id) =>
        //    collection1.RemoveAll(l => l.AkCartaId == id);

        public virtual void Clear1() => collection1.Clear();

        public virtual IEnumerable<AkJurnal1> Lines1 => collection1;
    }
}