using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.CoreBusiness.Models.Modules._03_Sumber;
using SPMBNET7.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMBNET7.Infrastructure.Carts._02_Akaun.Sessions
{
    public class SessionCartJurulatih : CartJurulatih
    {
        public static CartJurulatih GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext!.Session!;
            SessionCartJurulatih cart = session?.GetJson<SessionCartJurulatih>("CartJurulatih") ?? new SessionCartJurulatih();
            
                cart.Session = session;
            return cart;
        }
        private ISession? Session { get; set; }

        //Atlet
        public override void AddItem1(
            int suProfilId,
            int? suJurulatihId,
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
            base.AddItem1(suProfilId,
            suJurulatihId,
            jSukanId,
            jCarabayarId,
            noCekEFT,
            tarCekEFT,
            amaun,
            amaunsebelum,
            tunggakan,
            catatan,
            jumlah
            );

            Session?.SetJson("CartJurulatih", this);
        }

        public override void RemoveItem1(int? id)
        {
            base.RemoveItem1(id);
            Session?.SetJson("CartJurulatih", this);
        }
        public override void Clear1()
        {
            base.Clear1();
            Session?.Remove("CartJurulatih");
        }
        //Atlet End
    }
}
