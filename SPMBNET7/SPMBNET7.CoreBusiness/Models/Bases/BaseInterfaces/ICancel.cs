using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMBNET7.CoreBusiness.Models.Bases.BaseInterfaces
{
    public interface ICancel
    {
        //Soft Delete

        public int FlBatal { get; set; }
        public DateTime? TarBatal { get; set; }
        //Soft Delete end
    }
}
