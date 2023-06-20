using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Modules._91_Permohonan;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.EFRepository._91_Permohonan
{
    public class SpPendahuluanPelbagaiRepository : IRepository<SpPendahuluanPelbagai, int, string>
    {
        public readonly ApplicationDbContext context;

        public SpPendahuluanPelbagaiRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var model = await context.SpPendahuluanPelbagai.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<SpPendahuluanPelbagai>> GetAll()
        {
            return await context.SpPendahuluanPelbagai
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.JTahapAktiviti)
                .Include(b => b.JSukan)
                .Include(b => b.AkCarta)
                .Include(b => b.JNegeri)
                .Include(b => b.SuPekerja).ThenInclude(b => b!.JCaraBayar)
                .Include(b => b.SpPendahuluanPelbagai1).ThenInclude(b => b.JJantina)
                .Include(b => b.SpPendahuluanPelbagai2)
                .ToListAsync();
        }

        public async Task<IEnumerable<SpPendahuluanPelbagai>> GetAllIncludeDeletedItems()
        {
            return await context.SpPendahuluanPelbagai
                .IgnoreQueryFilters()
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.JTahapAktiviti)
                .Include(b => b.JSukan)
                .Include(b => b.AkCarta)
                .Include(b => b.JNegeri)
                .Include(b => b.SuPekerja).ThenInclude(b => b!.JCaraBayar)
                .Include(b => b.SpPendahuluanPelbagai1).ThenInclude(b => b.JJantina)
                .Include(b => b.SpPendahuluanPelbagai2)
                .ToListAsync();
        }

        public async Task<SpPendahuluanPelbagai> GetById(int id)
        {
            return await context.SpPendahuluanPelbagai
                .Where(d => d.Id == id)
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.JTahapAktiviti)
                .Include(b => b.JSukan)
                .Include(b => b.AkCarta)
                .Include(b => b.JNegeri)
                .Include(b => b.SuPekerja).ThenInclude(b => b!.JCaraBayar)
                .Include(b => b.SpPendahuluanPelbagai1).ThenInclude(b => b.JJantina)
                .Include(d => d.SpPendahuluanPelbagai2)
                .FirstOrDefaultAsync() ?? new SpPendahuluanPelbagai();
        }

        public async Task<SpPendahuluanPelbagai> GetByIdIncludeDeletedItems(int id)
        {
            return await context.SpPendahuluanPelbagai
                .IgnoreQueryFilters()
                .Where(d => d.Id == id)
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.JTahapAktiviti)
                .Include(b => b.JSukan)
                .Include(b => b.AkCarta)
                .Include(b => b.JNegeri)
                .Include(b => b.SuPekerja).ThenInclude(b => b!.JCaraBayar)
                .Include(b => b.SpPendahuluanPelbagai1).ThenInclude(b => b.JJantina)
                .Include(d => d.SpPendahuluanPelbagai2)
                .FirstOrDefaultAsync() ?? new SpPendahuluanPelbagai();
        }

        public Task<SpPendahuluanPelbagai> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<SpPendahuluanPelbagai> Insert(SpPendahuluanPelbagai entity)
        {
            await context.SpPendahuluanPelbagai.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(SpPendahuluanPelbagai entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
