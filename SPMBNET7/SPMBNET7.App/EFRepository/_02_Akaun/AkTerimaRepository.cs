using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.EFRepository._02_Akaun
{
    public class AkTerimaRepository : IRepository<AkTerima, int, string>
    {
        public readonly ApplicationDbContext context;

        public AkTerimaRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var model = await context.AkTerima.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkTerima>> GetAll()
        {
            return await context.AkTerima
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.SpPendahuluanPelbagai)
                .Include(b => b.AkBank)
                .Include(b => b.JNegeri)
                .Include(b => b.AkTerima1)
                    .ThenInclude(b => b.AkCarta)
                .Include(b => b.AkTerima2)
                    .ThenInclude(b => b.JCaraBayar)
                .Include(b => b.AkTerima3)
                    .ThenInclude(b => b.AkInvois)
                .ToListAsync();
        }

        public async Task<IEnumerable<AkTerima>> GetAllIncludeDeletedItems()
        {
            return await context.AkTerima
                .IgnoreQueryFilters()
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.SpPendahuluanPelbagai)
                .Include(b => b.AkBank)
                .Include(b => b.JNegeri)
                .Include(b => b.AkTerima1)
                    .ThenInclude(b => b.AkCarta)
                .Include(b => b.AkTerima2)
                    .ThenInclude(b => b.JCaraBayar)
                .Include(b => b.AkTerima3)
                    .ThenInclude(b => b.AkInvois)
                .ToListAsync();
        }

        public async Task<AkTerima> GetById(int id)
        {
            return await context.AkTerima
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.SpPendahuluanPelbagai)
                .Include(b => b.AkBank)
                .Include(b => b.JNegeri)
                .Include(b => b.AkTerima1)
                    .ThenInclude(b => b.AkCarta)
                .Include(b => b.AkTerima2)
                    .ThenInclude(b => b.JCaraBayar)
                .Include(b => b.AkTerima3)
                    .ThenInclude(b => b.AkInvois)
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync() ?? new AkTerima();
        }

        public async Task<AkTerima> GetByIdIncludeDeletedItems(int id)
        {
            return await context.AkTerima
                .IgnoreQueryFilters()
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.SpPendahuluanPelbagai)
                .Include(b => b.AkBank)
                .Include(b => b.JNegeri)
                .Include(b => b.AkTerima1)
                    .ThenInclude(b => b.AkCarta)
                .Include(b => b.AkTerima2)
                    .ThenInclude(b => b.JCaraBayar)
                .Include(b => b.AkTerima3)
                    .ThenInclude(b => b.AkInvois)
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync() ?? new AkTerima();
        }

        public Task<AkTerima> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<AkTerima> Insert(AkTerima entity)
        {
            await context.AkTerima.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkTerima entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
