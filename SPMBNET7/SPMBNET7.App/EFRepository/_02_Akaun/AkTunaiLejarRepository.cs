using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.EFRepository._02_Akaun
{
    public class AkTunaiLejarRepository : IRepository<AkTunaiLejar, int, string>
    {
        public readonly ApplicationDbContext context;

        public AkTunaiLejarRepository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AkTunaiLejar.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkTunaiLejar>> GetAll()
        {
            return await context.AkTunaiLejar
                .Include(b => b.JBahagian)
                .Include(b => b.AkTunaiRuncit).ThenInclude(b => b!.JKW)
                .Include(b => b.AkCarta)
                .ToListAsync();
        }

        public Task<IEnumerable<AkTunaiLejar>> GetAllIncludeDeletedItems() => throw new NotImplementedException();

        public Task<AkTunaiLejar> GetById(int id) => throw new NotImplementedException();

        public Task<AkTunaiLejar> GetByIdIncludeDeletedItems(int id) => throw new NotImplementedException();

        public Task<AkTunaiLejar> GetByString(string id) => throw new NotImplementedException();

        public async Task<AkTunaiLejar> Insert(AkTunaiLejar entity)
        {
            await context.AkTunaiLejar.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkTunaiLejar entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
