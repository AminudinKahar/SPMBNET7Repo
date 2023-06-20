using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.EFRepository._02_Akaun
{
    public class AkInden1Repository : ListViewIRepository<AkInden1, int>
    {
        public readonly ApplicationDbContext context;

        public AkInden1Repository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AkInden1.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkInden1>> GetAll(int akIndenId)
        {
            return await context.AkInden1
                .Include(b => b.AkCarta)
                .Where(x => x.AkIndenId == akIndenId)
                .ToListAsync();
        }

        public async Task<AkInden1> GetBy2Id(int akIndenId, int akCartaId)
        {
            return await context.AkInden1.FirstOrDefaultAsync(x => x.AkIndenId == akIndenId && x.AkCartaId == akCartaId) ?? new AkInden1();
        }

        public async Task<AkInden1> GetById(int id)
        {
            return await context.AkInden1.Include(x => x.AkCarta)
                .Where(x => x.Id == id).FirstOrDefaultAsync() ?? new AkInden1();
        }

        public async Task<AkInden1> Insert(AkInden1 entity)
        {
            await context.AkInden1.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkInden1 entity)
        {
            AkInden1 data = context.AkInden1.FirstOrDefault(x => x.Id == entity.Id);
            if (data != null) 
                data.Amaun = entity.Amaun;
            //Tambah kalau ada data dalam field lagi
            await context.SaveChangesAsync();
        }
    }
}
