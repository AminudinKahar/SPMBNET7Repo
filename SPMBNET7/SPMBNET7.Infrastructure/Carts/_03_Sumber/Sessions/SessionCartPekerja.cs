using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SPMBNET7.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMBNET7.Infrastructure.Carts._03_Sumber.Sessions
{
    public class SessionCartPekerja : CartPekerja
    {
        public static CartPekerja GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext!.Session!;
            SessionCartPekerja cart = session?.GetJson<SessionCartPekerja>("CartPekerja") ?? new SessionCartPekerja();
            cart.Session = session;
            return cart;
        }
        private ISession? Session { get; set; }

        public override void AddItem1(
            int SuPekerjaId,
            string Nama,
            string Hubungan,
            string NoKP
           )
        {
            base.AddItem1(SuPekerjaId, Nama, Hubungan, NoKP);
            Session?.SetJson("CartPekerja", this);
        }
        public override void RemoveItem1(string nokp)
        {
            base.RemoveItem1(nokp);
            Session?.SetJson("CartPekerja", this);
        }
        public override void Clear1()
        {
            base.Clear1();
            Session?.Remove("CartPekerja");
        }
    }
}
