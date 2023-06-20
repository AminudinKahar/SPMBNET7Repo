using Microsoft.EntityFrameworkCore;
using SPMBNET7.Sumber.Data;
using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.Sumber.EFRepository._01_Jadual
{
    public class JPenyemakRepository : IRepository<JPenyemak, int, string>
    {

        public readonly ApplicationDbContext context;

        public JPenyemakRepository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.JPenyemak.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<JPenyemak>> GetAll()
        {
            return await context.JPenyemak.Include(b => b.SuPekerja).ToListAsync();
        }

        public async Task<IEnumerable<JPenyemak>> GetAllIncludeDeletedItems()
        {
            return await context.JPenyemak
                .IgnoreQueryFilters()
                .Include(b => b.SuPekerja).ToListAsync();
        }

        public async Task<JPenyemak> GetById(int id)
        {
            return await context.JPenyemak.Include(b => b.SuPekerja)
                .FirstOrDefaultAsync(x => x.Id == id) ?? new JPenyemak();
        }

        public async Task<JPenyemak> GetByIdIncludeDeletedItems(int id)
        {
            return await context.JPenyemak.IgnoreQueryFilters()
                .Include(b => b.SuPekerja)
                .FirstOrDefaultAsync(x => x.Id == id) ?? new JPenyemak();
        }

        public Task<JPenyemak> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<JPenyemak> Insert(JPenyemak entity)
        {
            await context.JPenyemak.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(JPenyemak entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
