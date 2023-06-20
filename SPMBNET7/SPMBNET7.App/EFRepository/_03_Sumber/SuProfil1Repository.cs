using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Modules._03_Sumber;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.EFRepository._03_Sumber
{
    public class SuProfil1Repository : IRepository<SuProfil1, int, string>
    {
        public readonly ApplicationDbContext context;

        public SuProfil1Repository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var model = await context.SuProfil1.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<SuProfil1>> GetAll()
        {
            return await context.SuProfil1.ToListAsync();
        }

        public async Task<IEnumerable<SuProfil1>> GetAllIncludeDeletedItems()
        {
            return await context.SuProfil1
                .IgnoreQueryFilters()
                .ToListAsync();
        }

        public async Task<SuProfil1> GetById(int id)
        {
            return await context.SuProfil1.FindAsync(id) ?? new SuProfil1();

        }

        public async Task<SuProfil1> GetByIdIncludeDeletedItems(int id)
        {
            return await context.SuProfil1
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id) ?? new SuProfil1();
        }

        public Task<SuProfil1> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<SuProfil1> Insert(SuProfil1 entity)
        {
            await context.SuProfil1.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(SuProfil1 entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
