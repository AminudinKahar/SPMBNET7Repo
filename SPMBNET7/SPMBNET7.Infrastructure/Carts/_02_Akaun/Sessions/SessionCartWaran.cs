﻿using Microsoft.AspNetCore.Http;
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
    public class SessionCartWaran : CartWaran
    {
        public static CartWaran GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext!.Session!;
            SessionCartWaran cart = session?.GetJson<SessionCartWaran>("CartWaran") ?? new SessionCartWaran();
            cart.Session = session;
            return cart;
        }
        private ISession? Session { get; set; }

        //Belian1
        public override void AddItem1(
            int abWaranId,
            decimal amaun,
            int akCartaId,
            int? jBahagianId,
            string tk
           )
        {
            base.AddItem1(abWaranId,
                          amaun,
                          akCartaId,
                          jBahagianId,
                          tk);

            Session?.SetJson("CartWaran", this);
        }

        public override void RemoveItem1(int id, int id2)
        {
            base.RemoveItem1(id, id2);
            Session?.SetJson("CartWaran", this);
        }
        public override void Clear1()
        {
            base.Clear1();
            Session?.Remove("CartWaran");
        }
        //Belian1 End
    }
}
