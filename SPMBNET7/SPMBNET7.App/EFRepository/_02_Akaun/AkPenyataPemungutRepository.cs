using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.EFRepository._02_Akaun
{
    public class AkPenyataPemungutRepository : IRepository<AkPenyataPemungut, int, string>
    {
        public readonly ApplicationDbContext context;
        public AkPenyataPemungutRepository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AkPenyataPemungut.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkPenyataPemungut>> GetAll()
        {
            return await context.AkPenyataPemungut
                .Include(b => b.SuPekerja)
                .Include(b => b.JCaraBayar)
                .Include(b => b.AkBank)
                    .ThenInclude(b => b!.JBank)
                .Include(b => b.AkBank)
                    .ThenInclude(b => b!.AkCarta)
                .Include(b => b.AkPenyataPemungut1)
                    .ThenInclude(b => b.AkCarta)
                .Include(b => b.AkPenyataPemungut1)
                    .ThenInclude(b => b.JBahagian)
                        .ThenInclude(b => b!.JKW)
                .Include(b => b.AkPenyataPemungut2)
                    .ThenInclude(b => b.AkTerima2)
                        .ThenInclude(b => b!.AkTerima)
                .ToListAsync();
        }

        public async Task<IEnumerable<AkPenyataPemungut>> GetAllIncludeDeletedItems()
        {
            return await context.AkPenyataPemungut
                .IgnoreQueryFilters()
                .Include(b => b.SuPekerja)
                .Include(b => b.JCaraBayar)
                .Include(b => b.AkBank)
                    .ThenInclude(b => b!.JBank)
                .Include(b => b.AkBank)
                    .ThenInclude(b => b!.AkCarta)
                .Include(b => b.AkPenyataPemungut1)
                    .ThenInclude(b => b.AkCarta)
                .Include(b => b.AkPenyataPemungut1)
                    .ThenInclude(b => b.JBahagian)
                        .ThenInclude(b => b!.JKW)
                .Include(b => b.AkPenyataPemungut2)
                    .ThenInclude(b => b.AkTerima2)
                        .ThenInclude(b => b!.AkTerima)
                .ToListAsync();
        }

        public async Task<AkPenyataPemungut> GetById(int id)
        {
            return await context.AkPenyataPemungut
                .Include(b => b.SuPekerja)
                .Include(b => b.JCaraBayar)
                .Include(b => b.AkBank)
                    .ThenInclude(b => b!.JBank)
                .Include(b => b.AkBank)
                    .ThenInclude(b => b!.AkCarta)
                .Include(b => b.AkPenyataPemungut1)
                    .ThenInclude(b => b.AkCarta)
                .Include(b => b.AkPenyataPemungut1)
                    .ThenInclude(b => b.JBahagian)
                        .ThenInclude(b => b!.JKW)
                .Include(b => b.AkPenyataPemungut2)
                    .ThenInclude(b => b.AkTerima2)
                        .ThenInclude(b => b!.AkTerima)
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync() ?? new AkPenyataPemungut();
        }

        public async Task<AkPenyataPemungut> GetByIdIncludeDeletedItems(int id)
        {
            return await context.AkPenyataPemungut
                .IgnoreQueryFilters()
                .Include(b => b.SuPekerja)
                .Include(b => b.JCaraBayar)
                .Include(b => b.AkBank)
                    .ThenInclude(b => b!.JBank)
                .Include(b => b.AkBank)
                    .ThenInclude(b => b!.AkCarta)
                .Include(b => b.AkPenyataPemungut1)
                    .ThenInclude(b => b.AkCarta)
                .Include(b => b.AkPenyataPemungut1)
                    .ThenInclude(b => b.JBahagian)
                        .ThenInclude(b => b!.JKW)
                .Include(b => b.AkPenyataPemungut2)
                    .ThenInclude(b => b.AkTerima2)
                        .ThenInclude(b => b!.AkTerima)
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync() ?? new AkPenyataPemungut();
        }

        public Task<AkPenyataPemungut> GetByString(string id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<AkPenyataPemungut> Insert(AkPenyataPemungut entity)
        {
            await context.AkPenyataPemungut.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkPenyataPemungut entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
