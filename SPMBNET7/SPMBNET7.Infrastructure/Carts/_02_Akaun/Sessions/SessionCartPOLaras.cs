using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.CoreBusiness.Models.Modules._04_Aset;
using SPMBNET7.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMBNET7.Infrastructure.Carts._02_Akaun.Sessions
{
    public class SessionCartPOLaras : CartPOLaras
    {
        public static CartPOLaras GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext!.Session!;
            SessionCartPOLaras cart = session?.GetJson<SessionCartPOLaras>("CartPOLaras") ?? new SessionCartPOLaras();
            cart.Session = session;
            return cart;
        }
        private ISession? Session { get; set; }

        //POLaras1
        public override void AddItem1(
                int AkPOLarasId,
                int akCartaId,
                decimal Amaun
            )
        {
            base.AddItem1(AkPOLarasId,
                          akCartaId,
                          Amaun
                          );

            Session?.SetJson("CartPOLaras", this);
        }
        public override void RemoveItem1(int id)
        {
            base.RemoveItem1(id);
            Session?.SetJson("CartPOLaras", this);
        }
        public override void Clear1()
        {
            base.Clear1();
            Session?.Remove("CartPOLaras");
        }
        //POLaras1 End

        //POLaras2
        public override void AddItem2(
            int akPOLarasId,
            int Indek,
            decimal Bil,
            string NoStok,
            string Perihal,
            decimal Kuantiti,
            string Unit,
            decimal Harga,
            decimal Amaun
            )
        {
            base.AddItem2(akPOLarasId,
                    Indek,
                    Bil,
                    NoStok,
                    Perihal,
                    Kuantiti,
                    Unit,
                    Harga,
                    Amaun);

            Session?.SetJson("CartPOLaras", this);
        }
        public override void RemoveItem2(int id)
        {
            base.RemoveItem2(id);
            Session?.SetJson("CartPOLaras", this);
        }
        public override void Clear2()
        {
            base.Clear2();
            Session?.Remove("CartPOLaras");
        }
        //POLaras2 End
    }
}
