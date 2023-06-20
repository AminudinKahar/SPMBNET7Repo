using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.EFRepository._02_Akaun
{
    public class AkPOLarasRepository : IRepository<AkPOLaras, int, string>
    {
        public readonly ApplicationDbContext context;

        public AkPOLarasRepository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AkPOLaras.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkPOLaras>> GetAll()
        {
            return await context.AkPOLaras
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.AkPO).ThenInclude(b => b!.AkPembekal)
                .Include(b => b.AkPOLaras1)
                .Include(b => b.AkPOLaras2)
                .Include(d => d.AkPO)
                    .ThenInclude(d => d!.AkPembekal)
                        .ThenInclude(d => d!.JBank)
                .Include(d => d.AkPO)
                    .ThenInclude(d => d!.AkPembekal)
                        .ThenInclude(d => d!.JNegeri)
                .Include(d => d.AkPO)
                    .ThenInclude(d => d!.AkPO1)
                        .ThenInclude(d => d.AkCarta)
                .ToListAsync();
        }

        public async Task<IEnumerable<AkPOLaras>> GetAllIncludeDeletedItems()
        {
            return await context.AkPOLaras
                .IgnoreQueryFilters()
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.AkPO).ThenInclude(b => b!.AkPembekal)
                .Include(b => b.AkPOLaras1)
                .Include(b => b.AkPOLaras2)
                .Include(d => d.AkPO)
                    .ThenInclude(d => d!.AkPembekal)
                        .ThenInclude(d => d!.JBank)
                .Include(d => d.AkPO)
                    .ThenInclude(d => d!.AkPembekal)
                        .ThenInclude(d => d!.JNegeri)
                .Include(d => d.AkPO)
                    .ThenInclude(d => d!.AkPO1)
                        .ThenInclude(d => d.AkCarta)
                .ToListAsync();
        }

        public async Task<AkPOLaras> GetById(int id)
        {
            return await context.AkPOLaras
                .Where(d => d.Id == id)
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(d => d.AkPOLaras1).ThenInclude(d => d.AkCarta)
                .Include(d => d.AkPOLaras2)
                .Include(d => d.AkPO)
                    .ThenInclude(d => d!.AkPembekal)
                        .ThenInclude(d => d!.JBank)
                .Include(d => d.AkPO)
                    .ThenInclude(d => d!.AkPembekal)
                        .ThenInclude(d => d!.JNegeri)
                .Include(d => d.AkPO)
                    .ThenInclude(d => d!.AkPO1)
                        .ThenInclude(d => d.AkCarta)
                .FirstOrDefaultAsync() ?? new AkPOLaras();
        }

        public async Task<AkPOLaras> GetByIdIncludeDeletedItems(int id)
        {
            return await context.AkPOLaras
                .IgnoreQueryFilters()
                .Where(d => d.Id == id)
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(d => d.AkPOLaras1).ThenInclude(d => d.AkCarta)
                .Include(d => d.AkPOLaras2)
                .Include(d => d.AkPO)
                    .ThenInclude(d => d!.AkPembekal)
                        .ThenInclude(d => d!.JBank)
                .Include(d => d.AkPO)
                    .ThenInclude(d => d!.AkPembekal)
                        .ThenInclude(d => d!.JNegeri)
                .Include(d => d.AkPO)
                    .ThenInclude(d => d!.AkPO1)
                        .ThenInclude(d => d.AkCarta)
                .FirstOrDefaultAsync() ?? new AkPOLaras();
        }

        public Task<AkPOLaras> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<AkPOLaras> Insert(AkPOLaras entity)
        {
            await context.AkPOLaras.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkPOLaras entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
