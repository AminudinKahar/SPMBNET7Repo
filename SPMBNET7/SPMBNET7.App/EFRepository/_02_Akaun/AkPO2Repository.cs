using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.EFRepository._02_Akaun
{
    public class AkPO2Repository : ListViewIRepository<AkPO2, int>
    {
        public readonly ApplicationDbContext context;

        public AkPO2Repository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AkPO2.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkPO2>> GetAll(int akPOId)
        {
            return await context.AkPO2
                //.Include(b => b.JCaraBayar)
                .Where(x => x.AkPOId == akPOId)
                .ToListAsync();
        }

        public Task<AkPO2> GetBy2Id(int id1, int id2)
        {
            throw new NotImplementedException();
        }

        public async Task<AkPO2> GetById(int id)
        {
            return await context.AkPO2.FindAsync(id) ?? new AkPO2();
        }

        public async Task<AkPO2> Insert(AkPO2 entity)
        {
            await context.AkPO2.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkPO2 entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
