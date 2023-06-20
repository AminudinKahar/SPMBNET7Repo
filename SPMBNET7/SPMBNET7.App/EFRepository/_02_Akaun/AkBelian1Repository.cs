using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.EFRepository._02_Akaun
{
    public class AkBelian1Repository : ListViewIRepository<AkBelian1, int>
    {
        public readonly ApplicationDbContext context;

        public AkBelian1Repository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AkBelian1.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkBelian1>> GetAll(int akBelianId)
        {
            return await context.AkBelian1.Include(b => b.AkCarta)
                .Where(x => x.AkBelianId == akBelianId)
                .ToArrayAsync();
        }

        public async Task<AkBelian1> GetBy2Id(int akBelianId, int akCartaId)
        {
            return await context.AkBelian1.FirstOrDefaultAsync(x => x.AkBelianId == akBelianId && x.AkCartaId == akCartaId) ?? new AkBelian1();
        }

        public async Task<AkBelian1> GetById(int id)
        {
            return await context.AkBelian1.Include(d => d.AkCarta).Where(d => d.Id == id).FirstOrDefaultAsync() ?? new AkBelian1();
        }

        public async Task<AkBelian1> Insert(AkBelian1 entity)
        {
            await context.AkBelian1.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkBelian1 entity)
        {

            AkBelian1 data = context.AkBelian1.FirstOrDefault(x => x.Id == entity.Id);
            data!.Amaun = entity.Amaun;
            await context.SaveChangesAsync();
        }
    }
}
