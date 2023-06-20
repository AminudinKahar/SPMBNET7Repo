using SPMBNET7.CoreBusiness.Models.Bases.BaseInterfaces;
using SPMBNET7.CoreBusiness.Models.Bases.Helper;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.CoreBusiness.Models.Modules._03_Sumber;
using SPMBNET7.CoreBusiness.Models.Modules._04_Aset;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMBNET7.CoreBusiness.Models.Modules._01_Jadual
{
    public class JKW : AppLogHelper, ISoftDelete
    {
        //field
        public int Id { get; set; }
        [Required(ErrorMessage = "Kod Diperlukan")]
        [MaxLength(3)]
        public string Kod { get; set; } = string.Empty;
        [Required(ErrorMessage = "Perihal Diperlukan")]
        [MaxLength(100)]
        public string Perihal { get; set; } = string.Empty;
        //field end


        //Relationship
        public ICollection<AkBank> AkBank { get; set; } = new List<AkBank>();
        public ICollection<AkCarta> AkCarta { get; set; } = new List<AkCarta>();
        public ICollection<AkTerima> AkTerima { get; set; } = new List<AkTerima>();
        public ICollection<AkAkaun> AkAkaun { get; set; } = new List<AkAkaun>();
        public ICollection<AkPO> AkPO { get; set; } = new List<AkPO>();
        public ICollection<AkInden> AkInden { get; set; } = new List<AkInden>();
        public ICollection<AkPOLaras> AkPOLaras { get; set; } = new List<AkPOLaras>();
        public ICollection<AkJurnal> AkJurnal { get; set; } = new List<AkJurnal>();
        public ICollection<AkBelian> AkBelian { get; set; } = new List<AkBelian>();
        public ICollection<AkPV> AkPV { get; set; } = new List<AkPV>();
        public ICollection<AbBukuVot> AbBukuVot { get; set; } = new List<AbBukuVot>();
        public ICollection<AkTunaiRuncit> AkTunaiRuncit { get; set; } = new List<AkTunaiRuncit>();
        public ICollection<AkNotaMinta> AkNotaMinta { get; set; } = new List<AkNotaMinta>();
        public ICollection<AbWaran> AbWaran { get; set; } = new List<AbWaran>();
        public ICollection<SuProfil> SuProfil { get; set; } = new List<SuProfil>();
        public ICollection<JPTJ> JPTJ { get; set; } = new List<JPTJ>();
        public ICollection<AkInvois> AkInvois { get; set; } = new List<AkInvois>();
        public ICollection<AkTunaiLejar> AkTunaiLejar { get; set; } = new List<AkTunaiLejar>();
        public ICollection<AsAset> AsAset { get; set; } = new List<AsAset>();
        //relationship end

        //soft delete
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        public string? SebabHapus { get; set; }
        //soft delete end
    }
}
