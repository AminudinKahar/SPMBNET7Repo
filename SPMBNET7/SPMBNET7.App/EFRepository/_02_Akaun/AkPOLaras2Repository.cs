using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.EFRepository._02_Akaun
{
    public class AkPOLaras2Repository : ListViewIRepository<AkPOLaras2, int>
    {
        public readonly ApplicationDbContext context;

        public AkPOLaras2Repository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AkPOLaras2.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkPOLaras2>> GetAll(int akPOLarasId)
        {
            return await context.AkPOLaras2
                .Where(x => x.AkPOLarasId == akPOLarasId)
                .ToListAsync();
        }

        public Task<AkPOLaras2> GetBy2Id(int akPOLarasId, int id2)
        {
            throw new NotImplementedException();
        }

        public async Task<AkPOLaras2> GetById(int id)
        {
            return await context.AkPOLaras2.FindAsync(id) ?? new AkPOLaras2();
        }

        public async Task<AkPOLaras2> Insert(AkPOLaras2 entity)
        {
            await context.AkPOLaras2.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkPOLaras2 entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
