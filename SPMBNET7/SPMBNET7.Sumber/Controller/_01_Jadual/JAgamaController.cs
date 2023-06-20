using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SPMBNET7.Sumber.Data;
using SPMBNET7.CoreBusiness._Statics;
using SPMBNET7.CoreBusiness.Models.Administrations;
using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.Infrastructure.Interfaces;

namespace SPMBNET7.Sumber.Controller._01_Jadual
{
    [Authorize(Roles = "SuperAdmin,Supervisor")]
    public class JAgamaController : Microsoft.AspNetCore.Mvc.Controller
    {
        public const string modul = "JD001";
        public const string namamodul = "Jadual Agama";

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppLogIRepository<AppLog, int> _appLog;

        public JAgamaController(ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            AppLogIRepository<AppLog, int> appLog)
        {
            _context = context;
            _userManager = userManager;
            _appLog = appLog;
        }

        // GET: JAgama
        public async Task<IActionResult> Index()
        {
            var obj = await _context.JAgama.ToListAsync();

            if (User.IsInRole("SuperAdmin"))
            {
                obj = await _context.JAgama.IgnoreQueryFilters().ToListAsync();
            }

            return View(obj);
        }

        // GET: JAgama/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.JAgama == null)
            {
                return NotFound();
            }

            var jAgama = await _context.JAgama
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jAgama == null)
            {
                return NotFound();
            }

            return View(jAgama);
        }

        // GET: JAgama/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JAgama/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JAgama jAgama, string syscode)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

                    jAgama.UserId = user?.UserName ?? "";
                jAgama.TarMasuk = DateTime.Now;
                jAgama.SuPekerjaMasukId = pekerjaId;

                _context.Add(jAgama);
                _appLog.Insert("Tambah", jAgama.Perihal, "", 0, 0, pekerjaId,modul,syscode,namamodul,user);
                await _context.SaveChangesAsync();
                TempData[SD.Success] = "Data berjaya ditambah..!";
                return RedirectToAction(nameof(Index));
            }
            return View(jAgama);
        }

        // GET: JAgama/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.JAgama == null)
            {
                return NotFound();
            }

            var jAgama = await _context.JAgama.FindAsync(id);
            if (jAgama == null)
            {
                return NotFound();
            }
            return View(jAgama);
        }

        // POST: JAgama/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, JAgama jAgama, string syscode)
        {
            if (id != jAgama.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

                    var objAsal = await _context.JAgama.FirstOrDefaultAsync(x => x.Id == jAgama.Id);
                    var perihalAsal = objAsal.Perihal;
                    jAgama.UserId = objAsal.UserId;
                    jAgama.TarMasuk = objAsal.TarMasuk;
                    jAgama.SuPekerjaMasukId = objAsal.SuPekerjaMasukId;

                    _context.Entry(objAsal).State = EntityState.Detached;

                    jAgama.UserIdKemaskini = user?.UserName ?? "";
                    jAgama.TarKemaskini = DateTime.Now;
                    jAgama.SuPekerjaKemaskiniId = pekerjaId;

                    _context.Update(jAgama);

                    if (perihalAsal != jAgama.Perihal)
                    {
                        _appLog.Insert("Ubah", perihalAsal + " -> " + jAgama.Perihal, jAgama.Id.ToString(), id, 0, pekerjaId,modul,syscode,namamodul,user);
                    }

                    await _context.SaveChangesAsync();
                    TempData[SD.Success] = "Data berjaya diubah..!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JAgamaExists(jAgama.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(jAgama);
        }

        // GET: JAgama/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.JAgama == null)
            {
                return NotFound();
            }

            var jAgama = await _context.JAgama
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jAgama == null)
            {
                return NotFound();
            }

            return View(jAgama);
        }

        // POST: JAgama/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string syscode)
        {
            if (_context.JAgama == null)
            {
                return Problem("Entity set 'ApplicationDbContext.JAgama'  is null.");
            }
            var jAgama = await _context.JAgama.FindAsync(id);
            if (jAgama != null)
            {
                var user = await _userManager.GetUserAsync(User);
                int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;
                jAgama.UserIdKemaskini = user?.UserName ?? "";
                jAgama.TarKemaskini = DateTime.Now;
                jAgama.SuPekerjaKemaskiniId = pekerjaId;

                _context.JAgama.Remove(jAgama);
                _appLog.Insert("Hapus", jAgama.Perihal, jAgama.Id.ToString(), id, 0, pekerjaId, modul, syscode, namamodul, user);
                await _context.SaveChangesAsync();
                TempData[SD.Success] = "Data berjaya dihapuskan..!";
                return RedirectToAction(nameof(Index));
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RollBack(int id, string syscode)
        {
            var obj = await _context.JAgama.IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id);
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

            // Batal operation

            obj.FlHapus = 0;
            obj.UserIdKemaskini = user?.UserName ?? "";
            obj.TarKemaskini = DateTime.Now;
            obj.SuPekerjaKemaskiniId = pekerjaId;

            _context.JAgama.Update(obj);

            //await AddLogAsync("Rollback", obj.Perihal, 0);
            // Batal operation end
            _appLog.Insert("Rollback", obj.Perihal, obj.Id.ToString(), id, 0, pekerjaId, modul, syscode, namamodul, user);

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dikembalikan..!";
            return RedirectToAction(nameof(Index));
        }

        private bool JAgamaExists(int id)
        {
          return (_context.JAgama?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
