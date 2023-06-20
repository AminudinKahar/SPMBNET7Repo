using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.EFRepository._01_Jadual
{
    [Authorize]
    public class JKWRepository : IRepository<JKW, int, string>
    {
        private readonly ApplicationDbContext context;
        public JKWRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var kw = await context.JKW.FirstOrDefaultAsync(b => b.Id == id);
            if (kw != null)
            {
                context.Remove(kw);
            }
        }

        public async Task<IEnumerable<JKW>> GetAll()
        {
            return await context.JKW.ToListAsync();
        }

        public async Task<IEnumerable<JKW>> GetAllIncludeDeletedItems()
        {
            return await context.JKW.IgnoreQueryFilters().ToListAsync();
        }

        public async Task<JKW> GetById(int id)
        {
            return await context.JKW.FindAsync(id) ?? new JKW();
        }

        public async Task<JKW> GetByIdIncludeDeletedItems(int id)
        {
            return await context.JKW.IgnoreQueryFilters().Where(x => x.Id == id).FirstOrDefaultAsync() ?? new JKW();
        }

        public Task<JKW> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<JKW> Insert(JKW entity)
        {
            await context.JKW.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(JKW entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
