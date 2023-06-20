using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.EFRepository._02_Akaun
{
    public class AkBankReconRepository : IRepository<AkBankRecon, int, string>
    {
        public readonly ApplicationDbContext context;
        public AkBankReconRepository(ApplicationDbContext context)
        {
            this.context=context;
        }

        public async Task Delete(int id)
        {
            var akRecon = await context.AkBankRecon.FirstOrDefaultAsync(b => b.Id == id);
            if (akRecon != null)
            {
                context.Remove(akRecon);
            }
        }

        public async Task<IEnumerable<AkBankRecon>> GetAll()
        {
            return await context.AkBankRecon
                .Include(b => b.AkBank)
                    .ThenInclude(b => b!.JBank)
                .Include(b => b.AkBank)
                    .ThenInclude(b => b!.AkCarta)
                .Include(b => b.AkBankReconPenyataBank!)
                    .ThenInclude(b => b.AkPadananPenyata)
                        .ThenInclude(b => b.AkPV)
                .Include(b => b.AkBankReconPenyataBank!)
                    .ThenInclude(b => b.AkPadananPenyata)
                        .ThenInclude(b => b.AkTerima2)
                            .ThenInclude(b => b.AkTerima)
                .ToListAsync();



        }

        public async Task<IEnumerable<AkBankRecon>> GetAllIncludeDeletedItems()
        {
            return await context.AkBankRecon
                .IgnoreQueryFilters()
                .Include(b => b.AkBank)
                    .ThenInclude(b => b!.JBank)
                .Include(b => b.AkBank)
                    .ThenInclude(b => b!.AkCarta)
                .Include(b => b.AkBankReconPenyataBank!)
                    .ThenInclude(b => b.AkPadananPenyata)
                        .ThenInclude(b => b.AkPV)
                .Include(b => b.AkBankReconPenyataBank!)
                    .ThenInclude(b => b.AkPadananPenyata)
                        .ThenInclude(b => b.AkTerima2)
                            .ThenInclude(b => b.AkTerima)
                .Include(b => b.AkBankReconPenyataBank!)
                    .ThenInclude(b => b.AkPadananPenyata)
                        .ThenInclude(b => b.AkJurnal)
                .ToListAsync();
        }

        public async Task<AkBankRecon> GetById(int id)
        {
            return await context.AkBankRecon
                .IgnoreQueryFilters()
                .Include(b => b.AkBank)
                    .ThenInclude(b => b!.JBank)
                .Include(b => b.AkBank)
                    .ThenInclude(b => b!.AkCarta)
                .Include(b => b.AkBankReconPenyataBank!)
                    .ThenInclude(b => b.AkPadananPenyata)
                        .ThenInclude(b => b.AkPV)
                .Include(b => b.AkBankReconPenyataBank!)
                    .ThenInclude(b => b.AkPadananPenyata)
                        .ThenInclude(b => b.AkTerima2)
                            .ThenInclude(b => b.AkTerima)
                .Include(b => b.AkBankReconPenyataBank!)
                    .ThenInclude(b => b.AkPadananPenyata)
                        .ThenInclude(b => b.AkJurnal)
                .FirstOrDefaultAsync(b => b.Id == id) ?? new AkBankRecon();
        }

        public async Task<AkBankRecon> GetByIdIncludeDeletedItems(int id)
        {
            return await context.AkBankRecon
                .IgnoreQueryFilters()
                .Include(b => b.AkBank)
                    .ThenInclude(b => b!.JBank)
                .Include(b => b.AkBank)
                    .ThenInclude(b => b!.AkCarta)
                .Include(b => b.AkBankReconPenyataBank!)
                    .ThenInclude(b => b.AkPadananPenyata)
                        .ThenInclude(b => b.AkPV)
                .Include(b => b.AkBankReconPenyataBank!)
                    .ThenInclude(b => b.AkPadananPenyata)
                        .ThenInclude(b => b.AkTerima2)
                            .ThenInclude(b => b.AkTerima)
                .Include(b => b.AkBankReconPenyataBank!)
                    .ThenInclude(b => b.AkPadananPenyata)
                        .ThenInclude(b => b.AkJurnal)
                .FirstOrDefaultAsync(b => b.Id == id) ?? new AkBankRecon();
        }

        public Task<AkBankRecon> GetByString(string id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<AkBankRecon> Insert(AkBankRecon entity)
        {
            await context.AkBankRecon.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkBankRecon entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
