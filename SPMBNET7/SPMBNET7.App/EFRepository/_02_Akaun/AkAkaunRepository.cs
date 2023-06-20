using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.EFRepository._02_Akaun
{
    public class AkAkaunRepository : IRepository<AkAkaun, int, string>
    {
        public readonly ApplicationDbContext context;

        public AkAkaunRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var model = await context.AkAkaun.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkAkaun>> GetAll()
        {
            return await context.AkAkaun
                .Include(b => b.JKW)
                .Include(b => b.AkCarta1)
                .Include(b => b.AkCarta2)
                .ToListAsync();
        }

        public Task<IEnumerable<AkAkaun>> GetAllIncludeDeletedItems()
        {
            throw new NotImplementedException();
        }

        public async Task<AkAkaun> GetById(int id)
        {
            return await context.AkAkaun.FindAsync(id) ?? new AkAkaun();
        }

        public Task<AkAkaun> GetByIdIncludeDeletedItems(int id)
        {
            throw new NotImplementedException();
        }

        public Task<AkAkaun> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<AkAkaun> Insert(AkAkaun entity)
        {
            await context.AkAkaun.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkAkaun entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
