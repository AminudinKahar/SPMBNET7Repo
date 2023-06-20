using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Modules._03_Sumber;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.EFRepository._03_Sumber
{
    public class SuProfilJurulatihRepository : IRepository<SuProfil, int, string>
    {
        public readonly ApplicationDbContext context;

        public SuProfilJurulatihRepository(ApplicationDbContext context) => this.context = context;
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
            return await context.SuProfil.Where(x => x.FlKategori == 1).ToListAsync();
        }

        public async Task<IEnumerable<SuProfil>> GetAllIncludeDeletedItems()
        {
            return await context.SuProfil
                .Include(b => b.JKW)
                .Include(b => b.AkCarta)
                .Include(b => b.JBahagian)
                .Include(b => b.SuProfil1).ThenInclude(b => b.SuJurulatih).ThenInclude(b => b!.JBank)
                .Include(b => b.SuProfil1).ThenInclude(b => b.JSukan)
                .Include(b => b.SuProfil1).ThenInclude(b => b.JCaraBayar)
                .Where(x => x.FlKategori == 1)
                .IgnoreQueryFilters()
                .ToListAsync();
        }

        public async Task<SuProfil> GetById(int id)
        {
            return await context.SuProfil
                .Include(b => b.JKW)
                .Include(b => b.AkCarta)
                .Include(b => b.JBahagian)
                .Include(b => b.SuProfil1).ThenInclude(b => b.SuJurulatih).ThenInclude(b => b!.JBank)
                .Include(b => b.SuProfil1).ThenInclude(b => b.JSukan)
                .Include(b => b.SuProfil1).ThenInclude(b => b.JCaraBayar)
                .Where(x => x.FlKategori == 1)
                .FirstOrDefaultAsync(x => x.Id == id) ?? new SuProfil();
        }

        public async Task<SuProfil> GetByIdIncludeDeletedItems(int id)
        {
            return await context.SuProfil
                .Include(b => b.JKW)
                .Include(b => b.AkCarta)
                .Include(b => b.JBahagian)
                .Include(b => b.SuProfil1).ThenInclude(b => b.SuJurulatih).ThenInclude(b => b!.JBank)
                .Include(b => b.SuProfil1).ThenInclude(b => b.JSukan)
                .Include(b => b.SuProfil1).ThenInclude(b => b.JCaraBayar)
                .Where(x => x.FlKategori == 1)
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id) ?? new SuProfil();
        }

        public Task<SuProfil> GetByString(string id)
        {
            throw new System.NotImplementedException();
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
