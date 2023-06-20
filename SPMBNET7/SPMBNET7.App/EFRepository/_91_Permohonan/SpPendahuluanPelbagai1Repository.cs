using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Modules._91_Permohonan;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.EFRepository._91_Permohonan
{
    public class SpPendahuluanPelbagai1Repository : ListViewIRepository<SpPendahuluanPelbagai1, int>
    {
        public readonly ApplicationDbContext context;

        public SpPendahuluanPelbagai1Repository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.SpPendahuluanPelbagai1.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<SpPendahuluanPelbagai1>> GetAll(int sPPermohonanAktivitiId)
        {
            return await context.SpPendahuluanPelbagai1
                //.Include(b => b.AkCarta)
                .ToListAsync();
        }

        public Task<SpPendahuluanPelbagai1> GetBy2Id(int id1, int id2)
        {
            throw new NotImplementedException();
        }

        public async Task<SpPendahuluanPelbagai1> GetById(int id)
        {
            return await context.SpPendahuluanPelbagai1.FindAsync(id) ?? new SpPendahuluanPelbagai1();
        }

        public async Task<SpPendahuluanPelbagai1> Insert(SpPendahuluanPelbagai1 entity)
        {
            await context.SpPendahuluanPelbagai1.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(SpPendahuluanPelbagai1 entity)
        {

            SpPendahuluanPelbagai1 data = context.SpPendahuluanPelbagai1.FirstOrDefault(x => x.Id == entity.Id);
            if (data != null)
                data.Jumlah = entity.Jumlah;

            //Tambah kalau ada data dalam field lagi
            await context.SaveChangesAsync();
        }
    }
}
