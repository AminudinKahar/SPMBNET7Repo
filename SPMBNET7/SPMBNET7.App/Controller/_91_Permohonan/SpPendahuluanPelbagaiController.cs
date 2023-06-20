﻿using System;
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
using SPMBNET7.CoreBusiness.Models.Administrations;
using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.CoreBusiness.Models.Modules._03_Sumber;
using SPMBNET7.CoreBusiness.Models.Modules._91_Permohonan;
using SPMBNET7.Infrastructure.Carts._91_Permohonan;
using SPMBNET7.Infrastructure.Interfaces.Common;
using SPMBNET7.Infrastructure.Interfaces;
using SPMBNET7.CoreBusiness._Statics;
using SPMBNET7.Infrastructure.Math;
using SPMBNET7.App.Pages.PrintModels._91_Permohonan;
using Rotativa.AspNetCore;

namespace SPMBNET7.App.Controller._91_Permohonan
{
    [Authorize(Roles = Init.allExceptAdminRole)]
    public class SpPendahuluanPelbagaiController : Microsoft.AspNetCore.Mvc.Controller
    {
        public const string modul = "SP001";
        public const string namamodul = "Pendahuluan Pelbagai";

        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly IRepository<SpPendahuluanPelbagai, int, string> _spPendahuluanPelbagaiRepo;
        private readonly ListViewIRepository<SpPendahuluanPelbagai1, int> _spPendahuluanPelbagai1Repo;
        private readonly ListViewIRepository<SpPendahuluanPelbagai2, int> _spPendahuluanPelbagai2Repo;
        private readonly IRepository<JNegeri, int, string> _negeriRepo;
        private readonly IRepository<JSukan, int, string> _sukanRepo;
        private readonly IRepository<JJantina, int, string> _jantinaRepo;
        private readonly IRepository<SuPekerja, int, string> _suPekerjaRepo;
        private readonly IRepository<JTahapAktiviti, int, string> _tahapAktivitiRepo;
        private readonly IRepository<JBahagian, int, string> _bahagianRepo;
        private readonly IRepository<AkCarta, int, string> _akCartaRepo;
        private readonly IRepository<JKW, int, string> _kwRepo;
        private readonly IRepository<AbBukuVot, int, string> _abBukuVotRepo;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly CustomIRepository<string, int> _customRepo;
        private readonly IRepository<JPelulus, int, string> _pelulusRepo;
        private CartPendahuluan _cart;

        public SpPendahuluanPelbagaiController(
           ApplicationDbContext context,
           AppLogIRepository<AppLog, int> appLog,
           IRepository<SpPendahuluanPelbagai, int, string> SpPendahuluanPelbagaiRepository,
           ListViewIRepository<SpPendahuluanPelbagai1, int> SpPendahuluanPelbagai1Repository,
           ListViewIRepository<SpPendahuluanPelbagai2, int> SpPendahuluanPelbagai2Repository,
           IRepository<JNegeri, int, string> negeriRepository,
           IRepository<JSukan, int, string> sukanRepository,
           IRepository<JJantina, int, string> jantinaRepository,
           IRepository<SuPekerja, int, string> suPekerjaRepository,
           IRepository<JTahapAktiviti, int, string> tahapAktivitiRepository,
           IRepository<JBahagian, int, string> bahagianRepository,
           IRepository<AkCarta, int, string> akCartaRepository,
           IRepository<JKW, int, string> kwRepository,
           IRepository<AbBukuVot, int, string> abBukuVotRepository,
           UserManager<IdentityUser> userManager,
           CustomIRepository<string, int> customRepo,
           IRepository<JPelulus, int, string> pelulusRepo,
           CartPendahuluan cart
           )
        {
            _appLog = appLog;
            _spPendahuluanPelbagaiRepo = SpPendahuluanPelbagaiRepository;
            _spPendahuluanPelbagai1Repo = SpPendahuluanPelbagai1Repository;
            _spPendahuluanPelbagai2Repo = SpPendahuluanPelbagai2Repository;
            _kwRepo = kwRepository;
            _akCartaRepo = akCartaRepository;
            _context = context;
            _negeriRepo = negeriRepository;
            _sukanRepo = sukanRepository;
            _jantinaRepo = jantinaRepository;
            _suPekerjaRepo = suPekerjaRepository;
            _tahapAktivitiRepo = tahapAktivitiRepository;
            _bahagianRepo = bahagianRepository;
            _abBukuVotRepo = abBukuVotRepository;
            _userManager = userManager;
            _customRepo = customRepo;
            _pelulusRepo = pelulusRepo;
            _cart = cart;
        }

        //Function Running Number
        private string RunningNumber(SpPendahuluanPelbagai data)
        {
            var ptj = _context.JPTJ.Include(b => b.JKW).FirstOrDefault(x => x.JKWId == data.JKWId)!;

            var kumpulanWang = ptj.JKW!.Kod.Substring(0,1) + ptj.Kod;
            var year = DateTime.Now.Year.ToString();
            //var year = data.Tahun;
            string prefix = year + "/" + kumpulanWang + "/";
            int x = 1;
            string noRujukan = prefix + "000000";

            var LatestNoRujukan = _context.SpPendahuluanPelbagai
                .IgnoreQueryFilters()
                .Where(x => x.NoPermohonan.Substring(0, 9) == prefix)
                .Max(x => x.NoPermohonan);
            if (LatestNoRujukan == null)
            {
                noRujukan = string.Format("{0:" + prefix + "000000}", x);
            }
            else
            {
                x = int.Parse(LatestNoRujukan.Substring(12));
                x++;
                noRujukan = string.Format("{0:" + prefix + "000000}", x);
            }
            return noRujukan;
        }

