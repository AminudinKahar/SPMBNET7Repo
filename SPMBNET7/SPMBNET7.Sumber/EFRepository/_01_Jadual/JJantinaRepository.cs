using Microsoft.EntityFrameworkCore;
using SPMBNET7.Sumber.Data;
using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.Sumber.EFRepository._01_Jadual
{
    public class JJantinaRepository : IRepository<JJantina, int, string>
    {
        private readonly ApplicationDbContext context;
        public JJantinaRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var jantina = await context.JJantina.FirstOrDefaultAsync(b => b.Id == id);
            if (jantina != null)
            {
                context.Remove(jantina);
            }
        }

        public async Task<IEnumerable<JJantina>> GetAll()
        {
            return await context.JJantina.ToListAsync();
        }

        public async Task<IEnumerable<JJantina>> GetAllIncludeDeletedItems()
        {
            return await context.JJantina
                .IgnoreQueryFilters()
                .ToListAsync();
        }

        public async Task<JJantina> GetById(int id)
        {
            return await context.JJantina.FindAsync(id) ?? new JJantina();
        }

        public async Task<JJantina> GetByIdIncludeDeletedItems(int id)
        {
            return await context.JJantina.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == id) ?? new JJantina();
        }

        public Task<JJantina> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<JJantina> Insert(JJantina entity)
        {
            await context.JJantina.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(JJantina entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
