using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.EFRepository._02_Akaun
{
    public class AbWaran1Repository : ListViewIRepository<AbWaran1, int>
    {
        public readonly ApplicationDbContext context;

        public AbWaran1Repository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AbWaran.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AbWaran1>> GetAll(int abWaranId)
        {
            return await context.AbWaran1.Include(b => b.AkCarta)
                .Include(b => b.JBahagian)
                .Where(x => x.AbWaranId == abWaranId)
                .ToArrayAsync();
        }

        public async Task<AbWaran1> GetBy2Id(int abWaranId, int akCartaId)
        {
            return await context.AbWaran1
                .FirstOrDefaultAsync(x => x.AbWaranId == abWaranId && x.AkCartaId == akCartaId) ?? new AbWaran1();
        }

        public async Task<AbWaran1> GetById(int id)
        {
            return await context.AbWaran1
                .Include(d => d.AkCarta)
                .Include(b => b.JBahagian)
                .Where(d => d.Id == id)
                .FirstOrDefaultAsync() ?? new AbWaran1();
        }

        public async Task<AbWaran1> Insert(AbWaran1 entity)
        {
            await context.AbWaran1.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AbWaran1 entity)
        {
            AbWaran1 data = context.AbWaran1.FirstOrDefault(x => x.Id == entity.Id);
            if (data != null)
            {
                data.Amaun = entity.Amaun;
                data.TK = entity.TK;
            }
            
            await context.SaveChangesAsync();
        }
    }
}
