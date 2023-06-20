using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;

namespace SPMBNET7.Infrastructure.Carts._02_Akaun
{
    public class CartTunaiCV
    {
        //TunaiCV1

        private List<AkTunaiCV1> collection1 = new List<AkTunaiCV1>();

        public virtual void AddItem1(
            int akTunaiCVId,
            decimal amaun,
            int akCartaId
            )
        {
            AkTunaiCV1 line = collection1.FirstOrDefault(p => p.AkCartaId == akCartaId)!;

            if (line == null)
            {
                collection1.Add(new AkTunaiCV1
                {
                    AkTunaiCVId = akTunaiCVId,
                    Amaun = amaun,
                    AkCartaId = akCartaId
                });
            }
        }

        public virtual void RemoveItem1(int id) =>
            collection1.RemoveAll(l => l.AkCartaId == id);


        public virtual void Clear1() => collection1.Clear();

        public virtual IEnumerable<AkTunaiCV1> Lines1 => collection1;
        // TunaiCV1 End
    }
}