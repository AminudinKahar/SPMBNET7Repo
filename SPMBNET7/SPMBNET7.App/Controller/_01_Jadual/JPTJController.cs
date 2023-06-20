using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Administrations;
using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.Infrastructure.Interfaces.Common;
using SPMBNET7.Infrastructure.Interfaces;
using System.Security.Claims;
using SPMBNET7.CoreBusiness._Statics;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace SPMBNET7.App.Controller._01_Jadual
{
    [Authorize(Roles = "SuperAdmin,Supervisor")]
    public class JPTJController : Microsoft.AspNetCore.Mvc.Controller
    {
        public const string modul = "JD002";
        public const string namamodul = "Jadual Bahagian";

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<JPTJ, int, string> _jPtjRepo;
        private readonly AppLogIRepository<AppLog, int> _appLog;

        public JPTJController(ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            IRepository<JPTJ, int, string> jPtjRepo,
            AppLogIRepository<AppLog, int> appLog)
        {
            _context = context;
            _userManager = userManager;
            _jPtjRepo = jPtjRepo;
            _appLog = appLog;
        }

        // GET: JPTJ
        public async Task<IActionResult> Index()
        {
            var obj = await _jPtjRepo.GetAll();

            if (User.IsInRole("SuperAdmin"))
            {
                obj = await _jPtjRepo.GetAllIncludeDeletedItems();
            }
            return View(obj);
        }

        // GET: JPTJ/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.JPTJ == null)
            {
                return NotFound();
            }

            var jPTJ = await _jPtjRepo.GetById((int)id);
            if (jPTJ == null)
            {
                return NotFound();
            }

            return View(jPTJ);
        }

        // GET: JPTJ/Create
        public IActionResult Create()
        {
            // get latest no rujukan running number  
            var kw = _context.JKW.FirstOrDefault();

            string prefix = kw!.Kod.Substring(0,1);
            int x = 1;
            string noRujukan = prefix + "00";

            var LatestNoRujukan = _context.JPTJ
                        .IgnoreQueryFilters()
                        .Where(x => x.JKW!.Kod == kw.Kod)
                        .Max(x => x.Kod);

            if (LatestNoRujukan == null)
            {
                noRujukan = string.Format("{0:" + prefix + "00}", x);
            }
            else
            {
                x = int.Parse(LatestNoRujukan.Substring(1));
                x++;
                noRujukan = string.Format("{0:" + prefix + "00}", x);
            }

            // get latest no rujukan running number end
            ViewBag.NoRujukan = noRujukan;

            List<JKW> list = _context.JKW.ToList();

            ViewBag.JKw = list;
            return View();
        }

        [HttpPost]
        public JsonResult JsonGetKod(int data)
        {
            try
            {
                var result = "";
                if (data == 0)
                {
                    result = "";
                }
                else
                {
                    // get latest no rujukan running number  
                    var kw = _context.JKW.FirstOrDefault();

                    string prefix = kw!.Kod.Substring(0, 1);
                    int x = 1;
                    string noRujukan = prefix + "00";

                    var LatestNoRujukan = _context.JPTJ
                                .IgnoreQueryFilters()
                                .Where(x => x.JKW!.Kod == kw.Kod)
                                .Max(x => x.Kod);

                    if (LatestNoRujukan == null)
                    {
                        noRujukan = string.Format("{0:" + prefix + "00}", x);
                    }
                    else
                    {
                        x = int.Parse(LatestNoRujukan.Substring(1));
                        x++;
                        noRujukan = string.Format("{0:" + prefix + "00}", x);
                    }

                    result = noRujukan;
                }
                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }
        // POST: JPTJ/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JPTJ jPTJ, string syscode)
        {
            JPTJ m = new JPTJ();
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user!.Id).FirstOrDefault()!.SuPekerjaId;

            var username = user!.UserName;

            // get latest no rujukan running number  
            var kw = _context.JKW.FirstOrDefault();

            string prefix = kw!.Kod.Substring(0, 1);
            int x = 1;
            string noRujukan = prefix + "00";

            var LatestNoRujukan = _context.JPTJ
                        .IgnoreQueryFilters()
                        .Where(x => x.JKW!.Kod == kw.Kod)
                        .Max(x => x.Kod);

            if (LatestNoRujukan == null)
            {
                noRujukan = string.Format("{0:" + prefix + "00}", x);
            }
            else
            {
                x = int.Parse(LatestNoRujukan.Substring(1));
                x++;
                noRujukan = string.Format("{0:" + prefix + "00}", x);
            }

            if (jPTJ != null && jPTJ.JKWId != 0)
            {
                if (ModelState.IsValid)
                {
                    m.JKWId = jPTJ.JKWId;
                    m.Kod = noRujukan;
                    m.Perihal = jPTJ?.Perihal?.ToUpper() ?? "";
                    m.UserId = user?.UserName ?? "";
                    m.TarMasuk = DateTime.Now;
                    m.SuPekerjaMasukId = pekerjaId;

                    _context.JPTJ.Add(m);
                    _appLog.Insert("Tambah", m.Kod + " - " + m.Perihal, m.Kod, 0, 0, pekerjaId, modul, syscode, namamodul, user);
                    await _context.SaveChangesAsync();
                    TempData[SD.Success] = "Data berjaya ditambah..!";
                    return RedirectToAction(nameof(Index));
                }
            }
            List<JKW> list = _context.JKW.ToList();
            ViewBag.NoRujukan = noRujukan;
            ViewBag.JKw = list;
            return View(jPTJ);
        }

        // GET: JPTJ/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.JPTJ == null)
            {
                return NotFound();
            }

            var jPTJ = await _context.JPTJ.FindAsync(id);
            if (jPTJ == null)
            {
                return NotFound();
            }
            List<JKW> list = _context.JKW.ToList();
            List<JPTJ> ptjList = _context.JPTJ.ToList();

            ViewBag.JKw = list;
            ViewBag.JPtj = ptjList;
            return View(jPTJ);
        }

        // POST: JPTJ/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, JPTJ jPTJ, string syscode)
        {
            if (id != jPTJ.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user!.Id).FirstOrDefault()!.SuPekerjaId;

                    JPTJ jPtjAsal = await _context.JPTJ.FindAsync(id);

                    var perihalAsal = "";
                    if (jPtjAsal != null)
                    {
                        // list of input that cannot be change
                        jPTJ.JKWId = jPtjAsal.JKWId;
                        jPTJ.Kod = jPtjAsal.Kod;
                        jPTJ.TarMasuk = jPtjAsal.TarMasuk;
                        jPTJ.UserId = jPtjAsal.UserId;
                        perihalAsal = jPtjAsal.Perihal;
                        // list of input that cannot be change end
                        _context.Entry(jPtjAsal).State = EntityState.Detached;

                    }

                    jPTJ.Perihal = jPTJ.Perihal?.ToUpper()?? "";

                    jPTJ.UserIdKemaskini = user?.UserName ?? "";
                    jPTJ.TarKemaskini = DateTime.Now;
                    jPTJ.SuPekerjaKemaskiniId = pekerjaId;

                    _context.Update(jPTJ);

                    if (perihalAsal != jPTJ.Perihal)
                    {
                        _appLog.Insert("Ubah", perihalAsal + " -> " + jPTJ.Perihal, jPTJ.Kod, id, 0, pekerjaId, modul, syscode, namamodul, user);
                    }
                    else
                    {
                        _appLog.Insert("Ubah", "Ubah Data", jPTJ.Kod, id, 0, pekerjaId, modul, syscode, namamodul, user);
                    }

                    await _context.SaveChangesAsync();
                    TempData[SD.Success] = "Data berjaya diubah..!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JPTJExists(jPTJ.Id))
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
            List<JKW> list = _context.JKW.ToList();

            ViewBag.JKw = list;
            return View(jPTJ);
        }

        // GET: JPTJ/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.JPTJ == null)
            {
                return NotFound();
            }

            var jPTJ = await _jPtjRepo.GetById((int)id);
            if (jPTJ == null)
            {
                return NotFound();
            }

            return View(jPTJ);
        }

        // POST: JPTJ/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string syscode)
        {
            var jPtj = await _context.JPTJ.FindAsync(id);

            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user!.Id).FirstOrDefault()!.SuPekerjaId;
            jPtj!.UserIdKemaskini = user?.UserName ?? "";
            jPtj.TarKemaskini = DateTime.Now;
            jPtj.SuPekerjaKemaskiniId = pekerjaId;

            _context.JPTJ.Remove(jPtj);
            _appLog.Insert("Hapus", jPtj.Kod + " - " + jPtj.Perihal, jPtj.Kod, id, 0, pekerjaId, modul, syscode, namamodul, user);
            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dihapuskan..!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RollBack(int id, string syscode)
        {
            var obj = await _jPtjRepo.GetByIdIncludeDeletedItems(id);
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user!.Id).FirstOrDefault()!.SuPekerjaId;
            obj.UserIdKemaskini = user?.UserName ?? "";
            obj.TarKemaskini = DateTime.Now;
            obj.SuPekerjaKemaskiniId = pekerjaId;
            // Batal operation

            obj.FlHapus = 0;
            _context.JPTJ.Update(obj);

            //await AddLogAsync("Rollback", obj.Kod + " - " + obj.Perihal, 0);
            // Batal operation end
            _appLog.Insert("Rollback", obj.Kod + " - " + obj.Perihal, obj.Kod, id, 0, pekerjaId, modul, syscode, namamodul, user);

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dikembalikan..!";
            return RedirectToAction(nameof(Index));
        }

        private bool JPTJExists(int id)
        {
          return (_context.JPTJ?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
