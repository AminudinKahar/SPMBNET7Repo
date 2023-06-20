using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SPMBNET7.Sumber.Data;
using SPMBNET7.CoreBusiness.Models.Administrations;
using SPMBNET7.Infrastructure.Interfaces;
using System.Security.Claims;

namespace SPMBNET7.Sumber.EFRepository
{
    public class AppLogRepository : AppLogIRepository<AppLog, int>
    {
        public readonly ApplicationDbContext context;
        public readonly UserManager<IdentityUser> userManager;
        public AppLogRepository(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<IEnumerable<AppLog>> GetAll()
        {
            return await context.AppLog.ToListAsync();
        }

        public async void Insert(string operasi,
                                 string nota,
                                 string rujukan,
                                 int idRujukan,
                                 decimal jumlah,
                                 int? pekerjaId,
                                 string modul,
                                 string syscode,
                                 string namamodul,
                                 IdentityUser? user)
        {

                AppLog entity = new AppLog();

                entity.IdRujukan = idRujukan;
                entity.UserId = user?.UserName ?? "";
                entity.NoRujukan = rujukan;
                entity.LgNote = namamodul + " - " + nota;
                entity.Jumlah = jumlah;
                entity.SuPekerjaId = pekerjaId;
                entity.SysCode = syscode;
                entity.LgDate = DateTime.Now;

                if (operasi == "Tambah")
                {
                    entity.LgModule = modul + "C";
                    entity.LgOperation = "Tambah";
                }
                else if (operasi == "Hapus")
                {
                    entity.LgModule = modul + "D";
                    entity.LgOperation = "Hapus";
                }
                else if (operasi == "Ubah")
                {
                    entity.LgModule = modul + "E";
                    entity.LgOperation = "Ubah";
                }
                else if (operasi == "Posting")
                {
                    entity.LgModule = modul + "T";
                    entity.LgOperation = "Posting";
                }
                else if (operasi == "UnPosting")
                {
                    entity.LgModule = modul + "UT";
                    entity.LgOperation = "UnPosting";
                }
                else if (operasi == "Cetak")
                {
                    entity.LgModule = modul + "P";
                    entity.LgOperation = "Cetak";
                }
                else if (operasi == "Batal")
                {
                    entity.LgModule = modul + "B";
                    entity.LgOperation = "Batal";
                }
                else if (operasi == "Rollback")
                {
                    entity.LgModule = modul + "R";
                    entity.LgOperation = "Rollback";
                }
                else if (operasi == "Rekup")
                {
                    entity.LgModule = modul + "T";
                    entity.LgOperation = "Rekup";
                }
                await context.AppLog.AddAsync(entity);

        }
    }
}
