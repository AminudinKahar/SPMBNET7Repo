using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.EFRepository._02_Akaun
{
    public class AkCimbEFT1Repository : ListViewIRepository<AkCimbEFT1, int>
    {
        public readonly ApplicationDbContext context;

        public AkCimbEFT1Repository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AkCimbEFT1.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkCimbEFT1>> GetAll(int akCimbEFTid)
        {
            return await context.AkCimbEFT1
                .Where(x => x.AkCimbEFTId == akCimbEFTid)
                .ToArrayAsync();
        }

        public async Task<AkCimbEFT1> GetBy2Id(int akCimbEFTId, int indek)
        {
            return await context.AkCimbEFT1.FirstOrDefaultAsync(x => x.AkCimbEFTId == akCimbEFTId && x.Indek == indek) ?? new AkCimbEFT1();
        }

        public async Task<AkCimbEFT1> GetById(int id)
        {
            return await context.AkCimbEFT1.FindAsync(id) ?? new AkCimbEFT1();
        }

        public async Task<AkCimbEFT1> Insert(AkCimbEFT1 entity)
        {
            await context.AkCimbEFT1.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkCimbEFT1 entity)
        {
            AkCimbEFT1 data = context.AkCimbEFT1.FirstOrDefault(x => x.Id == entity.Id);
            await context.SaveChangesAsync();
        }
    }
}
