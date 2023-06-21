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
    public class SessionCartJurnal : CartJurnal
    {
        public static CartJurnal GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext!.Session!;
            SessionCartJurnal cart = session?.GetJson<SessionCartJurnal>("CartJurnal") ?? new SessionCartJurnal();
            
                cart.Session = session;
            return cart;
        }
        private ISession? Session { get; set; }

        public override void AddItem1(
            int AkJurnalId,
            int Indeks,
            int JBahagianDebitId,
            JBahagian? jBahagianDebit,
            int AkCartaDebitId,
            AkCarta? akCartaDebit,
            int JBahagianKreditId,
            JBahagian? jBahagianKredit,
            int AkCartaKreditId,
            AkCarta? akCartaKredit,
            decimal Amaun
           )
        {
            base.AddItem1(AkJurnalId,
                          Indeks,
                          JBahagianDebitId,
                          jBahagianDebit,
                          AkCartaDebitId,
                          akCartaDebit,
                          JBahagianKreditId,
                          jBahagianKredit,
                          AkCartaKreditId,
                          akCartaKredit,
                          Amaun);
            Session?.SetJson("CartJurnal", this);
        }
        public override void RemoveItem1(int jBahagianDebitId, int akCartaDebitId, int JBahagianKreditId, int akCartaKreditId, int indeksLama)
        {
            base.RemoveItem1(jBahagianDebitId, akCartaDebitId, JBahagianKreditId, akCartaKreditId, indeksLama);
            Session?.SetJson("CartJurnal", this);
        }
        public override void Clear1()
        {
            base.Clear1();
            Session?.Remove("CartJurnal");
        }
    }
}
