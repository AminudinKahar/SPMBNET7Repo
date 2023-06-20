using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.EFRepository._01_Jadual
{
    public class JPTJRepository : IRepository<JPTJ, int, string>
    {
        private readonly ApplicationDbContext context;

        public JPTJRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var obj = await context.JPTJ.FirstOrDefaultAsync(b => b.Id == id);
            if (obj != null)
            {
                context.Remove(obj);
            }
        }

        public async Task<IEnumerable<JPTJ>> GetAll()
        {
            return await context.JPTJ
                .Include(x => x.JKW)
                .ToListAsync();
        }

        public async Task<IEnumerable<JPTJ>> GetAllIncludeDeletedItems()
        {
            return await context.JPTJ
                .IgnoreQueryFilters()
                .Include(x => x.JKW)
                .ToListAsync();
        }

        public async Task<JPTJ> GetById(int id)
        {
            return await context.JPTJ
                .Include(x => x.JKW)
                .FirstOrDefaultAsync(p => p.Id == id) ?? new JPTJ();
        }

        public Task<JPTJ> GetByIdIncludeDeletedItems(int id)
        {
            throw new NotImplementedException();
        }

        public Task<JPTJ> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<JPTJ> Insert(JPTJ entity)
        {
            await context.JPTJ.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(JPTJ entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
