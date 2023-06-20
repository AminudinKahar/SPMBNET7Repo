using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.CoreBusiness.Models.Modules._03_Sumber;

namespace SPMBNET7.Infrastructure.Carts._02_Akaun
{
    public class CartAtlet
    {
        //Atlet

        private List<SuProfil1> collection1 = new List<SuProfil1>();

        public virtual void AddItem1(
            int suProfilId,
            int? suAtletId,
            int jSukanId,
            int? jCarabayarId,
            string noCekEFT,
            DateTime? tarCekEFT,
            decimal amaun,
            decimal amaunsebelum,
            decimal tunggakan,
            string catatan,
            decimal jumlah
            )
        {
            SuProfil1 line = collection1.FirstOrDefault(p => p.SuAtletId == suAtletId)!;

            if (line == null)
            {
                collection1!.Add(new SuProfil1
                {
                    SuProfilId = suProfilId,
                    SuAtletId = suAtletId,
                    JSukanId = jSukanId,
                    JCaraBayarId = jCarabayarId,
                    NoCekEFT = noCekEFT,
                    TarCekEFT = tarCekEFT,
                    Amaun = amaun,
                    AmaunSebelum = amaunsebelum,
                    Tunggakan = tunggakan,
                    Catatan = catatan,
                    Jumlah = jumlah
                });
            }
        }

        public virtual void RemoveItem1(int? id) =>
            collection1.RemoveAll(l => l.SuAtletId == id);


        public virtual void Clear1() => collection1.Clear();

        public virtual IEnumerable<SuProfil1> Lines1 => collection1;
        //Atlet End

    }
}