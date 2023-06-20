using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.EFRepository._02_Akaun
{
    public class AkNotaMinta2Repository : ListViewIRepository<AkNotaMinta2, int>
    {
        public readonly ApplicationDbContext context;

        public AkNotaMinta2Repository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AkNotaMinta2.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkNotaMinta2>> GetAll(int akNotaMintaId)
        {
            return await context.AkNotaMinta2
                .Where(x => x.AkNotaMintaId == akNotaMintaId)
                .ToArrayAsync();
        }

        public async Task<AkNotaMinta2> GetBy2Id(int akNotaMintaId, int indek)
        {
            return await context.AkNotaMinta2.FirstOrDefaultAsync(x => x.AkNotaMintaId == akNotaMintaId && x.Indek == indek) ?? new AkNotaMinta2();
        }

        public async Task<AkNotaMinta2> GetById(int id)
        {
            return await context.AkNotaMinta2.FindAsync(id) ?? new AkNotaMinta2();
        }

        public async Task<AkNotaMinta2> Insert(AkNotaMinta2 entity)
        {
            await context.AkNotaMinta2.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkNotaMinta2 entity)
        {
            AkNotaMinta2 data = context.AkNotaMinta2.FirstOrDefault(x => x.Id == entity.Id);
            data!.Bil = entity.Bil;
            data.NoStok = entity.NoStok;
            data.Perihal = entity.Perihal;
            data.Kuantiti = entity.Kuantiti;
            data.Unit = entity.Unit;
            data.Harga = entity.Harga;
            data.Amaun = entity.Amaun;
            await context.SaveChangesAsync();
        }
    }
}
