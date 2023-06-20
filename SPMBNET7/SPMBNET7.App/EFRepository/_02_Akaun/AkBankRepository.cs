using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.EFRepository._02_Akaun
{
    public class AkBankRepository : IRepository<AkBank, int, string>
    {
        public readonly ApplicationDbContext context;

        public AkBankRepository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AkBank.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }

        }

        public async Task<IEnumerable<AkBank>> GetAll()
        {

            return await context.AkBank
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.JBank)
                .Include(b => b.AkCarta)
                .ToListAsync();


        }

        public Task<IEnumerable<AkBank>> GetAllIncludeDeletedItems()
        {
            throw new NotImplementedException();
        }

        public async Task<AkBank> GetById(int id)
        {
            return await context.AkBank
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.JBank)
                .Include(b => b.AkCarta)
                .Where(d => d.Id == id)
                .FirstOrDefaultAsync() ?? new AkBank();
        }

        public Task<AkBank> GetByIdIncludeDeletedItems(int id)
        {
            throw new NotImplementedException();
        }

        public Task<AkBank> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<AkBank> Insert(AkBank entity)
        {
            await context.AkBank.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkBank entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
