using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SPMBNET7.CoreBusiness._Enums;
using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.CoreBusiness.Models.Modules._03_Sumber;
using SPMBNET7.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMBNET7.Infrastructure.Carts._02_Akaun.Sessions
{
    public class SessionCartCimbEFT : CartCimbEFT
    {
        public static CartCimbEFT GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext!.Session!;
            SessionCartCimbEFT cart = session?.GetJson<SessionCartCimbEFT>("CartCimbEFT") ?? new SessionCartCimbEFT();
            cart.Session = session;
            return cart;
        }
        private ISession? Session { get; set; }

        //CimbEFT1
        public override void AddItem1(
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
            base.AddItem1(akCimbEFTId,
                    Indek,
                    akPVId,
                    flPenerimaEFT,
                    akPembekalId,
                    suPekerjaId,
                    suAtletId,
                    suJurulatihId,
                    amaun,
                    noCek,
                    catatan,
                    jBankId,
                    flStatus);

            Session?.SetJson("CartCimbEFT", this);
        }
        public override void RemoveItem1(int id)
        {
            base.RemoveItem1(id);
            Session?.SetJson("CartCimbEFT", this);
        }
        public override void Clear1()
        {
            base.Clear1();
            Session?.Remove("CartCimbEFT");
        }
        //CimbEFT1 End
    }
}
