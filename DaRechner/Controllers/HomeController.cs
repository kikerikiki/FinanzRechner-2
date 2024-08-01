using LAPTemplateMVC.Models;
using LAPTemplateMVC.Models.dboSchema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace LAPTemplateMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FinanzRechnerContext _context;

        public HomeController(ILogger<HomeController> logger, FinanzRechnerContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var einnahmen = await _context.Procedures.EinnahmeGetAllAsync(1);
            var ausgaben = await _context.Procedures.AusgabeGetAllAsync(1);

            var kategorien = await _context.Kategorie.ToListAsync();

            var einnahmenList = einnahmen.Select(e => new Einnahme
            {
                Einnahmeid = e.EINNAHMEID,
                Kategorieid = e.KATEGORIEID,
                Bezeichnung = e.BEZEICHNUNG,
                Betrag = e.BETRAG,
                CrTimestamp = e.CR_TIMESTAMP,
                Kategorie = kategorien.FirstOrDefault(k => k.Kategorieid == e.KATEGORIEID)
            }).ToList();

            var ausgabenList = ausgaben.Select(a => new Ausgabe
            {
                Ausgabeid = a.AUSGABEID,
                Kategorieid = a.KATEGORIEID,
                Bezeichnung = a.BEZEICHNUNG,
                Betrag = a.BETRAG,
                CrTimestamp = a.CR_TIMESTAMP,
                Kategorie = kategorien.FirstOrDefault(k => k.Kategorieid == a.KATEGORIEID)
            }).ToList();

            ViewBag.Einnahmen = einnahmenList;
            ViewBag.Ausgaben = ausgabenList;

            return View();
        }

        public async Task<IActionResult> KategorieIndex()
        {
            var kategorieResults = await _context.Procedures.KategorieGetAllAsync(1);
            var validKategorien = kategorieResults.Where(kr => kr.VALID == 1).ToList();
            return View(validKategorien);
        }

        public async Task<IActionResult> EditEinnahme(long id)
        {
            var einnahme = await _context.Einnahme.FindAsync(id);
            if (einnahme == null)
            {
                return NotFound();
            }

            ViewBag.Kategorien = await _context.Kategorie.Where(k => k.Valid == 1).ToListAsync();

            return View(einnahme);
        }

        [HttpPost]
        public async Task<IActionResult> EditEinnahme(Einnahme model)
        {
            var returnvalue = new OutputParameter<int>();
            await _context.Procedures.EinnahmeUpdateAsync(model.Einnahmeid, model.Kategorieid, model.Bezeichnung, model.Betrag, model.Valid, returnvalue);
            if (returnvalue.Value == 0)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Kategorien = await _context.Kategorie.Where(k => k.Valid == 1).ToListAsync();

            return View(model);
        }

        public async Task<IActionResult> EditAusgabe(long id)
        {
            var ausgabe = await _context.Ausgabe.FindAsync(id);
            if (ausgabe == null)
            {
                return NotFound();
            }

            ViewBag.Kategorien = await _context.Kategorie.Where(k => k.Valid == 1).ToListAsync();

            return View(ausgabe);
        }

        [HttpPost]
        public async Task<IActionResult> EditAusgabe(Ausgabe model)
        {
            var returnvalue = new OutputParameter<int>();
            await _context.Procedures.AusgabeUpdateAsync(model.Ausgabeid, model.Kategorieid, model.Bezeichnung, model.Betrag, model.Valid, returnvalue);
            if (returnvalue.Value == 0)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Kategorien = await _context.Kategorie.Where(k => k.Valid == 1).ToListAsync();

            return View(model);
        }

        public IActionResult CreateKategorie()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateKategorie(Kategorie model)
        {
            model.Valid = 1;
            var returnvalue = new OutputParameter<int>();
            await _context.Procedures.KategorieInsertAsync(model.Name, model.Farbe, model.Valid, returnvalue);
            if (returnvalue.Value == 0)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public async Task<IActionResult> EditKategorie(long id)
        {
            var kategorie = await _context.Kategorie.FindAsync(id);
            if (kategorie == null)
            {
                return NotFound();
            }
            return View(kategorie);
        }

        [HttpPost]
        public async Task<IActionResult> EditKategorie(Kategorie model)
        {
            var returnvalue = new OutputParameter<int>();
            await _context.Procedures.KategorieUpdateAsync(model.Kategorieid, model.Name, model.Farbe, model.Valid, returnvalue);
            if (returnvalue.Value == 0)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteKategorie(long id, long newKategorieId)
        {
            var einnahmen = await _context.Procedures.EinnahmeGetByKategorieIDAsync(id, 1);
            foreach (var einnahme in einnahmen)
            {
                einnahme.KATEGORIEID = newKategorieId;
                await _context.Procedures.EinnahmeUpdateAsync(einnahme.EinnahmeID, einnahme.KATEGORIEID, einnahme.BEZEICHNUNG, einnahme.BETRAG, einnahme.VALID);
            }

            var ausgaben = await _context.Procedures.AusgabeGetByKategorieIDAsync(id, 1);
            foreach (var ausgabe in ausgaben)
            {
                ausgabe.KATEGORIEID = newKategorieId;
                await _context.Procedures.AusgabeUpdateAsync(ausgabe.AusgabeID, ausgabe.KATEGORIEID, ausgabe.BEZEICHNUNG, ausgabe.BETRAG, ausgabe.VALID);
            }

            var returnvalue = new OutputParameter<int>();
            await _context.Procedures.KategorieDeleteAsync(id, returnvalue);

            if (returnvalue.Value == 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Die Kategorie konnte nicht gelöscht werden.");
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEinnahme(long id)
        {
            var returnvalue = new OutputParameter<int>();
            await _context.Procedures.EinnahmeDeleteAsync(id, returnvalue);
            if (returnvalue.Value == 0)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Die Einnahme konnte nicht gelöscht werden.");
            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAusgabe(long id)
        {
            var returnvalue = new OutputParameter<int>();
            await _context.Procedures.AusgabeDeleteAsync(id, returnvalue);
            if (returnvalue.Value == 0)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Die Ausgabe konnte nicht gelöscht werden.");
            return View("Error");
        }

        public IActionResult AddEntry()
        {
            ViewBag.Kategorien = _context.Kategorie.Where(k => k.Valid == 1).ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddEntry(string EntryType, string Bezeichnung, decimal Betrag, long Kategorieid)
        {
            short valid = 1;
            if (EntryType == "Einnahme")
            {
                var returnvalue = new OutputParameter<int>();
                await _context.Procedures.EinnahmeInsertAsync(Kategorieid, Bezeichnung, Betrag, valid, returnvalue);
            }
            else if (EntryType == "Ausgabe")
            {
                var returnvalue = new OutputParameter<int>();
                await _context.Procedures.AusgabeInsertAsync(Kategorieid, Bezeichnung, Betrag, valid, returnvalue);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> TopEntries()
        {
            var einnahmen = await _context.Procedures.EinnahmeGetAllAsync(1);
            var ausgaben = await _context.Procedures.AusgabeGetAllAsync(1);

            var topEinnahmen = einnahmen?
                .OrderByDescending(e => e.BETRAG)
                .Take(5)
                .Select(e => new { BEZEICHNUNG = e.BEZEICHNUNG, BETRAG = e.BETRAG })
                .Cast<dynamic>()
                .ToList();

            var topAusgaben = ausgaben?
                .OrderByDescending(a => a.BETRAG)
                .Take(5)
                .Select(a => new { BEZEICHNUNG = a.BEZEICHNUNG, BETRAG = a.BETRAG })
                .Cast<dynamic>()
                .ToList();

            if (topEinnahmen == null)
            {
                topEinnahmen = new List<dynamic>();
            }

            if (topAusgaben == null)
            {
                topAusgaben = new List<dynamic>();
            }

            ViewBag.TopEinnahmen = topEinnahmen;
            ViewBag.TopAusgaben = topAusgaben;

            return View();
        }


        public async Task<IActionResult> SearchEntries(string term)
        {
            var einnahmen = await _context.Procedures.EinnahmeGetAllAsync(1);
            var ausgaben = await _context.Procedures.AusgabeGetAllAsync(1);

            var filteredEinnahmen = einnahmen
                .Where(e => string.IsNullOrEmpty(term) || e.BEZEICHNUNG.Contains(term, StringComparison.OrdinalIgnoreCase))
                .Select(e => new
                {
                    e.EINNAHMEID,
                    e.BEZEICHNUNG,
                    e.BETRAG,
                    e.CR_TIMESTAMP,
                    KategorieName = _context.Kategorie.FirstOrDefault(k => k.Kategorieid == e.KATEGORIEID)?.Name,
                    KategorieFarbe = _context.Kategorie.FirstOrDefault(k => k.Kategorieid == e.KATEGORIEID)?.Farbe
                })
                .ToList();

            var filteredAusgaben = ausgaben
                .Where(a => string.IsNullOrEmpty(term) || a.BEZEICHNUNG.Contains(term, StringComparison.OrdinalIgnoreCase))
                .Select(a => new
                {
                    a.AUSGABEID,
                    a.BEZEICHNUNG,
                    a.BETRAG,
                    a.CR_TIMESTAMP,
                    KategorieName = _context.Kategorie.FirstOrDefault(k => k.Kategorieid == a.KATEGORIEID)?.Name,
                    KategorieFarbe = _context.Kategorie.FirstOrDefault(k => k.Kategorieid == a.KATEGORIEID)?.Farbe
                })
                .ToList();

            return Json(new { einnahmen = filteredEinnahmen, ausgaben = filteredAusgaben });
        }

    public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
