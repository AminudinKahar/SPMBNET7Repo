﻿using Microsoft.EntityFrameworkCore;
using SPMBNET7.Sumber.Data;
using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.Sumber.EFRepository._01_Jadual
{
    public class JCaraBayarRepository : IRepository<JCaraBayar, int, string>
    {
        public readonly ApplicationDbContext context;
        public JCaraBayarRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var caraBayar = await context.JCaraBayar.FirstOrDefaultAsync(b => b.Id == id);
            if (caraBayar != null)
            {
                context.Remove(caraBayar);
            }
        }

        public async Task<IEnumerable<JCaraBayar>> GetAll()
        {
            return await context.JCaraBayar.ToListAsync();
        }

        public async Task<IEnumerable<JCaraBayar>> GetAllIncludeDeletedItems()
        {
            return await context.JCaraBayar
                .IgnoreQueryFilters()
                .ToListAsync();
        }

        public async Task<JCaraBayar> GetById(int id)
        {
            return await context.JCaraBayar.FindAsync(id) ?? new JCaraBayar();
        }

        public async Task<JCaraBayar> GetByIdIncludeDeletedItems(int id)
        {
            return await context.JCaraBayar.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == id) ?? new JCaraBayar();
        }

        public Task<JCaraBayar> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<JCaraBayar> Insert(JCaraBayar entity)
        {
            await context.JCaraBayar.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(JCaraBayar entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
