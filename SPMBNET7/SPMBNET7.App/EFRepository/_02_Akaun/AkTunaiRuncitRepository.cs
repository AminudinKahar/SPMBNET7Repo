using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.EFRepository._02_Akaun
{
    public class AkTunaiRuncitRepository : IRepository<AkTunaiRuncit, int, string>
    {
        public readonly ApplicationDbContext context;
        public AkTunaiRuncitRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var model = await context.AkTunaiRuncit.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkTunaiRuncit>> GetAll()
        {
            return await context.AkTunaiRuncit
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.AkCarta)
                .Include(b => b.AkTunaiPemegang).ThenInclude(b => b.SuPekerja)
                .ToListAsync();
        }

        public async Task<IEnumerable<AkTunaiRuncit>> GetAllIncludeDeletedItems()
        {
            return await context.AkTunaiRuncit
                .IgnoreQueryFilters()
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.AkCarta)
                .Include(b => b.AkTunaiPemegang).ThenInclude(b => b.SuPekerja)
                .ToListAsync();
        }

        public async Task<AkTunaiRuncit> GetById(int id)
        {
            return await context.AkTunaiRuncit
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.AkCarta)
                .Include(b => b.AkTunaiPemegang).ThenInclude(b => b.SuPekerja)
                .Include(b => b.AkTunaiLejar)
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync() ?? new AkTunaiRuncit();
        }

        public async Task<AkTunaiRuncit> GetByIdIncludeDeletedItems(int id)
        {
            return await context.AkTunaiRuncit.IgnoreQueryFilters()
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.AkCarta)
                .Include(b => b.AkTunaiPemegang)
                .ThenInclude(b => b.SuPekerja)
                .Include(b => b.AkTunaiLejar)
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync() ?? new AkTunaiRuncit();
        }

        public Task<AkTunaiRuncit> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<AkTunaiRuncit> Insert(AkTunaiRuncit entity)
        {
            await context.AkTunaiRuncit.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkTunaiRuncit entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
