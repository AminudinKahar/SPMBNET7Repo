using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMBNET7.Infrastructure.Carts._02_Akaun.Sessions
{
    public class SessionCartPenyataPemungut : CartPenyataPemungut
    {
        public static CartPenyataPemungut GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext!.Session!;
            SessionCartPenyataPemungut cart = session?.GetJson<SessionCartPenyataPemungut>("CartPenyataPemungut") ?? new SessionCartPenyataPemungut();
            cart.Session = session;
            return cart;
        }

        private ISession? Session { get; set; }

        // PenyataPemungut1
        public override void AddItem1(
            int akPenyataPemungutId,
            int indek,
            int jBahagianId,
            int akCartaId,
            decimal amaun
            )
        {
            base.AddItem1(
                akPenyataPemungutId,
                indek,
                jBahagianId,
                akCartaId,
                amaun
                );

            Session?.SetJson("CartPenyataPemungut", this);
        }

        public override void RemoveItem1(int id)
        {
            base.RemoveItem1(id);
            Session?.SetJson("CartPenyataPemungut", this);
        }

        public override void Clear1()
        {
            base.Clear1();
            Session?.Remove("CartPenyataPemungut");
        }

        // PenyataPemungut2
        public override void AddItem2(
            int akPenyataPemungutId,
            int indek,
            int akTerima2Id,
            decimal amaun
            )
        {
            base.AddItem2(
                akPenyataPemungutId,
                indek,
                akTerima2Id,
                amaun
                );

            Session?.SetJson("CartPenyataPemungut", this);
        }

        public override void RemoveItem2(int id)
        {
            base.RemoveItem2(id);
            Session?.SetJson("CartPenyataPemungut", this);
        }

        public override void Clear2()
        {
            base.Clear2();
            Session?.Remove("CartPenyataPemungut");
        }
    }
}
