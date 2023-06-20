using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.EFRepository._02_Akaun
{
    public class AkNotaDebitKreditBelian1Repository : ListViewIRepository<AkNotaDebitKreditBelian1, int>
    {
        public readonly ApplicationDbContext context;
        public AkNotaDebitKreditBelian1Repository(ApplicationDbContext context)
        {
            this.context=context;
        }

        public async Task Delete(int id)
        {
            var model = await context.AkNotaDebitKreditBelian1.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkNotaDebitKreditBelian1>> GetAll(int akNotaDebitKreditBelianId)
        {
            return await context.AkNotaDebitKreditBelian1.Include(b => b.AkCarta)
                .Where(x => x.AkNotaDebitKreditBelianId == akNotaDebitKreditBelianId)
                .ToArrayAsync();
        }

        public async Task<AkNotaDebitKreditBelian1> GetBy2Id(int akNotaDebitKreditBelianId, int akCartaId)
        {
            return await context.AkNotaDebitKreditBelian1.FirstOrDefaultAsync(x => x.AkNotaDebitKreditBelianId == akNotaDebitKreditBelianId && x.AkCartaId == akCartaId)
                ?? new AkNotaDebitKreditBelian1();
        }

        public async Task<AkNotaDebitKreditBelian1> GetById(int id)
        {
            return await context.AkNotaDebitKreditBelian1.Include(d => d.AkCarta).FirstOrDefaultAsync(d => d.Id == id) ?? new AkNotaDebitKreditBelian1();
        }

        public async Task<AkNotaDebitKreditBelian1> Insert(AkNotaDebitKreditBelian1 entity)
        {
            await context.AkNotaDebitKreditBelian1.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkNotaDebitKreditBelian1 entity)
        {
            AkNotaDebitKreditBelian1 data = await context.AkNotaDebitKreditBelian1.FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (data != null)
                data.Amaun = entity.Amaun;
            await context.SaveChangesAsync();
        }
    }
}
