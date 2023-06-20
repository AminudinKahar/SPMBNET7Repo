using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.EFRepository._02_Akaun
{
    public class AkNotaDebitKreditBelianRepository : IRepository<AkNotaDebitKreditBelian, int, string>
    {
        public readonly ApplicationDbContext context;
        public AkNotaDebitKreditBelianRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var model = await context.AkNotaDebitKreditBelian.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkNotaDebitKreditBelian>> GetAll()
        {
            return await context.AkNotaDebitKreditBelian
                .Include(b => b.JBahagian)
                    .ThenInclude(b => b!.JKW)
                .Include(b => b.AkBelian)
                    .ThenInclude(b => b.AkPembekal)
                .Include(b => b.AkBelian)
                    .ThenInclude(b => b.KodObjekAP)
                .Include(b => b.AkBelian)
                    .ThenInclude(b => b.AkPO)
                        .ThenInclude(b => b!.AkPembekal)
                .Include(b => b.AkNotaDebitKreditBelian1)
                    .ThenInclude(b => b.AkCarta)
                .Include(b => b.AkNotaDebitKreditBelian2)
                .ToListAsync();
        }

        public async Task<IEnumerable<AkNotaDebitKreditBelian>> GetAllIncludeDeletedItems()
        {
            return await context.AkNotaDebitKreditBelian
                .IgnoreQueryFilters()
                .Include(b => b.JBahagian)
                    .ThenInclude(b => b!.JKW)
                .Include(b => b.AkBelian)
                    .ThenInclude(b => b.AkPembekal)
                .Include(b => b.AkBelian)
                    .ThenInclude(b => b.KodObjekAP)
                .Include(b => b.AkBelian)
                    .ThenInclude(b => b.AkPO)
                        .ThenInclude(b => b!.AkPembekal)
                .Include(b => b.AkNotaDebitKreditBelian1)
                    .ThenInclude(b => b.AkCarta)
                .Include(b => b.AkNotaDebitKreditBelian2)
                .ToListAsync();
        }

        public async Task<AkNotaDebitKreditBelian> GetById(int id)
        {
            return await context.AkNotaDebitKreditBelian
                .Include(b => b.JBahagian)
                    .ThenInclude(b => b!.JKW)
                .Include(b => b.AkBelian)
                    .ThenInclude(b => b.AkPembekal)
                .Include(b => b.AkBelian)
                    .ThenInclude(b => b.KodObjekAP)
                .Include(b => b.AkBelian)
                    .ThenInclude(b => b.AkPO)
                        .ThenInclude(b => b!.AkPembekal)
                .Include(b => b.AkNotaDebitKreditBelian1)
                    .ThenInclude(b => b.AkCarta)
                .Include(b => b.AkNotaDebitKreditBelian2)
                .FirstOrDefaultAsync(m => m.Id == id) ?? new AkNotaDebitKreditBelian();
        }

        public async Task<AkNotaDebitKreditBelian> GetByIdIncludeDeletedItems(int id)
        {
            return await context.AkNotaDebitKreditBelian
                .IgnoreQueryFilters()
                .Include(b => b.JBahagian)
                    .ThenInclude(b => b!.JKW)
                .Include(b => b.AkBelian)
                    .ThenInclude(b => b.AkPembekal)
                .Include(b => b.AkBelian)
                    .ThenInclude(b => b.KodObjekAP)
                .Include(b => b.AkBelian)
                    .ThenInclude(b => b.AkPO)
                        .ThenInclude(b => b!.AkPembekal)
                .Include(b => b.AkNotaDebitKreditBelian1)
                    .ThenInclude(b => b.AkCarta)
                .Include(b => b.AkNotaDebitKreditBelian2)
                .FirstOrDefaultAsync(m => m.Id == id) ?? new AkNotaDebitKreditBelian();
        }

        public Task<AkNotaDebitKreditBelian> GetByString(string id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<AkNotaDebitKreditBelian> Insert(AkNotaDebitKreditBelian entity)
        {
            await context.AkNotaDebitKreditBelian.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkNotaDebitKreditBelian entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
