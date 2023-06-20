using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.CoreBusiness.Models.Modules._03_Sumber;

namespace SPMBNET7.Infrastructure.Carts._02_Akaun
{
    public class CartTunaiRuncit
    {
        //Tunai Pemegang

        private List<AkTunaiPemegang> collection1 = new List<AkTunaiPemegang>();

        public virtual void AddItem1(
            int akTunaiRuncitId,
            int suPekerjaId
            )
        {
            AkTunaiPemegang line = collection1.FirstOrDefault(p => p.SuPekerjaId == suPekerjaId)!;

            if (line == null)
            {
                collection1.Add(new AkTunaiPemegang
                {
                    AkTunaiRuncitId = akTunaiRuncitId,
                    SuPekerjaId = suPekerjaId

                });
            }
        }

        public virtual void RemoveItem1(int id) =>
            collection1.RemoveAll(l => l.SuPekerjaId == id);


        public virtual void Clear1() => collection1.Clear();

        public virtual IEnumerable<AkTunaiPemegang> Lines1 => collection1;
        // Tunai Pemegang End
    }
}