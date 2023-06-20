using Microsoft.EntityFrameworkCore;
using SPMBNET7.Sumber.Data;
using SPMBNET7.CoreBusiness.Models.Modules._03_Sumber;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.Sumber.EFRepository._03_Sumber
{
    public class SuProfilAtletRepository : IRepository<SuProfil, int, string>
    {
        public readonly ApplicationDbContext context;

        public SuProfilAtletRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var model = await context.SuProfil.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<SuProfil>> GetAll()
        {
            return await context.SuProfil.Where(x => x.FlKategori == 0).ToListAsync();
        }

        public async Task<IEnumerable<SuProfil>> GetAllIncludeDeletedItems()
        {
            return await context.SuProfil
                .Include(b => b.JKW)
                .Include(b => b.AkCarta)
                .Include(b => b.JBahagian)
                .Include(b => b.SuProfil1).ThenInclude(b => b.SuAtlet).ThenInclude(b => b.JBank)
                .Include(b => b.SuProfil1).ThenInclude(b => b.JSukan)
                .Include(b => b.SuProfil1).ThenInclude(b => b.JCaraBayar)
                .Where(x => x.FlKategori == 0)
                .IgnoreQueryFilters()
                .ToListAsync();
        }

        public async Task<SuProfil> GetById(int id)
        {
            return await context.SuProfil
                .Include(b => b.JKW)
                .Include(b => b.AkCarta)
                .Include(b => b.JBahagian)
                .Include(b => b.SuProfil1).ThenInclude(b => b.SuAtlet).ThenInclude(b => b.JBank)
                .Include(b => b.SuProfil1).ThenInclude(b => b.JSukan)
                .Include(b => b.SuProfil1).ThenInclude(b => b.JCaraBayar)
                .Where(x => x.FlKategori == 0)
                .FirstOrDefaultAsync(x => x.Id == id) ?? new SuProfil();

        }

        public async Task<SuProfil> GetByIdIncludeDeletedItems(int id)
        {
            return await context.SuProfil
                .Include(b => b.JKW)
                .Include(b => b.AkCarta)
                .Include(b => b.JBahagian)
                .Include(b => b.SuProfil1).ThenInclude(b => b.SuAtlet).ThenInclude(b => b.JBank)
                .Include(b => b.SuProfil1).ThenInclude(b => b.JSukan)
                .Include(b => b.SuProfil1).ThenInclude(b => b.JCaraBayar)
                .Where(x => x.FlKategori == 0)
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id) ?? new SuProfil();
        }

        public Task<SuProfil> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<SuProfil> Insert(SuProfil entity)
        {
            await context.SuProfil.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(SuProfil entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
