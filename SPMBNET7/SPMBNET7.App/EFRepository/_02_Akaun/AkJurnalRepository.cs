using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.EFRepository._02_Akaun
{
    public class AkJurnalRepository : IRepository<AkJurnal, int, string>
    {
        public readonly ApplicationDbContext context;
        public AkJurnalRepository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AkJurnal.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkJurnal>> GetAll()
        {
            return await context.AkJurnal
                .Include(b => b.JBahagian)
                .Include(b => b.AkTunaiRuncit)
                .Include(b => b.JKW)
                .Include(b => b.AkJurnal1)
                    .ThenInclude(b => b.JBahagianDebit)
                .Include(b => b.AkJurnal1)
                    .ThenInclude(b => b.AkCartaDebit)
                .Include(b => b.AkJurnal1)
                    .ThenInclude(b => b.JBahagianKredit)
                .Include(b => b.AkJurnal1)
                    .ThenInclude(b => b.AkCartaKredit)
                .ToListAsync();
        }

        public async Task<IEnumerable<AkJurnal>> GetAllIncludeDeletedItems()
        {
            return await context.AkJurnal
                .IgnoreQueryFilters()
                .Include(b => b.JBahagian)
                .Include(b => b.AkTunaiRuncit)
                .Include(b => b.JKW)
                .Include(b => b.AkJurnal1)
                    .ThenInclude(b => b.JBahagianDebit)
                .Include(b => b.AkJurnal1)
                    .ThenInclude(b => b.AkCartaDebit)
                .Include(b => b.AkJurnal1)
                    .ThenInclude(b => b.JBahagianKredit)
                .Include(b => b.AkJurnal1)
                    .ThenInclude(b => b.AkCartaKredit)
                .ToListAsync();
        }

        public async Task<AkJurnal> GetById(int id)
        {
            return await context.AkJurnal
                .Include(b => b.JBahagian)
                .Include(b => b.AkTunaiRuncit)
                .Include(b => b.JKW)
                .Include(b => b.AkPadananPenyata)
                .Include(b => b.AkJurnal1)
                    .ThenInclude(b => b.JBahagianDebit)
                .Include(b => b.AkJurnal1)
                    .ThenInclude(b => b.AkCartaDebit)
                .Include(b => b.AkJurnal1)
                    .ThenInclude(b => b.JBahagianKredit)
                .Include(b => b.AkJurnal1)
                    .ThenInclude(b => b.AkCartaKredit)
                .Where(x => x.Id == id).FirstOrDefaultAsync() ?? new AkJurnal();
        }

        public async Task<AkJurnal> GetByIdIncludeDeletedItems(int id)
        {
            return await context.AkJurnal
                .IgnoreQueryFilters()
                .Include(b => b.JBahagian)
                .Include(b => b.AkTunaiRuncit)
                .Include(b => b.JKW)
                .Include(b => b.AkJurnal1)
                    .ThenInclude(b => b.JBahagianDebit)
                .Include(b => b.AkJurnal1)
                    .ThenInclude(b => b.AkCartaDebit)
                .Include(b => b.AkJurnal1)
                    .ThenInclude(b => b.JBahagianKredit)
                .Include(b => b.AkJurnal1)
                    .ThenInclude(b => b.AkCartaKredit)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync() ?? new AkJurnal();
        }

        public Task<AkJurnal> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<AkJurnal> Insert(AkJurnal entity)
        {
            await context.AkJurnal.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkJurnal entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
