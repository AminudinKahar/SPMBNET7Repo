using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Modules._03_Sumber;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.EFRepository._03_Sumber
{
    public class SuTanggunganPekerjaRepository : ListViewIRepository<SuTanggunganPekerja, int>
    {
        public readonly ApplicationDbContext context;
        public SuTanggunganPekerjaRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var model = await context.SuTanggunganPekerja.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<SuTanggunganPekerja>> GetAll(int id)
        {
            return await context.SuTanggunganPekerja
                .Where(x => x.SuPekerjaId == id)
                .ToListAsync();
        }

        public async Task<SuTanggunganPekerja> GetBy2Id(int id1, int id2)
        {
            return await context.SuTanggunganPekerja.FirstOrDefaultAsync(x => x.SuPekerjaId == id1 && x.Id == id2) ?? new SuTanggunganPekerja();
        }

        public async Task<SuTanggunganPekerja> GetById(int id)
        {
            return await context.SuTanggunganPekerja.FindAsync(id) ?? new SuTanggunganPekerja();
        }

        public async Task<SuTanggunganPekerja> Insert(SuTanggunganPekerja entity)
        {
            await context.SuTanggunganPekerja.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(SuTanggunganPekerja entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
