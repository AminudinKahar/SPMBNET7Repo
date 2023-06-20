using Microsoft.EntityFrameworkCore;
using SPMBNET7.Sumber.Data;
using SPMBNET7.CoreBusiness.Models.Modules._03_Sumber;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.Sumber.EFRepository._03_Sumber
{
    public class SuPekerjaRepository : IRepository<SuPekerja, int, string>
    {
        public readonly ApplicationDbContext context;
        public SuPekerjaRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var model = await context.SuPekerja.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<SuPekerja>> GetAll()
        {
            return await context.SuPekerja
                .Include(b => b.JAgama)
                .Include(b => b.JBangsa)
                .Include(b => b.JCaraBayar)
                .Include(b => b.JNegeri)
                .Include(b => b.SuTanggungan)
                .ToListAsync();
        }

        public async Task<IEnumerable<SuPekerja>> GetAllIncludeDeletedItems()
        {
            return await context.SuPekerja
                .IgnoreQueryFilters()
                .Include(b => b.JAgama)
                .Include(b => b.JBangsa)
                .Include(b => b.JCaraBayar)
                .Include(b => b.JNegeri)
                .Include(b => b.SuTanggungan)
                .ToListAsync();
        }

        public async Task<SuPekerja> GetById(int id)
        {
            return await context.SuPekerja.Include(b => b.JAgama)
                .Include(b => b.JBangsa)
                .Include(b => b.JCaraBayar)
                .Include(b => b.JNegeri)
                .Include(b => b.SuTanggungan)
                .FirstOrDefaultAsync(x => x.Id == id) ?? new SuPekerja();
        }

        public async Task<SuPekerja> GetByIdIncludeDeletedItems(int id)
        {
            return await context.SuPekerja.IgnoreQueryFilters()
                .Include(b => b.JAgama)
                .Include(b => b.JBangsa)
                .Include(b => b.JCaraBayar)
                .Include(b => b.JNegeri)
                .Include(b => b.SuTanggungan)
                .FirstOrDefaultAsync(x => x.Id == id) ?? new SuPekerja();
        }

        public async Task<SuPekerja> GetByString(string noKP)
        {
            return await context.SuPekerja.Where(x => x.NoKp == noKP).FirstOrDefaultAsync() ?? new SuPekerja();
        }

        public async Task<SuPekerja> Insert(SuPekerja entity)
        {
            await context.SuPekerja.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(SuPekerja entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
