using Microsoft.AspNetCore.Identity;
using SPMBNET7.CoreBusiness.Models.Administrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMBNET7.Infrastructure.Interfaces
{
    public interface AppLogIRepository<T1, T2> where T1 : class
    {
        Task<IEnumerable<T1>> GetAll();
        void Insert(string operasi,
                    string nota,
                    string rujukan,
                    int idRujukan,
                    decimal jumlah,
                    int? pekerjaId,
                    string modul,
                    string syscode,
                    string namamodul,
                    IdentityUser? user);
    }
}
