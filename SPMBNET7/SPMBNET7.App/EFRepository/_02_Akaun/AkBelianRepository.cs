using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.EFRepository._02_Akaun
{
    public class AkBelianRepository : IRepository<AkBelian, int, string>
    {
        public readonly ApplicationDbContext context;

        public AkBelianRepository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AkBelian.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkBelian>> GetAll()
        {
            return await context.AkBelian
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.AkPO)
                    .ThenInclude(b => b!.AkPembekal)
                .Include(b => b.AkInden)
                    .ThenInclude(b => b!.AkPembekal)
                .Include(b => b.AkPembekal)
                .Include(b => b.KodObjekAP)
                .Include(b => b.AkBelian1!).ThenInclude(b => b.AkCarta)
                .Include(b => b.AkBelian2)
                .ToListAsync() ;
        }

        public async Task<IEnumerable<AkBelian>> GetAllIncludeDeletedItems()
        {
            return await context.AkBelian
                .IgnoreQueryFilters()
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.AkPO)
                    .ThenInclude(b => b!.AkPembekal)
                .Include(b => b.AkInden)
                    .ThenInclude(b => b!.AkPembekal)
                .Include(b => b.AkPembekal)
                .Include(b => b.KodObjekAP)
                .Include(b => b.AkBelian1!).ThenInclude(b => b.AkCarta)
                .Include(b => b.AkBelian2)
                .ToListAsync();
        }

        public async Task<AkBelian> GetById(int id)
        {
            return await context.AkBelian.Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.AkPO)
                    .ThenInclude(b => b!.AkPembekal)
                .Include(b => b.AkInden)
                    .ThenInclude(b => b!.AkPembekal)
                .Include(b => b.AkPembekal)
                .Include(b => b.KodObjekAP)
                .Include(b => b.AkBelian1!).ThenInclude(b => b.AkCarta)
                .Include(b => b.AkBelian2)
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync() ?? new AkBelian();
        }

        public async Task<AkBelian> GetByIdIncludeDeletedItems(int id)
        {
            return await context.AkBelian.Include(b => b.JKW)
                .IgnoreQueryFilters()
                .Include(b => b.JBahagian)
                .Include(b => b.AkPO)
                    .ThenInclude(b => b!.AkPembekal)
                .Include(b => b.AkInden)
                    .ThenInclude(b => b!.AkPembekal)
                .Include(b => b.AkPembekal)
                .Include(b => b.KodObjekAP)
                .Include(b => b.AkBelian1!).ThenInclude(b => b.AkCarta)
                .Include(b => b.AkBelian2)
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync() ?? new AkBelian();
        }

        public Task<AkBelian> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<AkBelian> Insert(AkBelian entity)
        {
            await context.AkBelian.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkBelian entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
