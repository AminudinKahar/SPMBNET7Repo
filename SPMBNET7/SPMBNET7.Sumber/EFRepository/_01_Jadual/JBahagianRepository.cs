using Microsoft.EntityFrameworkCore;
using SPMBNET7.Sumber.Data;
using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.Sumber.EFRepository._01_Jadual
{
    public class JBahagianRepository : IRepository<JBahagian, int, string>
    {
        private readonly ApplicationDbContext context;
        public JBahagianRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var obj = await context.JBahagian.FirstOrDefaultAsync(b => b.Id == id);
            if (obj != null)
            {
                context.Remove(obj);
            }
        }

        public async Task<IEnumerable<JBahagian>> GetAll()
        {
            return await context.JBahagian
                .Include(x => x.JKW)
                .Include(x => x.JPTJ)
                .ToListAsync();
        }

        public async Task<IEnumerable<JBahagian>> GetAllIncludeDeletedItems()
        {
            return await context.JBahagian
                .IgnoreQueryFilters()
                .Include(x => x.JKW)
                .Include(x => x.JPTJ)
                .ToListAsync();
        }

        public async Task<JBahagian> GetById(int id)
        {
            return await context.JBahagian.Include(x => x.JKW)
                .Include(x => x.JPTJ)
                .FirstOrDefaultAsync(x => x.Id == id) ?? new JBahagian();
        }

        public async Task<JBahagian> GetByIdIncludeDeletedItems(int id)
        {
            return await context.JBahagian
                .IgnoreQueryFilters()
                .Include(x => x.JKW)
                .Include(x => x.JPTJ)
                .FirstOrDefaultAsync(x => x.Id == id) ?? new JBahagian();
        }

        public Task<JBahagian> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<JBahagian> Insert(JBahagian entity)
        {
            await context.JBahagian.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(JBahagian entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
