using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Modules._03_Sumber;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.EFRepository._03_Sumber
{
    public class SuAtletRepository : IRepository<SuAtlet, int, string>
    {
        public readonly ApplicationDbContext context;
        public SuAtletRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var model = await context.SuAtlet.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<SuAtlet>> GetAll()
        {
            return await context.SuAtlet
                .Include(b => b.JSukan)
                .Include(b => b.JAgama)
                .Include(b => b.JBangsa)
                .Include(b => b.JCaraBayar)
                .Include(b => b.JNegeri)
                //.Include(b => b.SuTanggungan)
                .ToListAsync();
        }

        public async Task<IEnumerable<SuAtlet>> GetAllIncludeDeletedItems()
        {
            return await context.SuAtlet
                .IgnoreQueryFilters()
                .Include(b => b.JSukan)
                .Include(b => b.JAgama)
                .Include(b => b.JBangsa)
                .Include(b => b.JCaraBayar)
                .Include(b => b.JNegeri)
                //.Include(b => b.SuTanggungan)
                .ToListAsync();
        }

        public async Task<SuAtlet> GetById(int id)
        {
            return await context.SuAtlet
                .Include(b => b.JSukan)
                .Include(b => b.JAgama)
                .Include(b => b.JBangsa)
                .Include(b => b.JCaraBayar)
                .Include(b => b.JNegeri)
                //.Include(b => b.SuTanggungan)
                .FirstOrDefaultAsync(x => x.Id == id) ?? new SuAtlet();
        }

        public async Task<SuAtlet> GetByIdIncludeDeletedItems(int id)
        {
            return await context.SuAtlet.IgnoreQueryFilters()
                .Include(b => b.JSukan)
                .Include(b => b.JAgama)
                .Include(b => b.JBangsa)
                .Include(b => b.JCaraBayar)
                .Include(b => b.JNegeri)
                .FirstOrDefaultAsync(x => x.Id == id) ?? new SuAtlet();
        }

        public async Task<SuAtlet> GetByString(string noKP)
        {
            return await context.SuAtlet.Where(x => x.NoKp == noKP).FirstOrDefaultAsync() ?? new SuAtlet();
        }

        public async Task<SuAtlet> Insert(SuAtlet entity)
        {
            await context.SuAtlet.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(SuAtlet entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
