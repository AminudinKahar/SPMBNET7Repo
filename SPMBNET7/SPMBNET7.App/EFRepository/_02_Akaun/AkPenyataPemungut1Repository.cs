using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.EFRepository._02_Akaun
{
    public class AkPenyataPemungut1Repository : ListViewIRepository<AkPenyataPemungut1, int>
    {
        public readonly ApplicationDbContext context;

        public AkPenyataPemungut1Repository(ApplicationDbContext context)
        {
            this.context=context;
        }

        public async Task Delete(int id)
        {
            var model = await context.AkPenyataPemungut1.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkPenyataPemungut1>> GetAll(int akPenyataPemungutId)
        {
            return await context.AkPenyataPemungut1
                .Include(b => b.JBahagian)
                    .ThenInclude(b => b!.JKW)
                .Include(b => b.AkCarta)
                .Where(x => x.AkPenyataPemungutId == akPenyataPemungutId)
                .ToArrayAsync();
        }

        public async Task<AkPenyataPemungut1> GetBy2Id(int akPenyataPemungutId, int indek)
        {
            return await context.AkPenyataPemungut1.FirstOrDefaultAsync(x => x.AkPenyataPemungutId == akPenyataPemungutId && x.Indek == indek) ?? new AkPenyataPemungut1();
        }

        public async Task<AkPenyataPemungut1> GetById(int id)
        {
            return await context.AkPenyataPemungut1.FindAsync(id) ?? new AkPenyataPemungut1();
        }

        public async Task<AkPenyataPemungut1> Insert(AkPenyataPemungut1 entity)
        {
            await context.AkPenyataPemungut1.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkPenyataPemungut1 entity)
        {
            AkPenyataPemungut1 data = context.AkPenyataPemungut1.FirstOrDefault(x => x.Id == entity.Id);
            await context.SaveChangesAsync();
        }
    }
}
