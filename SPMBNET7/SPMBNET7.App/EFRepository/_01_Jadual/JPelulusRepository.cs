using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.EFRepository._01_Jadual
{
    public class JPelulusRepository : IRepository<JPelulus, int, string>
    {
        public readonly ApplicationDbContext context;

        public JPelulusRepository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.JPelulus.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<JPelulus>> GetAll()
        {
            return await context.JPelulus.Include(b => b.SuPekerja).ToListAsync();
        }

        public async Task<IEnumerable<JPelulus>> GetAllIncludeDeletedItems()
        {
            return await context.JPelulus
                .IgnoreQueryFilters()
                .Include(b => b.SuPekerja).ToListAsync();
        }

        public async Task<JPelulus> GetById(int id)
        {
            return await context.JPelulus.Include(b => b.SuPekerja)
                .FirstOrDefaultAsync(x => x.Id == id) ?? new JPelulus();
        }

        public async Task<JPelulus> GetByIdIncludeDeletedItems(int id)
        {
            return await context.JPelulus.IgnoreQueryFilters()
                .Include(b => b.SuPekerja)
                .FirstOrDefaultAsync(x => x.Id == id) ?? new JPelulus();
        }

        public Task<JPelulus> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<JPelulus> Insert(JPelulus entity)
        {
            await context.JPelulus.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(JPelulus entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
