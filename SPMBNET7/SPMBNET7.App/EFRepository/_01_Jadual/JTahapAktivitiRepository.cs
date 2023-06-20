using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.EFRepository._01_Jadual
{
    public class JTahapAktivitiRepository : IRepository<JTahapAktiviti, int, string>
    {
        public readonly ApplicationDbContext context;

        public JTahapAktivitiRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var model = await context.JTahapAktiviti.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<JTahapAktiviti>> GetAll()
        {
            return await context.JTahapAktiviti.ToListAsync();
        }

        public async Task<IEnumerable<JTahapAktiviti>> GetAllIncludeDeletedItems()
        {
            return await context.JTahapAktiviti
                 .IgnoreQueryFilters()
                 .ToListAsync();
        }

        public async Task<JTahapAktiviti> GetById(int id)
        {
            return await context.JTahapAktiviti.FindAsync(id) ?? new JTahapAktiviti();

        }

        public async Task<JTahapAktiviti> GetByIdIncludeDeletedItems(int id)
        {
            return await context.JTahapAktiviti
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id) ?? new JTahapAktiviti();
        }

        public Task<JTahapAktiviti> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<JTahapAktiviti> Insert(JTahapAktiviti entity)
        {
            await context.JTahapAktiviti.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(JTahapAktiviti entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
