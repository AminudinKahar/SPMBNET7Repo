﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMBNET7.CoreBusiness.Models.Modules._02_Akaun.ViewModels
{
    public class AkJurnal1ViewModel
    {
        public int AkJurnalId { get; set; }
        public int IndeksLama { get; set; }
        public int IndeksBaru { get; set; }
        public int JBahagianDebitId { get; set; }
        public int AkCartaDebitId { get; set; }
        public int JBahagianKreditId { get; set; }
        public int AkCartaKreditId { get; set; }
        public decimal Amaun { get; set; }
        public decimal Debit { get; set; }
        public decimal Kredit { get; set; }
    }
}
