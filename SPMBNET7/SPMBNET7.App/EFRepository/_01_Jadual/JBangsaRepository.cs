using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.EFRepository._01_Jadual
{
    public class JBangsaRepository : IRepository<JBangsa, int, string>
    {
        private readonly ApplicationDbContext context;
        public JBangsaRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var bangsa = await context.JBangsa.FirstOrDefaultAsync(b => b.Id == id);
            if (bangsa != null)
            {
                context.Remove(bangsa);
            }
        }

        public async Task<IEnumerable<JBangsa>> GetAll()
        {
            return await context.JBangsa.ToListAsync();
        }

        public async Task<IEnumerable<JBangsa>> GetAllIncludeDeletedItems()
        {
            return await context.JBangsa.IgnoreQueryFilters().ToListAsync();
        }

        public async Task<JBangsa> GetById(int id)
        {
            return await context.JBangsa.FindAsync(id) ?? new JBangsa();
        }

        public async Task<JBangsa> GetByIdIncludeDeletedItems(int id)
        {
            return await context.JBangsa.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == id) ?? new JBangsa();
        }

        public Task<JBangsa> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<JBangsa> Insert(JBangsa entity)
        {
            await context.JBangsa.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(JBangsa entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
