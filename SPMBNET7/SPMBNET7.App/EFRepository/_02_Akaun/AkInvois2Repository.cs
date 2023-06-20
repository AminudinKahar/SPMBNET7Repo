using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.EFRepository._02_Akaun
{
    public class AkInvois2Repository : ListViewIRepository<AkInvois2, int>
    {
        public readonly ApplicationDbContext context;

        public AkInvois2Repository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AkInvois2.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkInvois2>> GetAll(int akInvoisId)
        {
            return await context.AkInvois2
                .Where(x => x.AkInvoisId == akInvoisId)
                .ToArrayAsync();
        }

        public async Task<AkInvois2> GetBy2Id(int akInvoisId, int indek)
        {
            return await context.AkInvois2.FirstOrDefaultAsync(x => x.AkInvoisId == akInvoisId && x.Indek == indek) ?? new AkInvois2();
        }

        public async Task<AkInvois2> GetById(int id)
        {
            return await context.AkInvois2.FindAsync(id) ?? new AkInvois2();
        }

        public async Task<AkInvois2> Insert(AkInvois2 entity)
        {
            await context.AkInvois2.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkInvois2 entity)
        {
            AkInvois2 data = context.AkInvois2.FirstOrDefault(x => x.Id == entity.Id);
            if (data != null)
            {
                data.Baris = entity.Baris;
                data.Bil = entity.Bil;
                data.NoStok = entity.NoStok;
                data.Perihal = entity.Perihal;
                data.Kuantiti = entity.Kuantiti;
                data.Unit = entity.Unit;
                data.Harga = entity.Harga;
                data.Amaun = entity.Amaun;
            }
            
            await context.SaveChangesAsync();
        }
    }
}
