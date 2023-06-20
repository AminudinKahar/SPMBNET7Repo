﻿using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.EFRepository._02_Akaun
{
    public class AkNotaDebitKreditBelian2Repository : ListViewIRepository<AkNotaDebitKreditBelian2, int>
    {
        public readonly ApplicationDbContext context;
        public AkNotaDebitKreditBelian2Repository(ApplicationDbContext context)
        {
            this.context=context;
        }

        public async Task Delete(int id)
        {
            var model = await context.AkNotaDebitKreditBelian2.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkNotaDebitKreditBelian2>> GetAll(int akNotaDebitKreditBelianId)
        {
            return await context.AkNotaDebitKreditBelian2
                .Where(x => x.AkNotaDebitKreditBelianId == akNotaDebitKreditBelianId)
                .ToArrayAsync();
        }

        public async Task<AkNotaDebitKreditBelian2> GetBy2Id(int akNotaDebitKreditBelianId, int indek)
        {
            return await context.AkNotaDebitKreditBelian2.FirstOrDefaultAsync(x => x.AkNotaDebitKreditBelianId == akNotaDebitKreditBelianId
                && x.Indek == indek) ?? new AkNotaDebitKreditBelian2();
        }

        public async Task<AkNotaDebitKreditBelian2> GetById(int id)
        {
            return await context.AkNotaDebitKreditBelian2.FindAsync(id) ?? new AkNotaDebitKreditBelian2();
        }

        public async Task<AkNotaDebitKreditBelian2> Insert(AkNotaDebitKreditBelian2 entity)
        {
            await context.AkNotaDebitKreditBelian2.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkNotaDebitKreditBelian2 entity)
        {
            AkNotaDebitKreditBelian2 data = context.AkNotaDebitKreditBelian2.FirstOrDefault(x => x.Id == entity.Id);
            
            if (data != null)
            {
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
