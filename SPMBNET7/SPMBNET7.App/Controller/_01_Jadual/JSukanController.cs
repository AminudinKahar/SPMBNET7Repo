using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness._Statics;
using SPMBNET7.CoreBusiness.Models.Administrations;
using SPMBNET7.CoreBusiness.Models.Modules._03_Sumber;
using SPMBNET7.Infrastructure.Interfaces;

namespace SPMBNET7.App.Controller._01_Jadual
{
    [Authorize(Roles = "SuperAdmin,Supervisor")]
    public class JSukanController : Microsoft.AspNetCore.Mvc.Controller
    {
        public const string modul = "JD008";
        public const string namamodul = "Jadual Sukan";

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppLogIRepository<AppLog, int> _appLog;

        public JSukanController(ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            AppLogIRepository<AppLog, int> appLog)
        {
            _context = context;
            _userManager = userManager;
            _appLog = appLog;
        }

        // GET: JSukan
        public async Task<IActionResult> Index()
        {
            var obj = await _context.JSukan.ToListAsync();

            if (User.IsInRole("SuperAdmin"))
            {
                obj = await _context.JSukan.IgnoreQueryFilters().ToListAsync();
            }

            return View(obj);
        }

        // GET: JSukan/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.JSukan == null)
            {
                return NotFound();
            }

            var jSukan = await _context.JSukan
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jSukan == null)
            {
                return NotFound();
            }

            return View(jSukan);
        }

        // GET: JSukan/Create
        public IActionResult Create()
        {
            return View();
        }

        private bool SukanExists(string perihal)
        {
            return _context.JSukan.Any(e => e.Perihal == perihal);
        }
        // POST: JSukan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JSukan jSukan, string syscode)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user!.Id).FirstOrDefault()!.SuPekerjaId;

                JSukan m = new JSukan();
                if (SukanExists(jSukan.Perihal) == false)
                {
                    if (ModelState.IsValid)
                    {
                        //string noRujukan = GetKod(akJurnal.JKWId);
                        if (jSukan != null)
                        {
                            m.Kod = jSukan.Kod;
                            m.Perihal = jSukan.Perihal?.ToUpper() ?? "";
                            m.IsElit = jSukan.IsElit;
                            m.IsPembangunan = jSukan.IsPembangunan;
                                m.UserId = user?.UserName ?? "";
                            m.TarMasuk = DateTime.Now;
                            m.SuPekerjaMasukId = pekerjaId;

                            //m.SuTanggungan = _cart.Lines1.ToArray();

                            _context.Add(m);

                            //insert applog
                            _appLog.Insert("Tambah", m.Kod + " - " + m.Perihal, m.Perihal, 0, 0, pekerjaId,modul,syscode,namamodul,user);
                            //insert applog end

                            //await AddLogAsync("Tambah", noRujukan, kredit);
                            await _context.SaveChangesAsync();

                            //CartEmpty();
                            TempData[SD.Success] = "Maklumat berjaya ditambah.";
                            return RedirectToAction(nameof(Index));
                        }

                    }
                }
                else
                {
                    TempData[SD.Error] = "Sukan ini telah wujud..!";
                }
            }
            return View(jSukan);
        }

        // GET: JSukan/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.JSukan == null)
            {
                return NotFound();
            }

            var jSukan = await _context.JSukan.FindAsync(id);
            if (jSukan == null)
            {
                return NotFound();
            }
            return View(jSukan);
        }

        // POST: JSukan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, JSukan jSukan, string syscode)
        {
            if (id != jSukan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user!.Id).FirstOrDefault()!.SuPekerjaId;

                    var objAsal = await _context.JSukan.FirstOrDefaultAsync(x => x.Id == jSukan.Id);
                    var perihalAsal = "";

                    if (objAsal != null)
                    {
                        perihalAsal = objAsal.Perihal;

                        jSukan.UserId = objAsal.UserId;
                        jSukan.TarMasuk = objAsal.TarMasuk;
                        jSukan.SuPekerjaMasukId = objAsal.SuPekerjaMasukId;

                        _context.Entry(objAsal).State = EntityState.Detached;
                    }
                    
                    jSukan.Perihal = jSukan.Perihal?.ToUpper() ?? "";
                    jSukan.UserIdKemaskini = user?.UserName ?? "";
                    jSukan.TarKemaskini = DateTime.Now;
                    jSukan.SuPekerjaKemaskiniId = pekerjaId;

                    _context.Update(jSukan);

                    if (perihalAsal != jSukan.Perihal)
                    {
                        _appLog.Insert("Ubah", perihalAsal + " -> " + jSukan.Perihal, jSukan.Perihal, id, 0, pekerjaId,modul,syscode,namamodul,user);
                    }

                    await _context.SaveChangesAsync();
                    TempData[SD.Success] = "Data berjaya diubah..!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JSukanExists(jSukan.Id))
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
            return View(jSukan);
        }

        // GET: JSukan/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.JSukan == null)
            {
                return NotFound();
            }

            var jSukan = await _context.JSukan
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jSukan == null)
            {
                return NotFound();
            }

            return View(jSukan);
        }

        // POST: JSukan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string syscode)
        {
            if (_context.JSukan == null)
            {
                return Problem("Entity set 'ApplicationDbContext.JSukan'  is null.");
            }
            var jSukan = await _context.JSukan.FindAsync(id);
            if (jSukan != null)
            {
                var user = await _userManager.GetUserAsync(User);
                int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user!.Id).FirstOrDefault()!.SuPekerjaId;
                jSukan.UserIdKemaskini = user?.UserName ?? "";
                jSukan.TarKemaskini = DateTime.Now;
                jSukan.SuPekerjaKemaskiniId = pekerjaId;

                _context.JSukan.Remove(jSukan);
                _appLog.Insert("Hapus", jSukan.Perihal, jSukan.Perihal, id, 0, pekerjaId, modul, syscode, namamodul, user);
                await _context.SaveChangesAsync();
                TempData[SD.Success] = "Data berjaya dihapuskan..!";
                return RedirectToAction(nameof(Index));
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RollBack(int id, string syscode)
        {
            var obj = await _context.JSukan.IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id);
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user!.Id).FirstOrDefault()!.SuPekerjaId;

            // Batal operation
            obj!.UserIdKemaskini = user?.UserName ?? "";
            obj.TarKemaskini = DateTime.Now;
            obj.SuPekerjaKemaskiniId = pekerjaId;

            obj.FlHapus = 0;
            _context.JSukan.Update(obj);

            // Batal operation end
            _appLog.Insert("Hapus", obj.Perihal, obj.Perihal, id, 0, pekerjaId, modul, syscode, namamodul, user);

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dikembalikan..!";
            return RedirectToAction(nameof(Index));
        }

        private bool JSukanExists(int id)
        {
          return (_context.JSukan?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
