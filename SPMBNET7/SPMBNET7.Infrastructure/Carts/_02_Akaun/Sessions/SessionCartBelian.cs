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
    public class SessionCartBelian : CartBelian
    {
        public static CartBelian GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext!.Session!;
            SessionCartBelian cart = session?.GetJson<SessionCartBelian>("CartBelian") ?? new SessionCartBelian();
            
                cart.Session = session;
            return cart;
        }
        private ISession? Session { get; set; }

        //Belian1
        public override void AddItem1(
            int akBelianId,
            decimal amaun,
            int akCartaId
           )
        {
            base.AddItem1(akBelianId,
                          amaun,
                          akCartaId);

            Session?.SetJson("CartBelian", this);
        }

        public override void RemoveItem1(int id)
        {
            base.RemoveItem1(id);
            Session?.SetJson("CartBelian", this);
        }
        public override void Clear1()
        {
            base.Clear1();
            Session?.Remove("CartBelian");
        }
        //Belian1 End

        //Belian2
        public override void AddItem2(
            int akBelianId,
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
            base.AddItem2(akBelianId,
                    Indek,
                    Bil,
                    NoStok,
                    Perihal,
                    Kuantiti,
                    Unit,
                    Harga,
                    Amaun);

            Session?.SetJson("CartBelian", this);
        }
        public override void RemoveItem2(int id)
        {
            base.RemoveItem2(id);
            Session?.SetJson("CartBelian", this);
        }
        public override void Clear2()
        {
            base.Clear2();
            Session?.Remove("CartBelian");
        }
        //Belian2 End
    }
}
