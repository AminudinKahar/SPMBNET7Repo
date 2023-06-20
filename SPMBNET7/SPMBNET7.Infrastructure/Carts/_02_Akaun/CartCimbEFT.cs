using SPMBNET7.CoreBusiness._Enums;
using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.CoreBusiness.Models.Modules._03_Sumber;

namespace SPMBNET7.Infrastructure.Carts._02_Akaun
{
    public class CartCimbEFT
    {
        // CimbEFT 1
        private List<AkCimbEFT1> collection1 = new List<AkCimbEFT1>();

        public virtual void AddItem1(
            int akCimbEFTId,
            int Indek,
            int akPVId,
            KategoriPenerima flPenerimaEFT,
            int? akPembekalId,
            int? suPekerjaId,
            int? suAtletId,
            int? suJurulatihId,
            decimal amaun,
            string noCek,
            string catatan,
            int? jBankId,
            int? flStatus
            )
        {
            collection1.Add(new AkCimbEFT1
            {
                AkCimbEFTId = akCimbEFTId,
                Indek = Indek,
                AkPVId = akPVId,
                FlPenerimaEFT = flPenerimaEFT,
                AkPembekalId = akPembekalId,
                SuPekerjaId = suPekerjaId,
                SuJurulatihId = suJurulatihId,
                SuAtletId = suAtletId,
                Amaun = amaun,
                NoCek = noCek,
                Catatan = catatan,
                JBankId = jBankId,
                FlStatus = flStatus
            });
        }

        public virtual void RemoveItem1(int id) =>
            collection1.RemoveAll(l => l.Indek == id);


        public virtual void Clear1() => collection1.Clear();

        public virtual IEnumerable<AkCimbEFT1> Lines1 => collection1;
        // CimbEFT 1 end
    }
}