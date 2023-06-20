using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Modules._91_Permohonan;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.EFRepository._91_Permohonan
{
    public class SpPendahuluanPelbagai2Repository : ListViewIRepository<SpPendahuluanPelbagai2, int>
    {
        public readonly ApplicationDbContext context;

        public SpPendahuluanPelbagai2Repository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.SpPendahuluanPelbagai2.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<SpPendahuluanPelbagai2>> GetAll(int sPPermohonanAktivitiId)
        {
            return await context.SpPendahuluanPelbagai2
                //.Include(b => b.JJantina)
                .Where(x => x.SpPendahuluanPelbagaiId == sPPermohonanAktivitiId)
                .ToListAsync();
        }

        public Task<SpPendahuluanPelbagai2> GetBy2Id(int id1, int id2)
        {
            throw new NotImplementedException();
        }

        public async Task<SpPendahuluanPelbagai2> GetById(int id)
        {
            return await context.SpPendahuluanPelbagai2.FindAsync(id) ?? new SpPendahuluanPelbagai2();
        }

        public async Task<SpPendahuluanPelbagai2> Insert(SpPendahuluanPelbagai2 entity)
        {
            await context.SpPendahuluanPelbagai2.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(SpPendahuluanPelbagai2 entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
