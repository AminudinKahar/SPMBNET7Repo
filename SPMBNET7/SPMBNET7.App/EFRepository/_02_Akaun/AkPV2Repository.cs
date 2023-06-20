using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.EFRepository._02_Akaun
{
    public class AkPV2Repository : ListViewIRepository<AkPV2, int>
    {
        public readonly ApplicationDbContext context;

        public AkPV2Repository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AkPV2.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkPV2>> GetAll(int akPVId)
        {
            return await context.AkPV2
                .Include(b => b.AkBelian).ThenInclude(b => b!.AkPO)
                .Where(x => x.AkPVId == akPVId)
                .ToListAsync();
        }

        public async Task<AkPV2> GetBy2Id(int akPVId, int akBelianId)
        {
            return await context.AkPV2.FirstOrDefaultAsync(x => x.AkPVId == akPVId && x.AkBelianId == akBelianId) ?? new AkPV2();
        }

        public async Task<AkPV2> GetById(int id)
        {
            return await context.AkPV2.Include(b => b.AkBelian).ThenInclude(b => b!.AkPO).Where(x => x.Id == id).FirstOrDefaultAsync() ?? new AkPV2();
        }

        public async Task<AkPV2> Insert(AkPV2 entity)
        {
            await context.AkPV2.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkPV2 entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
