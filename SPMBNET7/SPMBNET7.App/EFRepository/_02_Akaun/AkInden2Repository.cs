using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.EFRepository._02_Akaun
{
    public class AkInden2Repository : ListViewIRepository<AkInden2, int>
    {
        public readonly ApplicationDbContext context;

        public AkInden2Repository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AkInden2.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkInden2>> GetAll(int akIndenId)
        {
            return await context.AkInden2
                //.Include(b => b.JCaraBayar)
                .Where(x => x.AkIndenId == akIndenId)
                .ToListAsync();
        }

        public Task<AkInden2> GetBy2Id(int id1, int id2)
        {
            throw new System.NotImplementedException();
        }

        public async Task<AkInden2> GetById(int id)
        {
            return await context.AkInden2.FindAsync(id) ?? new AkInden2();
        }

        public async Task<AkInden2> Insert(AkInden2 entity)
        {
            await context.AkInden2.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkInden2 entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
