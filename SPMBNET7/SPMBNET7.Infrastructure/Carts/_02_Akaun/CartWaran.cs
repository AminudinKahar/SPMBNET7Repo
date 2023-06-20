using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;

namespace SPMBNET7.Infrastructure.Carts._02_Akaun
{
    public class CartWaran
    {
        //Waran 1

        private List<AbWaran1> collection1 = new List<AbWaran1>();

        public virtual void AddItem1(
            int abWaranId,
            decimal amaun,
            int akCartaId,
            int? jBahagianId,
            string tk
            )
        {
            AbWaran1 line = collection1.FirstOrDefault(p => p.AkCartaId == akCartaId && p.JBahagianId == jBahagianId)!;

            if (line == null)
            {
                collection1.Add(new AbWaran1
                {
                    AbWaranId = abWaranId,
                    Amaun = amaun,
                    AkCartaId = akCartaId,
                    JBahagianId = jBahagianId,
                    TK = tk
                });
            }
        }

        public virtual void RemoveItem1(int id, int id2) =>
            collection1.RemoveAll(l => l.AkCartaId == id && l.JBahagianId == id2);


        public virtual void Clear1() => collection1.Clear();

        public virtual IEnumerable<AbWaran1> Lines1 => collection1;
        // Waran 1 End
    }
}