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
    public class SessionCartBankRecon : CartBankRecon
    {
        public static CartBankRecon GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext!.Session!;
            SessionCartBankRecon cart = session?.GetJson<SessionCartBankRecon>("CartBankRecon") ?? new SessionCartBankRecon();
            
                cart.Session = session;
            return cart;
        }
        private ISession? Session { get; set; }
        // BankRecon
        public override void AddItem1(
            int id,
            int indek,
            int akBankReconId,
            string noAkaunBank,
            DateTime tarikh,
            string kodTransaksi,
            string perihalTransaksi,
            string noDokumen,
            decimal debit,
            decimal kredit,
            decimal baki,
            bool isPadan
            )
        {
            base.AddItem1(id,
                indek,
                akBankReconId,
                noAkaunBank,
                tarikh,
                kodTransaksi,
                perihalTransaksi,
                noDokumen,
                debit,
                kredit,
                baki,
                isPadan);

            Session?.SetJson("CartBankRecon", this);
        }
        public override void RemoveItem1(int id)
        {
            base.RemoveItem1(id);
            Session?.SetJson("CartBankRecon", this);
        }
        public override void Clear1()
        {
            base.Clear1();
            Session?.Remove("CartBankRecon");
        }
        // BankRecon End
    }
}
