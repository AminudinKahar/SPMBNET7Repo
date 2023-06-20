﻿using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.EFRepository._02_Akaun
{
    public class AkNotaMintaRepository : IRepository<AkNotaMinta, int, string>
    {
        public readonly ApplicationDbContext context;

        public AkNotaMintaRepository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AkNotaMinta.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkNotaMinta>> GetAll()
        {
            return await context.AkNotaMinta
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.AkPembekal)
                .Include(b => b.AkNotaMinta1)
                .Include(b => b.AkNotaMinta2)
                .ToListAsync();
        }

        public async Task<IEnumerable<AkNotaMinta>> GetAllIncludeDeletedItems()
        {
            return await context.AkNotaMinta
                .IgnoreQueryFilters()
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.AkPembekal)
                .Include(b => b.AkNotaMinta1)
                .Include(b => b.AkNotaMinta2)
                .ToListAsync();
        }

        public async Task<AkNotaMinta> GetById(int id)
        {
            return await context.AkNotaMinta
                .Where(d => d.Id == id)
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.AkPembekal).ThenInclude(b => b!.JBank)
                .Include(b => b.AkPembekal).ThenInclude(b => b!.JNegeri)
                .Include(b => b.AkNotaMinta1).ThenInclude(b => b.AkCarta)
                .Include(b => b.AkNotaMinta2)
                .FirstOrDefaultAsync() ?? new AkNotaMinta();
        }

        public async Task<AkNotaMinta> GetByIdIncludeDeletedItems(int id)
        {
            return await context.AkNotaMinta
                .IgnoreQueryFilters()
                .Where(d => d.Id == id)
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.AkPembekal).ThenInclude(b => b!.JBank)
                .Include(b => b.AkPembekal).ThenInclude(b => b!.JNegeri)
                .Include(b => b.AkNotaMinta1).ThenInclude(b => b.AkCarta)
                .Include(b => b.AkNotaMinta2)
                .FirstOrDefaultAsync() ?? new AkNotaMinta();
        }

        public Task<AkNotaMinta> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<AkNotaMinta> Insert(AkNotaMinta entity)
        {
            await context.AkNotaMinta.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkNotaMinta entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
