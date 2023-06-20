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
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness._Statics;
using SPMBNET7.CoreBusiness.Models.Administrations;
using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.Infrastructure.Interfaces;

namespace SPMBNET7.App.Controller._01_Jadual
{
    [Authorize(Roles = "SuperAdmin , Supervisor")]
    public class JBangsaController : Microsoft.AspNetCore.Mvc.Controller
    {
        public const string modul = "JD003";
        public const string namamodul = "Jadual Bangsa";

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppLogIRepository<AppLog, int> _appLog;

        public JBangsaController(ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            AppLogIRepository<AppLog, int> appLog)
        {
            _context = context;
            _userManager = userManager;
            _appLog = appLog;
        }

        // GET: JBangsa
        public async Task<IActionResult> Index()
        {
            var obj = await _context.JBangsa.ToListAsync();

            if (User.IsInRole("SuperAdmin"))
            {
                obj = await _context.JBangsa.IgnoreQueryFilters().ToListAsync();
            }

            return View(obj);
        }

        // GET: JBangsa/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.JBangsa == null)
            {
                return NotFound();
            }

            var jBangsa = await _context.JBangsa
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jBangsa == null)
            {
                return NotFound();
            }

            return View(jBangsa);
        }

        // GET: JBangsa/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JBangsa/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JBangsa jBangsa, string syscode)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user!.Id).FirstOrDefault()!.SuPekerjaId;

                jBangsa.UserId = user?.UserName ?? "";
                jBangsa.TarMasuk = DateTime.Now;
                jBangsa.SuPekerjaMasukId = pekerjaId;

                _context.Add(jBangsa);
                _appLog.Insert("Tambah", jBangsa.Perihal, jBangsa.Perihal, 0, 0, pekerjaId,modul,syscode,namamodul,user);
                await _context.SaveChangesAsync();
                TempData[SD.Success] = "Data berjaya ditambah..!";
                return RedirectToAction(nameof(Index));
            }
            return View(jBangsa);
        }

        // GET: JBangsa/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.JBangsa == null)
            {
                return NotFound();
            }

            var jBangsa = await _context.JBangsa.FindAsync(id);
            if (jBangsa == null)
            {
                return NotFound();
            }
            return View(jBangsa);
        }

        // POST: JBangsa/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,JBangsa jBangsa, string syscode)
        {
            if (id != jBangsa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user!.Id).FirstOrDefault()!.SuPekerjaId;

                    var objAsal = await _context.JBangsa.FirstOrDefaultAsync(x => x.Id == jBangsa.Id);
                    var perihalAsal = objAsal!.Perihal;
                    jBangsa.UserId = objAsal.UserId;
                    jBangsa.TarMasuk = objAsal.TarMasuk;

                    _context.Entry(objAsal).State = EntityState.Detached;

                    jBangsa.UserIdKemaskini = user?.UserName ?? "";
                    jBangsa.TarKemaskini = DateTime.Now;
                    jBangsa.SuPekerjaKemaskiniId = pekerjaId;
                    _context.Update(jBangsa);

                    if (perihalAsal != jBangsa.Perihal)
                    {
                        _appLog.Insert("Ubah", perihalAsal + " -> " + jBangsa.Perihal, jBangsa.Perihal, id, 0, pekerjaId,modul,syscode,namamodul,user);
                    }
                    await _context.SaveChangesAsync();
                    TempData[SD.Success] = "Data berjaya diubah..!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JBangsaExists(jBangsa.Id))
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
            return View(jBangsa);
        }

        // GET: JBangsa/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.JBangsa == null)
            {
                return NotFound();
            }

            var jBangsa = await _context.JBangsa
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jBangsa == null)
            {
                return NotFound();
            }

            return View(jBangsa);
        }

        // POST: JBangsa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string syscode)
        {
            if (_context.JBangsa == null)
            {
                return Problem("Entity set 'ApplicationDbContext.JBangsa'  is null.");
            }
            var jBangsa = await _context.JBangsa.FindAsync(id);
            if (jBangsa != null)
            {
                var user = await _userManager.GetUserAsync(User);
                int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user!.Id).FirstOrDefault()!.SuPekerjaId;
                jBangsa.UserIdKemaskini = user?.UserName ?? "";
                jBangsa.TarKemaskini = DateTime.Now;
                jBangsa.SuPekerjaKemaskiniId = pekerjaId;

                _context.JBangsa.Remove(jBangsa);
                _appLog.Insert("Hapus", jBangsa.Perihal, jBangsa.Perihal, id, 0, pekerjaId, modul, syscode, namamodul, user);
                await _context.SaveChangesAsync();
                TempData[SD.Success] = "Data berjaya dihapuskan..!";
                return RedirectToAction(nameof(Index));
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RollBack(int id, string syscode)
        {
            var obj = await _context.JBangsa.IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id);
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user!.Id).FirstOrDefault()!.SuPekerjaId;
            obj!.UserIdKemaskini = user?.UserName ?? "";
            obj.TarKemaskini = DateTime.Now;
            obj.SuPekerjaKemaskiniId = pekerjaId;
            // Batal operation

            obj.FlHapus = 0;
            _context.JBangsa.Update(obj);

            // Batal operation end
            _appLog.Insert("RollBack", obj.Perihal, obj.Perihal, id, 0, pekerjaId, modul, syscode, namamodul, user);

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dikembalikan..!";
            return RedirectToAction(nameof(Index));
        }
        private bool JBangsaExists(int id)
        {
          return (_context.JBangsa?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
