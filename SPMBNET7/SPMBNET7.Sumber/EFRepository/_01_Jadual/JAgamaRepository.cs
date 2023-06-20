using Microsoft.EntityFrameworkCore;
using SPMBNET7.Sumber.Data;
using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.Sumber.EFRepository._01_Jadual
{
    public class JAgamaRepository : IRepository<JAgama, int, string>
    {
        private readonly ApplicationDbContext context;
        public JAgamaRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var agama = await context.JAgama.FirstOrDefaultAsync(b => b.Id == id);
            if (agama != null)
            {
                context.Remove(agama);
            }
        }

        public async Task<IEnumerable<JAgama>> GetAll()
        {
            return await context.JAgama.ToListAsync();
        }

        public async Task<IEnumerable<JAgama>> GetAllIncludeDeletedItems()
        {
            return await context.JAgama.IgnoreQueryFilters().ToListAsync();
        }

        public async Task<JAgama> GetById(int id)
        {
            return await context.JAgama.FindAsync(id) ?? new JAgama();
        }

        public async Task<JAgama> GetByIdIncludeDeletedItems(int id)
        {
            return await context.JAgama.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == id) ?? new JAgama();
        }

        public Task<JAgama> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<JAgama> Insert(JAgama entity)
        {
            await context.JAgama.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(JAgama entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
