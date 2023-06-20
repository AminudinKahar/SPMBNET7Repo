using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.EFRepository._02_Akaun
{
    public class AkPenghutangRepository : IRepository<AkPenghutang, int, string>
    {
        public readonly ApplicationDbContext context;
        public AkPenghutangRepository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var akPenghutang = await context.AkPenghutang.FirstOrDefaultAsync(b => b.Id == id);
            if (akPenghutang != null)
            {
                context.Remove(akPenghutang);
            }
        }

        public async Task<IEnumerable<AkPenghutang>> GetAll()
        {
            return await context.AkPenghutang
                .Include(b => b.JNegeri).Include(b => b.JBank)
                .ToListAsync();
        }

        public async Task<IEnumerable<AkPenghutang>> GetAllIncludeDeletedItems()
        {
            return await context.AkPenghutang
                .IgnoreQueryFilters()
                .Include(b => b.JNegeri).Include(b => b.JBank)
                .ToListAsync();
        }

        public async Task<AkPenghutang> GetById(int id)
        {
            return await context.AkPenghutang.Include(b => b.JNegeri)
                .Include(b => b.JBank)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync() ?? new AkPenghutang();
        }

        public async Task<AkPenghutang> GetByIdIncludeDeletedItems(int id)
        {
            return await context.AkPenghutang
                .IgnoreQueryFilters()
                .Include(b => b.JNegeri).Include(b => b.JBank)
                .Where(x => x.Id == id).FirstOrDefaultAsync() ?? new AkPenghutang();
        }

        public Task<AkPenghutang> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<AkPenghutang> Insert(AkPenghutang entity)
        {
            await context.AkPenghutang.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkPenghutang entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
