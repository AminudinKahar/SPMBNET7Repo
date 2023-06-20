using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.EFRepository._02_Akaun
{
    public class AkPenyataPemungut2Repository : ListViewIRepository<AkPenyataPemungut2, int>
    {
        public readonly ApplicationDbContext context;

        public AkPenyataPemungut2Repository(ApplicationDbContext context)
        {
            this.context=context;
        }
        public async Task Delete(int id)
        {
            var model = await context.AkPenyataPemungut2.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkPenyataPemungut2>> GetAll(int akPenyataPemungutId)
        {
            return await context.AkPenyataPemungut2
                .Include(b => b.AkTerima2)
                    .ThenInclude(b => b!.AkTerima)
                .Where(x => x.AkPenyataPemungutId == akPenyataPemungutId)
                .ToArrayAsync();
        }

        public async Task<AkPenyataPemungut2> GetBy2Id(int akPenyataPemungutId, int indek)
        {
            return await context.AkPenyataPemungut2.FirstOrDefaultAsync(x => x.AkPenyataPemungutId == akPenyataPemungutId && x.Indek == indek) ?? new AkPenyataPemungut2();
        }

        public async Task<AkPenyataPemungut2> GetById(int id)
        {
            return await context.AkPenyataPemungut2.FindAsync(id) ?? new AkPenyataPemungut2();
        }

        public async Task<AkPenyataPemungut2> Insert(AkPenyataPemungut2 entity)
        {
            await context.AkPenyataPemungut2.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkPenyataPemungut2 entity)
        {
            AkPenyataPemungut2 data = context.AkPenyataPemungut2.FirstOrDefault(x => x.Id == entity.Id);
            await context.SaveChangesAsync();
        }
    }
}
