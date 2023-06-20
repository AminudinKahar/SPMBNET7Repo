using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMBNET7.Infrastructure.Carts._02_Akaun.Sessions
{
    public class SessionCartTunaiCV : CartTunaiCV
    {
        public static CartTunaiCV GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext!.Session!;
            SessionCartTunaiCV cart = session?.GetJson<SessionCartTunaiCV>("CartTunaiCV") ?? new SessionCartTunaiCV();
            cart.Session = session;
            return cart;
        }
        private ISession? Session { get; set; }

        //Terima1
        public override void AddItem1(
            int akTunaiCVId,
            decimal amaun,
            int akCartaId
           )
        {
            base.AddItem1(akTunaiCVId,
                          amaun,
                          akCartaId);

            Session?.SetJson("CartTunaiCV", this);
        }
        public override void RemoveItem1(int id)
        {
            base.RemoveItem1(id);
            Session?.SetJson("CartTunaiCV", this);
        }
        public override void Clear1()
        {
            base.Clear1();
            Session?.Remove("CartTunaiCV");
        }
        //Terima1 End
    }
}
