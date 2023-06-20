using Microsoft.EntityFrameworkCore;
using SPMBNET7.Sumber.Data;
using SPMBNET7.CoreBusiness.Models.Modules._03_Sumber;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.Sumber.EFRepository._03_Sumber
{
    public class SuJurulatihRepository : IRepository<SuJurulatih, int, string>
    {
        public readonly ApplicationDbContext context;
        public SuJurulatihRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var model = await context.SuJurulatih.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<SuJurulatih>> GetAll()
        {
            return await context.SuJurulatih
                .Include(b => b.JSukan)
                .Include(b => b.JAgama)
                .Include(b => b.JBangsa)
                .Include(b => b.JCaraBayar)
                .Include(b => b.JNegeri)
                .OrderBy(b => b.KodJurulatih)
                //.Include(b => b.SuTanggungan)
                .ToListAsync();
        }

        public async Task<IEnumerable<SuJurulatih>> GetAllIncludeDeletedItems()
        {
            return await context.SuJurulatih
                .IgnoreQueryFilters()
                .Include(b => b.JSukan)
                .Include(b => b.JAgama)
                .Include(b => b.JBangsa)
                .Include(b => b.JCaraBayar)
                .Include(b => b.JNegeri)
                .OrderBy(b => b.KodJurulatih)
                //.Include(b => b.SuTanggungan)
                .ToListAsync();
        }

        public async Task<SuJurulatih> GetById(int id)
        {
            return await context.SuJurulatih.Include(b => b.JSukan)
                .Include(b => b.JAgama)
                .Include(b => b.JBangsa)
                .Include(b => b.JCaraBayar)
                .Include(b => b.JNegeri)
                .FirstOrDefaultAsync(x => x.Id == id) ?? new SuJurulatih();
        }

        public async Task<SuJurulatih> GetByIdIncludeDeletedItems(int id)
        {
            return await context.SuJurulatih
                .IgnoreQueryFilters()
                .Include(b => b.JSukan)
                .Include(b => b.JAgama)
                .Include(b => b.JBangsa)
                .Include(b => b.JCaraBayar)
                .Include(b => b.JNegeri)
                //.Include(b => b.SuTanggungan)
                .FirstOrDefaultAsync(x => x.Id == id) ?? new SuJurulatih();
        }

        public async Task<SuJurulatih> GetByString(string noKP)
        {
            return await context.SuJurulatih.Where(x => x.NoKp == noKP).FirstOrDefaultAsync() ?? new SuJurulatih();
        }

        public async Task<SuJurulatih> Insert(SuJurulatih entity)
        {
            await context.SuJurulatih.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(SuJurulatih entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