        [HttpPost]
        public JsonResult JsonGetKod(SpPendahuluanPelbagai data)
        {
            try
            {
                var result = "";
                if (data == null)
                {
                    result = "";
                }
                else
                {
                    result = RunningNumber(data);
                }
                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }
        //End Function Running Number

        //Start Function Get Baki Vot
        [HttpPost]
        public async Task<JsonResult> GetBakiVot(SpPendahuluanPelbagai spPendahuluanPelbagai,
            int jKWId,
            int jBahagianId)
        {

            try
            {

                // check for baki peruntukan
                var tahun = DateTime.Now.Year.ToString();
                bool IsExistAbBukuVot = await _context.AbBukuVot
                        .Where(x => x.Tahun == tahun && x.VotId == spPendahuluanPelbagai.AkCartaId && x.JKWId == jKWId && x.JBahagianId == jBahagianId)
                        .AnyAsync();

                if (IsExistAbBukuVot == true)
                {
                    decimal sum = await _customRepo.GetBalanceFromAbBukuVot(tahun, spPendahuluanPelbagai.AkCartaId, jKWId, jBahagianId);

                    if (sum < spPendahuluanPelbagai.JumKeseluruhan)
                    {
                        return Json(new { result = "ERROR" });
                    }
                }
                else
                {
                    return Json(new { result = "ERROR" });
                }
                // check for baki peruntukan end

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }

        }
        //End Function Get Baki Vot

        //Start Function Get Baki Amaun Vot
        [HttpPost]
        public async Task<JsonResult> GetBakiAmaunVot(int akCartaId, int jKWId, int jBahagianId)
        {

            try
            {

                // check for baki peruntukan
                var tahun = DateTime.Now.Year.ToString();
                bool IsExistAbBukuVot = await _context.AbBukuVot
                        .Where(x => x.Tahun == tahun && x.VotId == akCartaId && x.JKWId == jKWId && x.JBahagianId == jBahagianId)
                        .AnyAsync();

                if (IsExistAbBukuVot == true)
                {
                    var sum = await _customRepo.GetBalanceFromAbBukuVot(tahun, akCartaId, jKWId, jBahagianId);

                    return Json(new { result = "OK", record = sum });
                }
                else
                {
                    return Json(new { result = "ERROR" });
                }
                // check for baki peruntukan end


            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }

        }
        //End Function Get Baki Amaun Vot

        //Start Function Get Id Bahagian
        [HttpPost]
        public JsonResult GetBahagian(JBahagian jBahagian)
        {
            try
            {
                var result = _context.JBahagian.Where(b => b.Id == jBahagian.Id).FirstOrDefault();

                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }

        }
        //End Function Get Id Bahagian

        //Start Function Get Id Pemohon/Penyedia
        [HttpPost]
        public JsonResult GetPekerja(SuPekerja suPekerja)
        {
            try
            {
                var result = _context.SuPekerja.Where(b => b.Id == suPekerja.Id).FirstOrDefault();

                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }

        }
        //End Function Get Id Pemohon/Penyedia

        //Start Function Get Jantina Id
        [HttpPost]
        public JsonResult GetJantina(JJantina jJantina)
        {
            try
            {
                var result = _context.JJantina.Where(b => b.Id == jJantina.Id).FirstOrDefault();

                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }

        }
        //End Function Get Jantina Id

        //Function Cart Empty
        public JsonResult CartEmpty()
        {
            try
            {
                ViewBag.spPendahuluanPelbagai1 = new List<int>();
                ViewBag.spPendahuluanPelbagai2 = new List<int>();
                _cart.Clear1();
                _cart.Clear2();

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        //Function Cart Empty end

        private void PopulateList(int? pekerjaId)
        {
            var user = _context.applicationUsers.Include(x => x.SuPekerja).FirstOrDefault(x => x.UserName == User.Identity!.Name);

            List<JKW> kwList = _context.JKW.OrderBy(b => b.Kod).ToList();
            ViewBag.JKW = kwList;

            List<JNegeri> negeriList = _context.JNegeri.OrderBy(b => b.Kod).ToList();
            ViewBag.JNegeri = negeriList;

            List<JSukan> sukanList = _context.JSukan.OrderBy(b => b.Id).ToList();
            ViewBag.JSukan = sukanList;

            List<JJantina> jantinaList = _context.JJantina.OrderBy(b => b.Id).ToList();
            ViewBag.JJantina = jantinaList;

            ViewBag.JTahapAktiviti = _context.JTahapAktiviti.ToList();

            string[] arr = user!.JBahagianList.Split(',');

            List<JBahagian> bahagianList = new List<JBahagian>();

            if (user.UserName != Init.superAdminEmail)
            {
                foreach (var item in arr)
                {
                    var bahagian = _context.JBahagian.FirstOrDefault(x => x.Id == int.Parse(item));

                    if (bahagian != null) bahagianList.Add(bahagian);

                }
            }
            else
            {
                bahagianList.AddRange(_context.JBahagian.ToList());
            }
            

            ViewBag.JBahagian = bahagianList;

            var pekerja = _context.SuPekerja.FirstOrDefault(b => b.Id == pekerjaId);

            if (User.IsInRole("SuperAdmin"))
            {
                ViewBag.IdPekerja = 1;
                ViewBag.NamaPekerja = "SuperAdmin";
            }
            else
            {
                ViewBag.IdPekerja = pekerjaId;
                ViewBag.NamaPekerja = pekerja?.Nama ?? "";
            }

            List<AkCarta> akCartaList = _context.AkCarta
                .Include(b => b.JKW)
                .Include(b => b.JParas)
                .Where(b => b.JParas!.Kod == "4")
                .OrderBy(b => b.Kod)
                .ToList();

            ViewBag.AkCarta = akCartaList;
        }

        private void PopulateTable(int? id)
        {

            List<SpPendahuluanPelbagai1> spPendahuluanPelbagai1 = _context.SpPendahuluanPelbagai1
                //.Include(b => b.AkCarta)
                .Where(b => b.SpPendahuluanPelbagaiId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.spPendahuluanPelbagai1 = spPendahuluanPelbagai1;

            List<SpPendahuluanPelbagai2> spPendahuluanPelbagai2 = _context.SpPendahuluanPelbagai2
                //.Include(b => b.AkCarta)
                .Where(b => b.SpPendahuluanPelbagaiId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.spPendahuluanPelbagai2 = spPendahuluanPelbagai2;
        }

        //Function PopulateCartfromDb guna utk Edit
        private void PopulateCartFromDb(SpPendahuluanPelbagai spPendahuluanPelbagai)
        {
            List<SpPendahuluanPelbagai1> spPendahuluanPelbagai1Table = _context.SpPendahuluanPelbagai1
                .Include(b => b.JJantina)
                .Where(b => b.SpPendahuluanPelbagaiId == spPendahuluanPelbagai.Id)
                .OrderBy(b => b.Id)
                .ToList();
            foreach (SpPendahuluanPelbagai1 spPendahuluanPelbagai1 in spPendahuluanPelbagai1Table)
            {
                _cart.AddItem1(spPendahuluanPelbagai1.SpPendahuluanPelbagaiId,
                               spPendahuluanPelbagai1.JJantinaId,
                               spPendahuluanPelbagai1.BilAtl,
                               spPendahuluanPelbagai1.BilJul,
                               spPendahuluanPelbagai1.BilPeg,
                               spPendahuluanPelbagai1.BilTek,
                               spPendahuluanPelbagai1.BilUru,
                               spPendahuluanPelbagai1.Jumlah);
            }

            ViewBag.spPendahuluanPelbagai1 = spPendahuluanPelbagai1Table;

            List<SpPendahuluanPelbagai2> spPendahuluanPelbagai2Table = _context.SpPendahuluanPelbagai2
                //.Include(b => b.Indek)
                .Where(b => b.SpPendahuluanPelbagaiId == spPendahuluanPelbagai.Id)
                .OrderBy(b => b.Id)
                .ToList();
            foreach (SpPendahuluanPelbagai2 spPendahuluanPelbagai2 in spPendahuluanPelbagai2Table)
            {
                _cart.AddItem2(spPendahuluanPelbagai2.SpPendahuluanPelbagaiId,
                               spPendahuluanPelbagai2.Indek,
                               spPendahuluanPelbagai2.Baris,
                               spPendahuluanPelbagai2.Perihal ?? "",
                               spPendahuluanPelbagai2.Kadar,
                               spPendahuluanPelbagai2.Bil,
                               spPendahuluanPelbagai2.Bulan,
                               spPendahuluanPelbagai2.Jumlah);
            }

            ViewBag.spPendahuluanPelbagai2 = spPendahuluanPelbagai2Table;
        }
        //Function PopulateCartfromDb end

        // populate table from cart ( used when user prompt error when submit form)
        private void PopulateTableFromCart()
        {
            // table 1
            List<SpPendahuluanPelbagai1> sp1 = new List<SpPendahuluanPelbagai1>();
            var cart1 = _cart.Lines1.ToList();
            
            if (cart1 != null && cart1.Count() > 0)
            {
                foreach (var item in cart1)
                {
                    sp1.Add(item);
                }
            }
            ViewBag.spPendahuluanPelbagai1 = sp1;
            // table 1 end

            // table 2
            List<SpPendahuluanPelbagai2> sp2 = new List<SpPendahuluanPelbagai2>();
            var cart2 = _cart.Lines2.ToList();

            if (cart2 != null && cart2.Count() > 0)
            {
                foreach (var item in cart2)
                {
                    sp2.Add(item);
                }
            }
            ViewBag.spPendahuluanPelbagai2 = sp2;
            // table 2 end
        }
        // populate table from cart end
        // GET: SpPendahuluanPelbagai
        [Authorize(Policy = "SP001")]
        public async Task<IActionResult> Index(
             string searchString,
             string searchDate1,
             string searchDate2,
             string searchColumn)
        {
            List<SelectListItem> columnList = new()
            {
                new SelectListItem() { Text = "Tarikh", Value = "Tarikh" },
                new SelectListItem() { Text = "No Permohonan", Value = "NoRujukan" },
                new SelectListItem() { Text = "Nama Pemohon", Value = "Nama" }
            };

            if (!string.IsNullOrEmpty(searchColumn))
            {
                ViewBag.SearchColumn = new SelectList(columnList, "Value", "Text", searchColumn);
            }
            else
            {
                ViewBag.SearchColumn = new SelectList(columnList, "Value", "Text", "");
            }

            var user = _context.applicationUsers.Include(x => x.SuPekerja).FirstOrDefault(x => x.UserName == User.Identity!.Name)!;

            var searchResult = await _spPendahuluanPelbagaiRepo.GetAll();

            if (User.IsInRole("SuperAdmin") || User.IsInRole("Supervisor"))
            {
                searchResult = await _spPendahuluanPelbagaiRepo.GetAllIncludeDeletedItems();
            }
            else
            {
                searchResult = searchResult.Where(b => b.SuPekerjaId == user.SuPekerjaId).ToList();
            }

            if (!string.IsNullOrEmpty(searchString) || (!string.IsNullOrEmpty(searchDate1) && !string.IsNullOrEmpty(searchDate2)))
            {
                // searching with '%like%' condition
                if (!string.IsNullOrEmpty(searchString))
                {
                    if (searchColumn == "NoRujukan")
                    {
                        searchResult = searchResult.Where(s => s.NoPermohonan.ToUpper().Contains(searchString.ToUpper())).ToList();
                    }
                    else if (searchColumn == "Nama")
                    {
                        searchResult = searchResult.Where(s => s.SuPekerja!.Nama.ToUpper().Contains(searchString.ToUpper())).ToList();
                    }

                    ViewBag.SearchData1 = searchString;

                }

                // searching with '%like%' condition end

                // searching with date range condition
                if (!string.IsNullOrEmpty(searchDate1) && !string.IsNullOrEmpty(searchDate2))
                {
                    if (searchColumn == "Tarikh")
                    {
                        DateTime date1 = DateTime.Parse(searchDate1);
                        DateTime date2 = DateTime.Parse(searchDate2).AddHours(23.99);
                        searchResult = searchResult.Where(x => x.TarSedia >= date1
                            && x.TarSedia <= date2).ToList();
                    }
                    ViewBag.SearchData1 = searchDate1;
                    ViewBag.SearchData2 = searchDate2;
                }

                ViewBag.SearchColumn = new SelectList(columnList, "Value", "Text", searchColumn);
            }
            // searching with date range condition end
            else
            {
                ViewBag.SearchColumn = new SelectList(columnList, "Value", "Text", "Tarikh");
            }

            List<JPenyemak> penyemak = _context.JPenyemak
                .Include(x => x.SuPekerja)
                .Where(x => x.IsPendahuluan == true).OrderBy(b => b.SuPekerja!.Nama).ToList();
            ViewBag.JPenyemak = penyemak;

            List<JPelulus> pelulus = _context.JPelulus
                .Include(x => x.SuPekerja)
                .Where(x => x.IsPendahuluan == true).OrderBy(b => b.SuPekerja!.Nama).ToList();
            ViewBag.JPelulus = pelulus;

            return View(searchResult);
        }

        // GET: SpPendahuluanPelbagai/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SpPendahuluanPelbagai == null)
            {
                return NotFound();
            }

            var spPendahuluanPelbagai = await _spPendahuluanPelbagaiRepo.GetById((int)id);
            //var kw = await _kwRepo.GetById(spPendahuluanPelbagai.JKWId);
            //spPendahuluanPelbagai.JKW = kw;
            int jumlahPeserta = 0;
            decimal jumlahPerihal = 0;
            decimal jumlahAkPV = 0;

            if (spPendahuluanPelbagai == null)
            {
                return NotFound();
            }
            else
            {
                if (spPendahuluanPelbagai.SpPendahuluanPelbagai1 != null)
                {
                    foreach (var item in spPendahuluanPelbagai.SpPendahuluanPelbagai1)
                    {
                        jumlahPeserta += item.Jumlah;
                    }
                }
                
                if (spPendahuluanPelbagai.SpPendahuluanPelbagai2 != null)
                {
                    foreach (var item in spPendahuluanPelbagai.SpPendahuluanPelbagai2)
                    {
                        jumlahPerihal += item.Jumlah;
                    }
                }
                
                var akPV = _context.AkPV.Where(b => b.SpPendahuluanPelbagaiId == id).ToList();

                foreach (var i in akPV)
                {
                    jumlahAkPV += i.Jumlah;
                }

                ViewData["jumlahPeserta"] = jumlahPeserta;
                ViewData["jumlahPerihal"] = jumlahPerihal;
                ViewData["jumlahAkPV"] = jumlahAkPV;

                PopulateList(spPendahuluanPelbagai.SuPekerjaId);
                PopulateTable(id);
                return View(spPendahuluanPelbagai);
            }
            
        }

        // GET: SpPendahuluanPelbagai/Create
        public async Task<IActionResult> Create()
        {
            SpPendahuluanPelbagai sp = new SpPendahuluanPelbagai();

            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

            if (pekerjaId == null)
            {
                sp.SuPekerjaId = 1;
            }
            else
            {
                sp.SuPekerjaId = pekerjaId;
            }

            var jkw = _context.JKW.FirstOrDefault();

            sp.JKWId = jkw!.Id;

            ViewBag.NoPermohonan = RunningNumber(sp);
            PopulateList(sp.SuPekerjaId);
            CartEmpty();
            return View();
        }

        // POST: SpPendahuluanPelbagai/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "SP001C")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SpPendahuluanPelbagai spPendahuluanPelbagai, string syscode)
        {
            SpPendahuluanPelbagai m = new SpPendahuluanPelbagai();
            var bahagian = _context.JBahagian.FirstOrDefault(x => x.Id == spPendahuluanPelbagai.JBahagianId);
            var tahap = _context.JTahapAktiviti.FirstOrDefault(x => x.Id == spPendahuluanPelbagai.JTahapAktivitiId);
            var sukan = _context.JSukan.FirstOrDefault(x => x.Id == spPendahuluanPelbagai.JSukanId);
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

            if (user != null && user.Email == Init.superAdminEmail)
            {
                pekerjaId = 1;
            }

            if (ModelState.IsValid)
            {
                m.JKWId = spPendahuluanPelbagai.JKWId;
                m.JenisPermohonan = spPendahuluanPelbagai.JenisPermohonan;
                m.NoPermohonan = RunningNumber(spPendahuluanPelbagai);
                m.Tarikh = spPendahuluanPelbagai.Tarikh?.ToUpper() ?? "";
                m.JNegeriId = spPendahuluanPelbagai.JNegeriId;
                m.JSukan = sukan;
                m.Aktiviti = spPendahuluanPelbagai.Aktiviti?.ToUpper() ?? "";
                m.Tempat = spPendahuluanPelbagai.Tempat?.ToUpper() ?? "";
                m.JTahapAktiviti = tahap;
                m.AkCartaId = spPendahuluanPelbagai.AkCartaId;
                m.JumKeseluruhan = spPendahuluanPelbagai.JumKeseluruhan;
                m.SuPekerjaId = pekerjaId;
                m.FlPosting = 0;
                m.FlHapus = 0;
                m.FlCetak = 0;
                m.TarMasuk = DateTime.Now;
                m.JBahagian = bahagian;
                m.UserId = user?.UserName ?? "";
                m.SuPekerjaMasukId = pekerjaId;

                m.SpPendahuluanPelbagai1 = _cart.Lines1.ToArray();
                m.SpPendahuluanPelbagai2 = _cart.Lines2.ToArray();

                await _spPendahuluanPelbagaiRepo.Insert(m);

                //insert applog
                _appLog.Insert("Tambah", m.NoPermohonan, m.NoPermohonan, 0, m.JumKeseluruhan, pekerjaId,modul,syscode,namamodul,user);
                //insert applog end

                await _context.SaveChangesAsync();

                //CartEmpty();
                TempData[SD.Success] = "Maklumat Borang Permohonan berjaya ditambah";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.NoPermohonan = RunningNumber(spPendahuluanPelbagai);
            PopulateTableFromCart();
            PopulateList(spPendahuluanPelbagai.SuPekerjaId);
            return View(spPendahuluanPelbagai);
        }

        // GET: SpPendahuluanPelbagai/Edit/5
        [Authorize(Policy = "SP001E")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SpPendahuluanPelbagai == null)
            {
                return NotFound();
            }

            var spPendahuluanPelbagai = await _spPendahuluanPelbagaiRepo.GetById((int)id);
            int jumlahPeserta = 0;
            decimal jumlahPerihal = 0;

            if (spPendahuluanPelbagai == null)
            {
                return NotFound();
            }

            if (spPendahuluanPelbagai.SpPendahuluanPelbagai1 != null)
            {
                foreach (var item in spPendahuluanPelbagai.SpPendahuluanPelbagai1)
                {
                    jumlahPeserta += item.Jumlah;
                }
            }
            

            if (spPendahuluanPelbagai.SpPendahuluanPelbagai2 != null)
            {
                foreach (var item in spPendahuluanPelbagai.SpPendahuluanPelbagai2)
                {
                    jumlahPerihal += item.Jumlah;
                }
            }
            
            ViewData["jumlahPeserta"] = jumlahPeserta;
            ViewData["jumlahPerihal"] = jumlahPerihal;

            CartEmpty();
            PopulateList(spPendahuluanPelbagai.SuPekerjaId);
            PopulateTable(id);
            PopulateCartFromDb(spPendahuluanPelbagai);
            return View(spPendahuluanPelbagai);
        }

        public async Task<JsonResult> SaveSpPendahuluanPelbagai1(SpPendahuluanPelbagai1 spPendahuluanPelbagai1)
        {

            try
            {
                if (spPendahuluanPelbagai1 != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    _cart.AddItem1(spPendahuluanPelbagai1.SpPendahuluanPelbagaiId,
                         spPendahuluanPelbagai1.JJantinaId,
                         spPendahuluanPelbagai1.BilAtl,
                         spPendahuluanPelbagai1.BilJul,
                         spPendahuluanPelbagai1.BilPeg,
                         spPendahuluanPelbagai1.BilTek,
                         spPendahuluanPelbagai1.BilUru,
                         spPendahuluanPelbagai1.Jumlah);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        public async Task<JsonResult> SaveSpPendahuluanPelbagai2(SpPendahuluanPelbagai2 spPendahuluanPelbagai2)
        {

            try
            {
                if (spPendahuluanPelbagai2 != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    _cart.AddItem2(spPendahuluanPelbagai2.SpPendahuluanPelbagaiId,
                         spPendahuluanPelbagai2.Indek,
                         spPendahuluanPelbagai2.Baris,
                         spPendahuluanPelbagai2.Perihal?.ToUpper() ?? "",
                         spPendahuluanPelbagai2.Kadar,
                         spPendahuluanPelbagai2.Bil,
                         spPendahuluanPelbagai2.Bulan,
                         spPendahuluanPelbagai2.Jumlah);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        public JsonResult RemoveSpPendahuluanPelbagai1(SpPendahuluanPelbagai1 spPendahuluanPelbagai1)
        {

            try
            {
                if (spPendahuluanPelbagai1 != null)
                {

                    _cart.RemoveItem1(spPendahuluanPelbagai1.JJantinaId);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult RemoveSpPendahuluanPelbagai2(SpPendahuluanPelbagai2 spPendahuluanPelbagai2)
        {

            try
            {
                if (spPendahuluanPelbagai2 != null)
                {

                    _cart.RemoveItem2(spPendahuluanPelbagai2.Indek);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        //save cart SpPendahuluanPelbagai1
        public JsonResult SaveCartSpPendahuluanPelbagai1(SpPendahuluanPelbagai1 spPendahuluanPelbagai1)
        {

            try
            {

                var akP1 = _cart.Lines1.Where(x => x.JJantinaId == spPendahuluanPelbagai1.JJantinaId).FirstOrDefault();

                var user = _userManager.GetUserName(User);

                if (akP1 != null)
                {
                    _cart.RemoveItem1(spPendahuluanPelbagai1.JJantinaId);

                    _cart.AddItem1(spPendahuluanPelbagai1.SpPendahuluanPelbagaiId,
                                    spPendahuluanPelbagai1.JJantinaId,
                                    spPendahuluanPelbagai1.BilAtl,
                                    spPendahuluanPelbagai1.BilJul,
                                    spPendahuluanPelbagai1.BilPeg,
                                    spPendahuluanPelbagai1.BilTek,
                                    spPendahuluanPelbagai1.BilUru,
                                    spPendahuluanPelbagai1.Jumlah);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        //save cart SpPendahuluanPelbagai1 end

        // get an item from cart SpPendahuluanPelbagai1
        public JsonResult GetAnItemCartSpPendahuluanPelbagai1(SpPendahuluanPelbagai1 spPendahuluanPelbagai1)
        {

            try
            {
                SpPendahuluanPelbagai1 data = _cart.Lines1.Where(x => x.JJantinaId == spPendahuluanPelbagai1.JJantinaId).FirstOrDefault();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get an item from cart SpPendahuluanPelbagai1 end

        // get all item from cart SpPendahuluanPelbagai1
        public JsonResult GetAllItemCartSpPendahuluanPelbagai1(SpPendahuluanPelbagai1 spPendahuluanPelbagai1)
        {

            try
            {
                List<SpPendahuluanPelbagai1> data = _cart.Lines1.ToList();

                foreach (SpPendahuluanPelbagai1 item in data)
                {
                    var jJantina = _context.JJantina.Find(item.JJantinaId);

                    item.JJantina = jJantina;
                }

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get all item from cart SpPendahuluanPelbagai1 end 

        //save cart SpPendahuluanPelbagai2
        public JsonResult SaveCartSpPendahuluanPelbagai2(SpPendahuluanPelbagai2 spPendahuluanPelbagai2)
        {

            try
            {

                var akP1 = _cart.Lines2.Where(x => x.Indek == spPendahuluanPelbagai2.Indek).FirstOrDefault();

                var user = _userManager.GetUserName(User);

                if (akP1 != null)
                {
                    _cart.RemoveItem2(spPendahuluanPelbagai2.Indek);

                    _cart.AddItem2(spPendahuluanPelbagai2.SpPendahuluanPelbagaiId,
                                    spPendahuluanPelbagai2.Indek,
                                    spPendahuluanPelbagai2.Baris,
                                    spPendahuluanPelbagai2.Perihal?.ToUpper() ?? "",
                                    spPendahuluanPelbagai2.Kadar,
                                    spPendahuluanPelbagai2.Bil,
                                    spPendahuluanPelbagai2.Bulan,
                                    spPendahuluanPelbagai2.Jumlah);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        //save cart SpPendahuluanPelbagai2 end

        // get an item from cart SpPendahuluanPelbagai2
        public JsonResult GetAnItemCartSpPendahuluanPelbagai2(SpPendahuluanPelbagai2 spPendahuluanPelbagai2)
        {

            try
            {
                SpPendahuluanPelbagai2 data = _cart.Lines2.Where(x => x.Indek == spPendahuluanPelbagai2.Indek).FirstOrDefault();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get an item from cart SpPendahuluanPelbagai2 end

        // get all item from cart SpPendahuluanPelbagai2
        public JsonResult GetAllItemCartSpPendahuluanPelbagai2(SpPendahuluanPelbagai2 spPendahuluanPelbagai2)
        {

            try
            {
                List<SpPendahuluanPelbagai2> data = _cart.Lines2.OrderBy(b => b.Indek).ToList();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get all item from cart SpPendahuluanPelbagai1 end

        // POST: SpPendahuluanPelbagai/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "SP001E")]
        public async Task<IActionResult> Edit(int id, SpPendahuluanPelbagai spPendahuluanPelbagai, decimal JumKeseluruhan, string syscode)
        {
            if (id != spPendahuluanPelbagai.Id)
            {
                return NotFound();
            }

            if (spPendahuluanPelbagai.JumKeseluruhan == JumKeseluruhan)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var user = await _userManager.GetUserAsync(User);
                        int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;
                        SpPendahuluanPelbagai spPendahuluanPelbagaiAsal = await _spPendahuluanPelbagaiRepo.GetById(id);
                        // list of input that cannot be change
                        spPendahuluanPelbagai.JKWId = spPendahuluanPelbagaiAsal.JKWId;
                        spPendahuluanPelbagai.TarMasuk = spPendahuluanPelbagaiAsal.TarMasuk;
                        spPendahuluanPelbagai.UserId = spPendahuluanPelbagaiAsal.UserId;
                        spPendahuluanPelbagai.SuPekerjaMasukId = spPendahuluanPelbagaiAsal.SuPekerjaMasukId;
                        spPendahuluanPelbagai.FlCetak = 0;
                        // list of input that cannot be change end

                        if (spPendahuluanPelbagaiAsal.SpPendahuluanPelbagai1 != null)
                        {
                            foreach (SpPendahuluanPelbagai1 item in spPendahuluanPelbagaiAsal.SpPendahuluanPelbagai1)
                            {
                                var model = _context.SpPendahuluanPelbagai1.FirstOrDefault(b => b.Id == item.Id);
                                if (model != null)
                                {
                                    _context.Remove(model);
                                }
                            }
                        }
                        

                        if (spPendahuluanPelbagaiAsal.SpPendahuluanPelbagai2 != null)
                        {
                            foreach (SpPendahuluanPelbagai2 item in spPendahuluanPelbagaiAsal.SpPendahuluanPelbagai2)
                            {
                                var model = _context.SpPendahuluanPelbagai2.FirstOrDefault(b => b.Id == item.Id);
                                if (model != null)
                                {
                                    _context.Remove(model);
                                }
                            }
                        }
                        
                        // AK CODE
                        decimal jumSeluruhAsal = spPendahuluanPelbagaiAsal.JumKeseluruhan;
                        decimal jumSokongAsal = spPendahuluanPelbagaiAsal.JumSokong;
                        decimal jumLulusAsal = spPendahuluanPelbagaiAsal.JumLulus;
                        var logSeluruh = "";
                        var logSokong = "";
                        var logLulus = "";
                        //AK CODE END
                        _context.Entry(spPendahuluanPelbagaiAsal).State = EntityState.Detached;

                        spPendahuluanPelbagai.SpPendahuluanPelbagai1 = _cart.Lines1.ToList();
                        spPendahuluanPelbagai.SpPendahuluanPelbagai2 = _cart.Lines2.ToList();

                        // AK CODE 19/03/2022
                        spPendahuluanPelbagai.TarSokong = null;
                        spPendahuluanPelbagai.JPenyemakId = null;
                        spPendahuluanPelbagai.JumSokong = 0;
                        spPendahuluanPelbagai.FlStatusSokong = 0;

                        spPendahuluanPelbagai.TarLulus = null;
                        spPendahuluanPelbagai.JPelulusId = null;
                        spPendahuluanPelbagai.JumLulus = 0;
                        spPendahuluanPelbagai.FlStatusLulus = 0;
                        // Ak CODE 19/03/2022 END

                        spPendahuluanPelbagai.UserIdKemaskini = user?.UserName ?? "";
                        spPendahuluanPelbagai.TarKemaskini = DateTime.Now;
                        spPendahuluanPelbagai.SuPekerjaKemaskiniId = pekerjaId;

                        _context.Update(spPendahuluanPelbagai);

                        // AK CODE
                        //insert applog
                        if (jumSeluruhAsal != spPendahuluanPelbagai.JumKeseluruhan)
                        {
                            logSeluruh = "Kredit : RM " + jumSeluruhAsal.ToString() + " -> RM " + spPendahuluanPelbagai.JumKeseluruhan;
                        }

                        if (jumSokongAsal != spPendahuluanPelbagai.JumSokong)
                        {
                            logSokong = "Debit : RM " + jumSokongAsal.ToString() + " -> RM " + spPendahuluanPelbagai.JumSokong;
                        }

                        if (jumLulusAsal != spPendahuluanPelbagai.JumLulus)
                        {
                            logLulus = "Debit : RM " + jumLulusAsal.ToString() + " -> RM " + spPendahuluanPelbagai.JumLulus;
                        }

                        if (logSeluruh != "" || logSokong != "" || logLulus != "")
                        {
                            _appLog.Insert("Ubah",
                                           "Ubah Data : " + logSeluruh + ", " + logSokong + ", " + logLulus,
                                           spPendahuluanPelbagai.NoPermohonan,
                                           id,
                                           spPendahuluanPelbagai.JumKeseluruhan,
                                           pekerjaId,
                                           modul,
                                           syscode,
                                           namamodul,
                                           user);
                        }
                        else
                        {
                            _appLog.Insert("Ubah",
                                           "Ubah Data",
                                           spPendahuluanPelbagai.NoPermohonan,
                                           id,
                                           spPendahuluanPelbagai.JumKeseluruhan,
                                           pekerjaId,
                                           modul,
                                           syscode,
                                           namamodul,
                                           user);
                        }
                        //insert applog end
                        //AK CODE END

                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!SpPendahuluanPelbagaiExists(spPendahuluanPelbagai.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    CartEmpty();
                    // checking for jumlah objek & jumlah perihal
                    if (spPendahuluanPelbagai.JumKeseluruhan != JumKeseluruhan)
                    {
                        TempData[SD.Warning] = "Jumlah Objek tidak sama dengan Jumlah Urusniaga";
                    }
                    else
                    {
                        TempData[SD.Success] = "Data berjaya diubah..!";
                    }

                    return RedirectToAction(nameof(Index));
                }
            }

            TempData[SD.Warning] = "Jumlah Objek tidak sama dengan Jumlah Urusniaga";
            PopulateList(spPendahuluanPelbagai.SuPekerjaId);
            PopulateTableFromCart();
            return View(spPendahuluanPelbagai);
        }

        // GET: SpPendahuluanPelbagai/Delete/5
        [Authorize(Policy = "SP001D")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SpPendahuluanPelbagai == null)
            {
                return NotFound();
            }

            var spPendahuluanPelbagai = await _spPendahuluanPelbagaiRepo.GetById((int) id);
            if (spPendahuluanPelbagai == null)
            {
                return NotFound();
            }

            PopulateList(spPendahuluanPelbagai.SuPekerjaId);
            PopulateTable(id);
            return View(spPendahuluanPelbagai);
        }

        // POST: SpPendahuluanPelbagai/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Policy = "SP001D")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string syscode)
        {
            if (_context.SpPendahuluanPelbagai == null)
            {
                return Problem("Entity set 'ApplicationDbContext.SpPendahuluanPelbagai'  is null.");
            }
            var obj = await _context.SpPendahuluanPelbagai.FindAsync(id);
            if (obj != null)
            {
                var user = await _userManager.GetUserAsync(User);
                int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

                obj.UserIdKemaskini = user?.UserName ?? "";
                obj.TarKemaskini = DateTime.Now;
                obj.SuPekerjaKemaskiniId = pekerjaId;
                // check if already posting redirect back
                if (obj.FlPosting == 1)
                {
                    TempData[SD.Error] = "Akses tidak dibenarkan..!";
                    return RedirectToAction(nameof(Index));
                }
                obj.FlCetak = 0;
                _context.SpPendahuluanPelbagai.Update(obj);

                //insert applog
                _appLog.Insert("Hapus", obj.NoPermohonan, obj.NoPermohonan, id, obj.JumKeseluruhan, pekerjaId, modul, syscode, namamodul, user);
                //insert applog end

                _context.SpPendahuluanPelbagai.Remove(obj);
                await _context.SaveChangesAsync();
                TempData[SD.Success] = "Data berjaya dihapuskan..!";
            }
            
            return RedirectToAction(nameof(Index));
        }

        // Sokong function
        [HttpPost, ActionName("Sokong")]
        [Authorize(Policy = "SP001T")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sokong(int? id, decimal jumSokong, int penyemakId, DateTime? tarikhSokong, string syscode)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var user = await _userManager.GetUserAsync(User);
                int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;
                //var user = UserManager.GetUserAsync(User);
                var namaUser = _context.
                    applicationUsers.Include(x => x.SuPekerja).FirstOrDefault(x => x.Email == user!.UserName);
                //var pelulus = await _context.JPelulus.Include(x => x.SuPekerja).Where(x => x.IsPendahuluan == true).FirstOrDefaultAsync();
                //var sokong = Convert.ToDecimal(jumSokong);

                SpPendahuluanPelbagai sp = await _spPendahuluanPelbagaiRepo.GetById((int)id);

                //check for print
                if (sp.FlCetak == 0)
                {
                    //duplicate id error
                    TempData[SD.Error] = "Data gagal disokong. Sila cetak data dahulu sebelum menjalani operasi ini.";
                    return RedirectToAction(nameof(Index));
                }
                //check for print end

                //posting operation start here
                if (jumSokong != 0)
                {
                    if (jumSokong <= sp.JumKeseluruhan)
                    {
                        //update posting status in SPPENDAHULUANPELBAGAI
                        sp.FlStatusSokong = 1;
                        sp.TarSokong = tarikhSokong;
                        sp.JumSokong = jumSokong;

                        sp.JPenyemakId = penyemakId;


                        await _spPendahuluanPelbagaiRepo.Update(sp);

                        //insert applog
                        _appLog.Insert("Posting", "Sokong Data", sp.NoPermohonan, (int)id, sp.JumKeseluruhan, pekerjaId,modul,syscode,namamodul,user);

                        //insert applog end

                        await _context.SaveChangesAsync();

                        TempData[SD.Success] = "Data berjaya disokong.";
                    }
                    else
                    {
                        TempData[SD.Error] = "Jumlah sokong tidak boleh lebih dari jumlah dicadang.";
                    }
                }
                else
                {
                    TempData[SD.Error] = "Jumlah RM0.00 tidak dibenarkan.";
                }


            }

            return RedirectToAction(nameof(Index));

        }
        // Sokong function end

        // posting function
        [HttpPost, ActionName("Posting")]
        [Authorize(Policy = "SP001T")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Posting(int? id, decimal jumSokong, decimal jumLulus, int pelulusId, DateTime tarikhLulus, string syscode)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {

                if (jumLulus != 0)
                {
                    if (jumLulus <= jumSokong)
                    {
                        var user = await _userManager.GetUserAsync(User);
                        int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;
                        var pelulus = await _context.JPelulus.Include(x => x.SuPekerja).Where(x => x.IsPendahuluan == true).FirstOrDefaultAsync();

                        SpPendahuluanPelbagai sp = await _spPendahuluanPelbagaiRepo.GetById((int)id);

                        //check for print
                        if (sp.FlCetak == 0)
                        {
                            //duplicate id error
                            TempData[SD.Error] = "Data gagal diluluskan. Sila cetak data dahulu sebelum menjalani operasi ini.";
                            return RedirectToAction(nameof(Index));
                        }
                        //check for print end

                        // check peruntukan
                        bool IsExistAbBukuVot = await _context.AbBukuVot
                                   .Where(x => x.Tahun == sp.TarMasuk.Year.ToString() && x.VotId == sp.AkCartaId && x.JKWId == sp.JKWId && x.JBahagianId == sp.JBahagianId)
                                   .AnyAsync();

                        if (IsExistAbBukuVot == true)
                        {
                            decimal sum = await _customRepo.GetBalanceFromAbBukuVot(sp.TarMasuk.Year.ToString(), sp.AkCartaId, sp.JKWId, sp.JBahagianId);

                            if (sum < jumLulus)
                            {
                                TempData[SD.Error] = "Bajet untuk kod akaun " + sp.AkCarta?.Kod + " tidak mencukupi.";
                                return RedirectToAction(nameof(Index));
                            }
                        }
                        else
                        {
                            TempData[SD.Error] = "Tiada peruntukan untuk kod akaun " + sp.AkCarta?.Kod;
                            return RedirectToAction(nameof(Index));
                        }
                        // check peruntukan end

                        var abBukuVot = await _context.AbBukuVot.Where(x => x.Rujukan.EndsWith("SP/" + sp.NoPermohonan)).FirstOrDefaultAsync();
                        if (abBukuVot != null)
                        {

                            //duplicate id error
                            TempData[SD.Error] = "Data gagal diluluskan.";

                        }
                        else
                        {
                            //posting operation start here


                            //insert into AbBukuVot
                            AbBukuVot abBukuVotPosting = new AbBukuVot()
                            {
                                Tahun = sp.TarMasuk.Year.ToString(),
                                JKWId = sp.JKWId,
                                Tarikh = sp.TarMasuk,
                                Kod = sp.SuPekerja?.NoGaji ?? "", // tak pasti tarik dari id pekerja ke?
                                Penerima = sp.SuPekerja?.Nama ?? "",
                                VotId = (int)sp.AkCartaId!,
                                Rujukan = "SP/" + sp.NoPermohonan,
                                Tanggungan = jumLulus,
                                JBahagianId = sp.JBahagianId
                            };

                            await _abBukuVotRepo.Insert(abBukuVotPosting);
                            // insert into AbBukuVot end

                            //update posting status in SPPENDAHULUANPELBAGAI
                            sp.FlStatusLulus = 1;
                            sp.FlPosting = 1;
                            sp.TarikhPosting = DateTime.Now;
                            sp.JumSokong = jumSokong;
                            sp.JumLulus = jumLulus;
                            sp.TarLulus = tarikhLulus;
                            sp.JPelulusId = pelulusId;

                            await _spPendahuluanPelbagaiRepo.Update(sp);

                            //insert applog
                            _appLog.Insert("Posting", "Posting Data", sp.NoPermohonan, (int)id, sp.JumKeseluruhan, pekerjaId,modul, syscode, namamodul, user);

                            //insert applog end

                            await _context.SaveChangesAsync();

                            TempData[SD.Success] = "Data berjaya diluluskan.";
                        }
                    }
                    else
                    {
                        TempData[SD.Error] = "Jumlah lulus tidak boleh lebih dari jumlah disokong.";
                    }

                }
                else
                {
                    TempData[SD.Error] = "Jumlah RM0.00 tidak dibenarkan.";
                }



            }

            return RedirectToAction(nameof(Index));

        }
        // posting function end

        // unposting function
        [Authorize(Policy = "SP001UT")]
        public async Task<IActionResult> UnPosting(int? id, string syscode)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var user = await _userManager.GetUserAsync(User);
                int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

                SpPendahuluanPelbagai obj = await _spPendahuluanPelbagaiRepo.GetById((int)id);

                var akPV = await _context.AkPV.Where(x => x.SpPendahuluanPelbagaiId == id).FirstOrDefaultAsync();
                if (akPV != null)
                {
                    TempData[SD.Error] = "Data terkait dengan No Baucer " + akPV.NoPV + ". Batal lulus tidak dibenarkan.";
                    return RedirectToAction(nameof(Index));
                }

                var akTerima = await _context.AkTerima.Where(x => x.SpPendahuluanPelbagaiId == id).FirstOrDefaultAsync();

                if (akTerima != null)
                {
                    TempData[SD.Error] = "Data terkait dengan Terimaan " +akTerima.NoRujukan+". Batal lulus tidak dibenarkan.";
                    return RedirectToAction(nameof(Index));
                }
                List<AbBukuVot> abBukuVot = _context.AbBukuVot.Where(x => x.Rujukan.EndsWith(obj.NoPermohonan)).ToList();
                if (abBukuVot == null)
                {

                    //duplicate id error
                    TempData[SD.Error] = "Data belum diluluskan.";
                    return RedirectToAction(nameof(Index));

                }
                else
                {

                    //unposting operation start here
                    //delete data from abBukuVot
                    foreach (AbBukuVot item in abBukuVot)
                    {
                        await _abBukuVotRepo.Delete(item.Id);
                    }
                    //delete data from abBukuVot end

                    //update posting status in SPPENDAHULUANPELBAGAI
                    obj.FlPosting = 0;
                    obj.TarikhPosting = null;

                    // AK CODE 19/03/2022
                    obj.FlStatusLulus = 0;
                    obj.TarLulus = null;
                    obj.JPelulusId = null;
                    obj.JumLulus = 0;
                    // AK CODE 19/03/2022

                    await _spPendahuluanPelbagaiRepo.Update(obj);

                    //insert applog
                    _appLog.Insert("UnPosting", "UnPosting Data", obj.NoPermohonan, (int)id, obj.JumKeseluruhan, pekerjaId,modul,syscode,namamodul,user);

                    //insert applog end

                    await _context.SaveChangesAsync();

                    TempData[SD.Success] = "Data berjaya batal kelulusan.";
                    //unposting operation end
                }


            }

            return RedirectToAction(nameof(Index));

        }
        // unposting function end

        // AK CODE 31/03/2022
        [Authorize(Policy = "SP001R")]
        public async Task<IActionResult> RollBack(int id, string syscode)
        {
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

            var obj = await _spPendahuluanPelbagaiRepo.GetByIdIncludeDeletedItems(id);
            // check if already posting redirect back
            if (obj.FlPosting == 1)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }

            // rollback operation

            obj.FlHapus = 0;
            obj.FlCetak = 0;
            obj.TarSokong = null;
            obj.JPenyemakId = null;
            obj.JumSokong = 0;
            obj.FlStatusSokong = 0;

            obj.TarLulus = null;
            obj.JPelulusId = null;
            obj.JumLulus = 0;
            obj.FlStatusLulus = 0;

            obj.UserIdKemaskini = user?.UserName ?? "";
            obj.TarKemaskini = DateTime.Now;
            obj.SuPekerjaKemaskiniId = pekerjaId;

            _context.SpPendahuluanPelbagai.Update(obj);

            // rollback operation end

            //insert applog
            _appLog.Insert("Rollback", "Rollback Data", obj.NoPermohonan, id, obj.JumKeseluruhan, pekerjaId,modul,syscode,namamodul,user);
            //insert applog end

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dikembalikan..!";
            return RedirectToAction(nameof(Index));
        }
        // AK CODE 31/03/2022 END

        public async Task<IActionResult> PrintPdf(int id, string syscode)
        {
            SpPendahuluanPelbagai sp = await _spPendahuluanPelbagaiRepo.GetByIdIncludeDeletedItems(id);

            var jumlahDalamPerkataan = ("Ringgit Malaysia " + CalculatePrice.JumlahDalamPerkataan(sp.JumKeseluruhan)).ToUpper();

            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

            PendahuluanPelbagaiPrintModel data = new PendahuluanPelbagaiPrintModel();
            
            if (sp.SpPendahuluanPelbagai1 != null)
            {
                foreach (SpPendahuluanPelbagai1 item in sp.SpPendahuluanPelbagai1)
                {
                    data.BilAtl += item.BilAtl;
                    data.BilJul += item.BilJul;
                    data.BilPeg += item.BilPeg;
                    data.BilTek += item.BilTek;
                    data.BilUru += item.BilUru;
                    data.Jumlah += item.Jumlah;
                }
            }
            

            if (sp.SpPendahuluanPelbagai2 != null)
            {
                foreach (SpPendahuluanPelbagai2 item in sp.SpPendahuluanPelbagai2)
                {
                    data.JumlahPerihal += item.Jumlah;
                }
            }
            
            List<JTahapAktiviti> list = _context.JTahapAktiviti.ToList();

            data.Tahap = list;

            CompanyDetails company = new CompanyDetails();
            data.CompanyDetail = company;
            data.SpPendahuluanPelbagai = sp;
            //data.spPermohonanAktiviti.JNegeri = negeri;
            data.JumlahDalamPerkataan = jumlahDalamPerkataan;
            data.Username = user?.UserName ?? "";

            //update cetak -> 1
            sp.FlCetak = 1;
            sp.TarSedia = DateTime.Now;
            await _spPendahuluanPelbagaiRepo.Update(sp);

            //insert applog
            _appLog.Insert("Cetak", "Cetak Data", sp.NoPermohonan, id, sp.JumKeseluruhan, pekerjaId,modul,syscode,namamodul,user);
            //insert applog end

            await _context.SaveChangesAsync();

            return new ViewAsPdf("PendahuluanPelbagaiPrintPDF", data)
            {
                PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                //CustomSwitches = "--footer-center \"  Tarikh: " +
                //    DateTime.Now.ToString("g",
                //  CultureInfo.CreateSpecificCulture("en-us")) + 
                //  "            Mukasurat: [page]/[toPage]\"" +
                //    " --footer-line --footer-font-size \"10\" --footer-spacing 1 --footer-font-name \"Segoe UI\"",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
            };
        }

        private bool SpPendahuluanPelbagaiExists(int id)
        {
          return (_context.SpPendahuluanPelbagai?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
