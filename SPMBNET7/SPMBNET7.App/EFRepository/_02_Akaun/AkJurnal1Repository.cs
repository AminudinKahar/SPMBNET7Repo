﻿using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.EFRepository._02_Akaun
{
    public class AkJurnal1Repository : ListViewIRepository<AkJurnal1, int>
    {
        public readonly ApplicationDbContext context;
        public AkJurnal1Repository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var model = await context.AkJurnal1.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkJurnal1>> GetAll(int id)
        {
            return await context.AkJurnal1
                .Include(b => b.AkCartaDebit)
                .Include(b => b.AkCartaKredit)
                .Include(b => b.JBahagianDebit)
                .Include(b => b.JBahagianKredit)
                .Where(x => x.AkJurnalId == id)
                .ToListAsync();
        }

        public async Task<AkJurnal1> GetBy2Id(int id1, int id2)
        {
            return await context.AkJurnal1.FirstOrDefaultAsync(x => x.AkJurnalId == id1 && x.Id == id2) ?? new AkJurnal1();
        }

        public async Task<AkJurnal1> GetById(int id)
        {
            return await context.AkJurnal1.FindAsync(id) ?? new AkJurnal1();
        }

        public async Task<AkJurnal1> Insert(AkJurnal1 entity)
        {
            await context.AkJurnal1.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkJurnal1 entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
