﻿using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.EFRepository._02_Akaun
{
    public class AbWaranRepository : IRepository<AbWaran, int, string>
    {
        public readonly ApplicationDbContext context;

        public AbWaranRepository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AbWaran.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AbWaran>> GetAll()
        {
            return await context.AbWaran
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.AbWaran1).ThenInclude(b => b.AkCarta)
                .Include(b => b.AbWaran1).ThenInclude(b => b.JBahagian)
                .ToListAsync();
        }

        public async Task<IEnumerable<AbWaran>> GetAllIncludeDeletedItems()
        {
            return await context.AbWaran
                .IgnoreQueryFilters()
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.AbWaran1).ThenInclude(b => b.AkCarta)
                .Include(b => b.AbWaran1).ThenInclude(b => b.JBahagian)
                .ToListAsync();
        }

        public async Task<AbWaran> GetById(int id)
        {
            return await context.AbWaran
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.AbWaran1).ThenInclude(b => b.AkCarta)
                .Include(b => b.AbWaran1).ThenInclude(b => b.JBahagian)
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync() ?? new AbWaran();
        }

        public async Task<AbWaran> GetByIdIncludeDeletedItems(int id)
        {
            return await context.AbWaran
                .IgnoreQueryFilters()
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.AbWaran1).ThenInclude(b => b.AkCarta)
                .Include(b => b.AbWaran1).ThenInclude(b => b.JBahagian)
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync() ?? new AbWaran();
        }

        public Task<AbWaran> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<AbWaran> Insert(AbWaran entity)
        {
            await context.AbWaran.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AbWaran entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
