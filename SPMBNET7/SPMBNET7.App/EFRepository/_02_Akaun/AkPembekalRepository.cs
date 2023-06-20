using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.EFRepository._02_Akaun
{
    public class AkPembekalRepository : IRepository<AkPembekal, int, string>
    {
        public readonly ApplicationDbContext context;
        public AkPembekalRepository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var akPembekal = await context.AkPembekal.FirstOrDefaultAsync(b => b.Id == id);
            if (akPembekal != null)
            {
                context.Remove(akPembekal);
            }
        }

        public async Task<IEnumerable<AkPembekal>> GetAll()
        {
            return await context.AkPembekal
                .Include(b => b.JNegeri).Include(b => b.JBank)
                .ToListAsync();
        }

        public async Task<IEnumerable<AkPembekal>> GetAllIncludeDeletedItems()
        {
            return await context.AkPembekal
                .IgnoreQueryFilters()
                .Include(b => b.JNegeri).Include(b => b.JBank)
                .ToListAsync();
        }

        public async Task<AkPembekal> GetById(int id)
        {
            return await context.AkPembekal.Include(b => b.JNegeri).Include(b => b.JBank).Where(x => x.Id == id).FirstOrDefaultAsync() ?? new AkPembekal();
        }

        public async Task<AkPembekal> GetByIdIncludeDeletedItems(int id)
        {
            return await context.AkPembekal.IgnoreQueryFilters().Include(b => b.JNegeri).Include(b => b.JBank).Where(x => x.Id == id).FirstOrDefaultAsync() ?? new AkPembekal();
        }

        public Task<AkPembekal> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<AkPembekal> Insert(AkPembekal entity)
        {
            await context.AkPembekal.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkPembekal entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
