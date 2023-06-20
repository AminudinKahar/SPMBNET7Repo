using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.EFRepository._02_Akaun
{
    public class AkTerima2Repository : ListViewIRepository<AkTerima2, int>
    {
        public readonly ApplicationDbContext context;

        public AkTerima2Repository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var model = await context.AkTerima2.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkTerima2>> GetAll(int akTerimaId)
        {
            return await context.AkTerima2
                .Include(b => b.JCaraBayar)
                .Where(x => x.AkTerimaId == akTerimaId)
                .ToListAsync();
        }

        public async Task<AkTerima2> GetBy2Id(int akTerimaId, int jCaraBayarId)
        {
            var result = await context.AkTerima2.FirstOrDefaultAsync(x => x.AkTerimaId == akTerimaId && x.JCaraBayarId == jCaraBayarId);
            return result!;
        }

        public async Task<AkTerima2> GetById(int id)
        {
            return await context.AkTerima2
                .Include(b => b.JCaraBayar)
                .Include(b => b.AkPadananPenyata)
                .Where(x => x.Id == id).FirstOrDefaultAsync() ?? new AkTerima2();
        }

        public async Task<AkTerima2> Insert(AkTerima2 entity)
        {
            await context.AkTerima2.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkTerima2 entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
