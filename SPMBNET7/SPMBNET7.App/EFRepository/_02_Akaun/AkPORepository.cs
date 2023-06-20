using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.EFRepository._02_Akaun
{
    public class AkPORepository : IRepository<AkPO, int, string>
    {
        public readonly ApplicationDbContext context;

        public AkPORepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var model = await context.AkPO.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkPO>> GetAll()
        {
            return await context.AkPO
                .Include(b => b.JBahagian)
                .Include(b => b.JKW)
                .Include(b => b.AkPembekal)
                .Include(b => b.AkNotaMinta)
                .Include(b => b.AkPO1)
                .Include(b => b.AkPO2)
                .ToListAsync();
        }

        public async Task<IEnumerable<AkPO>> GetAllIncludeDeletedItems()
        {
            return await context.AkPO
                .IgnoreQueryFilters()
                .Include(b => b.JBahagian)
                .Include(b => b.JKW)
                .Include(b => b.AkPembekal)
                .Include(b => b.AkNotaMinta)
                .Include(b => b.AkPO1)
                .Include(b => b.AkPO2)
                .ToListAsync();
        }

        public async Task<AkPO> GetById(int id)
        {
            return await context.AkPO
                .Where(d => d.Id == id)
                .Include(b => b.JBahagian)
                .Include(b => b.JKW)
                .Include(b => b.AkNotaMinta)
                .Include(d => d.AkPO1).ThenInclude(d => d.AkCarta)
                .Include(d => d.AkPO2)
                .Include(d => d.AkPembekal).ThenInclude(d => d!.JNegeri)
                .Include(d => d.AkPembekal).ThenInclude(d => d!.JBank)
                .FirstOrDefaultAsync() ?? new AkPO();
        }

        public async Task<AkPO> GetByIdIncludeDeletedItems(int id)
        {
            return await context.AkPO
                .IgnoreQueryFilters()
                .Where(d => d.Id == id)
                .Include(b => b.JBahagian)
                .Include(b => b.JKW)
                .Include(b => b.AkNotaMinta)
                .Include(d => d.AkPO1).ThenInclude(d => d.AkCarta)
                .Include(d => d.AkPO2)
                .Include(d => d.AkPembekal).ThenInclude(d => d!.JNegeri)
                .Include(d => d.AkPembekal).ThenInclude(d => d!.JBank)
                .FirstOrDefaultAsync() ?? new AkPO();
        }

        public Task<AkPO> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<AkPO> Insert(AkPO entity)
        {
            await context.AkPO.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkPO entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
