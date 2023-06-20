﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace SPMBNET7.CoreBusiness.Models.Modules._02_Akaun
{
    public class AkPOLaras1
    {
        //field
        public int Id { get; set; }
        public int AkPOLarasId { get; set; }
        [DisplayName("Kod Objek")]
        public int AkCartaId { get; set; }
        [DisplayName("Amaun RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amaun { get; set; }
        //field end

        //Relationship
        public AkCarta? AkCarta { get; set; } 
        //relationship end
    }
}