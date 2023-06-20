using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SPMBNET7.Sumber.Data;
using SPMBNET7.CoreBusiness.Models.Administrations;
using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.Infrastructure.Interfaces.Common;
using SPMBNET7.Infrastructure.Interfaces;
using SPMBNET7.CoreBusiness.Models.Modules._03_Sumber;
using SPMBNET7.CoreBusiness._Statics;

namespace SPMBNET7.Sumber.Controller._01_Jadual
{
    public class JPelulusController : Microsoft.AspNetCore.Mvc.Controller
    {
        public const string syscode = "SPPB";
        public const string modul = "JD010";
        public const string namamodul = "Jadual Pelulus";

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly IRepository<JPelulus, int, string> _pelulusRepo;

        public JPelulusController(ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            AppLogIRepository<AppLog, int> appLog,
            IRepository<JPelulus, int, string> pelulusRepo)
        {
            _context = context;
            _userManager = userManager;
            _appLog = appLog;
            _pelulusRepo = pelulusRepo;
        }

        // GET: JPelulus
        public async Task<IActionResult> Index()
        {
            var obj = await _pelulusRepo.GetAll();

            if (User.IsInRole("SuperAdmin"))
            {
                obj = await _pelulusRepo.GetAllIncludeDeletedItems();
            }

            return View(obj);
        }

        // GET: JPelulus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jPelulus = await _pelulusRepo.GetById((int)id);
            if (jPelulus == null)
            {
                return NotFound();
            }

            PopulateList();
            return View(jPelulus);
        }
        private void PopulateList()
        {
            List<SuPekerja> pekerjaList = _context.SuPekerja.ToList();
            ViewBag.SuPekerja = pekerjaList;
        }
        // GET: JPelulus/Create
        public IActionResult Create()
        {
            PopulateList();
            return View();
        }

        // POST: JPelulus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JPelulus jPelulus)
        {
            JPelulus m = new JPelulus();
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;
            var pekerja = await _context.SuPekerja.FirstOrDefaultAsync(x => x.Id == jPelulus.SuPekerjaId);

            if (IsSuPekerjaExists(jPelulus.SuPekerjaId) == true)
            {
                TempData[SD.Error] = "Pelulus ini telah wujud..!";
                PopulateList();
                return View(jPelulus);
            }

            if (jPelulus.MinAmaun > jPelulus.MaksAmaun)
            {
                TempData[SD.Error] = "Amaun Minimum lebih besar dari Amaun Maksimum..!";
                PopulateList();
                return View(jPelulus);
            }
            if (ModelState.IsValid)
            {
                if (jPelulus != null && jPelulus.SuPekerjaId != 0 && pekerja != null)
                {
                    m.SuPekerjaId = jPelulus.SuPekerjaId;
                    m.MinAmaun = jPelulus.MinAmaun;
                    m.MaksAmaun = jPelulus.MaksAmaun;
                    m.IsBelian = jPelulus.IsBelian;
                    m.IsNotaMinta = jPelulus.IsNotaMinta;
                    m.IsPendahuluan = jPelulus.IsPendahuluan;
                    m.IsPO = jPelulus.IsPO;
                    m.IsPV = jPelulus.IsPV;
                    m.IsInvois = jPelulus.IsInvois;
                    m.IsLaporanBukuVot = jPelulus.IsLaporanBukuVot;
                    m.TarMasuk = DateTime.Now;
                    m.UserId = user?.UserName ?? "";
                    m.SuPekerjaMasukId = pekerjaId;
                    await _pelulusRepo.Insert(m);

                    //insert applog
                    _appLog.Insert("Tambah", pekerja.NoGaji + " - " + pekerja.NoKp, pekerja.NoGaji, 0, 0, pekerjaId,modul,syscode,namamodul,user);
                    //insert applog end

                    await _context.SaveChangesAsync();

                    //CartEmpty();
                    TempData[SD.Success] = "Maklumat Pelulus berjaya ditambah";
                    return RedirectToAction(nameof(Index));
                }
            }
            TempData[SD.Error] = "Sila isi ruangan yang bertanda (*)..!";
            PopulateList();
            return View(jPelulus);
        }

        // GET: JPelulus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.JPelulus == null)
            {
                return NotFound();
            }

            var jPelulus = await _pelulusRepo.GetById((int)id);
            if (jPelulus == null)
            {
                return NotFound();
            }
            PopulateList();
            return View(jPelulus);
        }

        // POST: JPelulus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, JPelulus jPelulus)
        {
            if (id != jPelulus.Id)
            {
                return NotFound();
            }

            if (jPelulus.MinAmaun > jPelulus.MaksAmaun)
            {
                TempData[SD.Error] = "Amaun Minimum lebih besar dari Amaun Maksimum..!";
                PopulateList();
                return View(jPelulus);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;
                    var objAsal = await _context.JPelulus.Include(x => x.SuPekerja).FirstOrDefaultAsync(x => x.Id == id);
                    jPelulus.UserId = objAsal.UserId;
                    jPelulus.TarMasuk = objAsal.TarMasuk;
                    jPelulus.SuPekerjaMasukId = objAsal.SuPekerjaMasukId;

                    jPelulus.SuPekerjaId = objAsal.SuPekerjaId;

                    _context.Entry(objAsal).State = EntityState.Detached;

                    var objPekerja = await _context.SuPekerja.FirstOrDefaultAsync(x => x.Id == jPelulus.SuPekerjaId);

                    jPelulus.UserIdKemaskini = user?.UserName ?? "";
                    jPelulus.TarKemaskini = DateTime.Now;
                    jPelulus.SuPekerjaKemaskiniId = pekerjaId;

                    _context.Update(jPelulus);
                    //insert applog

                    _appLog.Insert("Ubah", objPekerja.NoGaji + " - " + objPekerja.NoKp, objPekerja.NoGaji, id, 0, pekerjaId,modul,syscode,namamodul,user);

                    //insert applog end
                    TempData[SD.Success] = "Data berjaya diubah..!";
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JPelulusExists(jPelulus.Id))
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
            PopulateList();
            return View(jPelulus);
        }

        // GET: JPelulus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.JPelulus == null)
            {
                return NotFound();
            }

            var jPelulus = await _pelulusRepo.GetById((int)id);
            if (jPelulus == null)
            {
                return NotFound();
            }

            return View(jPelulus);
        }

        // POST: JPelulus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.JPelulus == null)
            {
                return Problem("Entity set 'ApplicationDbContext.JPelulus'  is null.");
            }
            var jPelulus = await _pelulusRepo.GetById(id);
            if (jPelulus != null)
            {
                var user = await _userManager.GetUserAsync(User);
                int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;
                jPelulus.UserIdKemaskini = user?.UserName ?? "";
                jPelulus.TarKemaskini = DateTime.Now;
                jPelulus.SuPekerjaKemaskiniId = pekerjaId;

                await _pelulusRepo.Delete(id);
                //insert applog
                _appLog.Insert("Hapus", jPelulus.SuPekerja.NoGaji + " - " + jPelulus.SuPekerja.NoKp, jPelulus.SuPekerja.NoGaji, id, 0, pekerjaId,modul,syscode,namamodul,user);

                //insert applog end

                await _context.SaveChangesAsync();
                TempData[SD.Success] = "Data berjaya dihapuskan..!";
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JPelulusExists(int id)
        {
          return (_context.JPelulus?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private bool IsSuPekerjaExists(int id)
        {
            return _context.JPelulus.Any(e => e.SuPekerjaId == id);
        }
    }
}
