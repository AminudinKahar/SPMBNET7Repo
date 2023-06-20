using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Modules._03_Sumber;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.EFRepository._01_Jadual
{
    public class JSukanRepository : IRepository<JSukan, int, string>
    {
        public readonly ApplicationDbContext context;

        public JSukanRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var model = await context.JSukan.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<JSukan>> GetAll()
        {
            return await context.JSukan.ToListAsync();
        }

        public async Task<IEnumerable<JSukan>> GetAllIncludeDeletedItems()
        {
            return await context.JSukan
                .IgnoreQueryFilters()
                .ToListAsync();
        }

        public async Task<JSukan> GetById(int id)
        {
            return await context.JSukan.FindAsync(id) ?? new JSukan();

        }

        public async Task<JSukan> GetByIdIncludeDeletedItems(int id)
        {
            return await context.JSukan.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == id) ?? new JSukan();
        }

        public Task<JSukan> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<JSukan> Insert(JSukan entity)
        {
            await context.JSukan.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(JSukan entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
