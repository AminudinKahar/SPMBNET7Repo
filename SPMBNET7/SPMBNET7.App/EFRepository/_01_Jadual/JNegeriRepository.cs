using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.EFRepository._01_Jadual
{
    public class JNegeriRepository : IRepository<JNegeri, int, string>
    {
        public readonly ApplicationDbContext context;

        public JNegeriRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var model = await context.JNegeri.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<JNegeri>> GetAll()
        {
            return await context.JNegeri.ToListAsync();
        }

        public async Task<IEnumerable<JNegeri>> GetAllIncludeDeletedItems()
        {
            return await context.JNegeri
                .IgnoreQueryFilters()
                .ToListAsync();
        }

        public async Task<JNegeri> GetById(int id)
        {
            return await context.JNegeri.FindAsync(id) ?? new JNegeri();

        }

        public async Task<JNegeri> GetByIdIncludeDeletedItems(int id)
        {
            return await context.JNegeri
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id) ?? new JNegeri();
        }

        public Task<JNegeri> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<JNegeri> Insert(JNegeri entity)
        {
            await context.JNegeri.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(JNegeri entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
