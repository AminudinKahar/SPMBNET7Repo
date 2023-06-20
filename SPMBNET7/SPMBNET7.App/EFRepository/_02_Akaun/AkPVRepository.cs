using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.EFRepository._02_Akaun
{
    public class AkPVRepository : IRepository<AkPV, int, string>
    {
        public readonly ApplicationDbContext context;

        public AkPVRepository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AkPV.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkPV>> GetAll()
        {
            return await context.AkPV
                .Include(b => b.JKW)
                .Include(b => b.JBank)
                .Include(b => b.JBahagian)
                .Include(b => b.AkPembekal)
                .Include(b => b.SuPekerja)
                .Include(b => b.SpPendahuluanPelbagai)
                .Include(b => b.SuProfil)
                .Include(b => b.AkBank)
                .Include(b => b.JCaraBayar)
                .Include(b => b.AkPV1)
                .Include(b => b.AkPV2)
                .ThenInclude(b => b.AkBelian)
                .Include(b => b.AkPVGanda)
                    .ThenInclude(b => b.SuAtlet)
                .Include(b => b.AkPVGanda)
                    .ThenInclude(b => b.SuPekerja)
                .Include(b => b.AkPVGanda)
                    .ThenInclude(b => b.SuPekerja)
                .Include(b => b.AkPVGanda)
                    .ThenInclude(b => b.JBank)
                .Include(b => b.AkPVGanda)
                    .ThenInclude(b => b.JCaraBayar)
                .Include(b => b.AkPadananPenyata)
                .ToListAsync();
        }

        public async Task<IEnumerable<AkPV>> GetAllIncludeDeletedItems()
        {
            return await context.AkPV
                .IgnoreQueryFilters()
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.JBank)
                .Include(b => b.AkPembekal)
                .Include(b => b.SuPekerja)
                .Include(b => b.SpPendahuluanPelbagai)
                .Include(b => b.SuProfil)
                .Include(b => b.AkBank)
                .Include(b => b.JCaraBayar)
                .Include(b => b.AkPV1)
                    .ThenInclude(b => b.AkCarta)
                .Include(b => b.AkPV2)
                    .ThenInclude(b => b.AkBelian)
                        .ThenInclude(b => b!.AkPO)
                .Include(b => b.AkPV2)
                    .ThenInclude(b => b.AkBelian)
                        .ThenInclude(b => b!.AkInden)
                .Include(b => b.AkPVGanda)
                    .ThenInclude(b => b.SuAtlet)
                .Include(b => b.AkPVGanda)
                    .ThenInclude(b => b.SuPekerja)
                .Include(b => b.AkPVGanda)
                    .ThenInclude(b => b.SuPekerja)
                .Include(b => b.AkPVGanda)
                    .ThenInclude(b => b.JBank)
                .Include(b => b.AkPVGanda)
                    .ThenInclude(b => b.JCaraBayar)
                .Include(b => b.AkPadananPenyata)
                .ToListAsync();
        }

        public async Task<AkPV> GetById(int id)
        {
            return await context.AkPV
                .Include(b => b.JKW)
                .Include(b => b.JBank)
                .Include(b => b.JBahagian)
                .Include(b => b.AkTunaiRuncit).ThenInclude(b => b!.AkCarta)
                .Include(b => b.SpPendahuluanPelbagai).ThenInclude(b => b!.AkCarta)
                .Include(b => b.SuProfil).ThenInclude(b => b!.AkCarta)
                .Include(b => b.AkPembekal).ThenInclude(x => x!.JBank)
                .Include(b => b.SuPekerja).ThenInclude(x => x!.JBank)
                .Include(b => b.AkBank).ThenInclude(b => b!.JBank)
                .Include(b => b.JCaraBayar)
                .Include(b => b.AkPV1)
                    .ThenInclude(b => b.AkCarta)
                .Include(b => b.AkPV2)
                    .ThenInclude(b => b.AkBelian)
                        .ThenInclude(b => b!.AkPO)
                .Include(b => b.AkPV2)
                    .ThenInclude(b => b.AkBelian)
                        .ThenInclude(b => b!.AkInden)
                .Include(b => b.AkPVGanda)
                    .ThenInclude(b => b.SuAtlet)
                .Include(b => b.AkPVGanda)
                    .ThenInclude(b => b.SuPekerja)
                .Include(b => b.AkPVGanda)
                    .ThenInclude(b => b.SuPekerja)
                .Include(b => b.AkPVGanda)
                    .ThenInclude(b => b.JBank)
                .Include(b => b.AkPVGanda)
                    .ThenInclude(b => b.JCaraBayar)
                .Include(b => b.AkPadananPenyata)
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync() ?? new AkPV();
        }

        public async Task<AkPV> GetByIdIncludeDeletedItems(int id)
        {
            return await context.AkPV
                .IgnoreQueryFilters()
                .Include(b => b.JKW)
                .Include(b => b.JBank)
                .Include(b => b.JBahagian)
                .Include(b => b.AkTunaiRuncit).ThenInclude(b => b!.AkCarta)
                .Include(b => b.SpPendahuluanPelbagai).ThenInclude(b => b!.AkCarta)
                .Include(b => b.SuProfil).ThenInclude(b => b!.AkCarta)
                .Include(b => b.AkPembekal).ThenInclude(x => x!.JBank)
                .Include(b => b.SuPekerja).ThenInclude(x => x!.JBank)
                .Include(b => b.AkBank).ThenInclude(b => b!.JBank)
                .Include(b => b.JCaraBayar)
                .Include(b => b.AkPV1)
                    .ThenInclude(b => b.AkCarta)
                .Include(b => b.AkPV2)
                    .ThenInclude(b => b.AkBelian)
                        .ThenInclude(b => b!.AkPO)
                .Include(b => b.AkPV2)
                    .ThenInclude(b => b.AkBelian)
                        .ThenInclude(b => b!.AkInden)
                .Include(b => b.AkPVGanda)
                    .ThenInclude(b => b.SuAtlet)
                .Include(b => b.AkPVGanda)
                    .ThenInclude(b => b.SuPekerja)
                .Include(b => b.AkPVGanda)
                    .ThenInclude(b => b.SuPekerja)
                .Include(b => b.AkPVGanda)
                    .ThenInclude(b => b.JBank)
                .Include(b => b.AkPVGanda)
                    .ThenInclude(b => b.JCaraBayar)
                .Where(b => b.Id == id)
                .Include(b => b.AkPadananPenyata)
                .FirstOrDefaultAsync() ?? new AkPV();
        }

        public Task<AkPV> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<AkPV> Insert(AkPV entity)
        {
            await context.AkPV.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkPV entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
