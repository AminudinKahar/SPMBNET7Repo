using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.EFRepository._02_Akaun
{
    public class AkNotaMinta1Repository : ListViewIRepository<AkNotaMinta1, int>
    {
        public readonly ApplicationDbContext context;

        public AkNotaMinta1Repository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AkNotaMinta1.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkNotaMinta1>> GetAll(int AkNotaMintaId)
        {
            return await context.AkNotaMinta1.Include(b => b.AkCarta)
                .Where(x => x.AkNotaMintaId == AkNotaMintaId)
                .ToArrayAsync();
        }

        public async Task<AkNotaMinta1> GetBy2Id(int akNotaMintaId, int akCartaId)
        {
            return await context.AkNotaMinta1.FirstOrDefaultAsync(x => x.AkNotaMintaId == akNotaMintaId && x.AkCartaId == akCartaId) ?? new AkNotaMinta1();
        }

        public async Task<AkNotaMinta1> GetById(int id)
        {
            return await context.AkNotaMinta1.Include(d => d.AkCarta).Where(d => d.Id == id).FirstOrDefaultAsync() ?? new AkNotaMinta1();
        }

        public async Task<AkNotaMinta1> Insert(AkNotaMinta1 entity)
        {
            await context.AkNotaMinta1.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkNotaMinta1 entity)
        {
            AkNotaMinta1 data = context.AkNotaMinta1.FirstOrDefault(x => x.Id == entity.Id);
            data!.Amaun = entity.Amaun;
            await context.SaveChangesAsync();
        }
    }
}
