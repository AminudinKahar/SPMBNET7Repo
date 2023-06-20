using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMBNET7.CoreBusiness.Models.Bases.BaseInterfaces
{
    public interface IRecon
    {
        // bank recon flag

        public int FlTunai { get; set; }
        public DateTime? TarTunai { get; set; }
        // bank recon flag end
    }
}
