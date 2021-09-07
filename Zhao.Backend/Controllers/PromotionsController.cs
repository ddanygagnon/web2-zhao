using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zhao.Backend.Extra;
using Zhao.Backend.Models;


namespace Zhao.Backend.Controllers
{
    public static class PromotionFiltre
    {
        public static bool EstFiltrer { get; set; }
    }

    public class PromotionsController : Controller
    {
        private readonly TpContext _context;
        private IAuthorizationService _authServ;

        public PromotionsController(TpContext context, IAuthorizationService authServ)
        {
            _context = context;
            _authServ = authServ;
        }

        // GET: Promotions
        public async Task<IActionResult> Index(string sortOrder)
        {
            PromotionFiltre.EstFiltrer = false;
            return (await _authServ.AuthorizeAsync(User, "EstAdministrateur")).Succeeded
                ? View(await _context.Promotions.OrderBy(p => p.DateFin).ToListAsync())
                : View(await _context.Promotions.Where(p => p.DateFin == null || p.DateFin > DateTime.Now).OrderBy(p => p.DateFin)
                    .ToListAsync());
        }

        public async Task<IActionResult> FiltrerComptoir()
        {
            PromotionFiltre.EstFiltrer = true;
            return (await _authServ.AuthorizeAsync(User, "EstAdministrateur")).Succeeded
                ? View("Index",
                    _context.Promotions.Where(x => x.Type == Enum.GetName(PromotionChoixType.Comptoir))
                        .OrderBy(x => x.DateFin).ToList())
                : View("Index",
                    _context.Promotions.Where(x => x.Type == Enum.GetName(PromotionChoixType.Comptoir))
                        .Where(p => p.DateFin == null || p.DateFin > DateTime.Now)
                        .OrderBy(x => x.DateFin).ToList());
        }

        public async Task<IActionResult> FiltrerLivraison()
        {
            PromotionFiltre.EstFiltrer = true;
            return (await _authServ.AuthorizeAsync(User, "EstAdministrateur")).Succeeded
                ? View("Index",
                    _context.Promotions.Where(x => x.Type == Enum.GetName(PromotionChoixType.Livraison))
                        .OrderBy(x => x.DateFin).ToList())
                : View("Index",
                    _context.Promotions.Where(x => x.Type == Enum.GetName(PromotionChoixType.Livraison))
                        .Where(p => p.DateFin == null || p.DateFin > DateTime.Now)
                        .OrderBy(x => x.DateFin).ToList());
        }

        public async Task<IActionResult> FiltrerSalleManger()
        {
            PromotionFiltre.EstFiltrer = true;
            return (await _authServ.AuthorizeAsync(User, "EstAdministrateur")).Succeeded
                ? View("Index",
                    _context.Promotions.Where(x => x.Type == "Salle à manger").OrderBy(x => x.DateFin).ToList())
                : View("Index",
                    _context.Promotions.Where(x => x.Type == "Salle à manger").OrderBy(x => x.DateFin)
                        .Where(p => p.DateFin == null || p.DateFin > DateTime.Now).ToList());
        }

        public async Task<IActionResult> FiltrerTous()
        {
            PromotionFiltre.EstFiltrer = true;
            return (await _authServ.AuthorizeAsync(User, "EstAdministrateur")).Succeeded
                ? View("Index",
                    _context.Promotions.Where(x => x.Type == Enum.GetName(PromotionChoixType.Tous))
                        .OrderBy(x => x.DateFin)
                        .ToList())
                : View("Index",
                    _context.Promotions.Where(x => x.Type == Enum.GetName(PromotionChoixType.Tous))
                        .OrderBy(x => x.DateFin)
                        .Where(p => p.DateFin == null || p.DateFin > DateTime.Now)
                        .ToList());
        }

        // GET: Promotions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var promotion = await _context.Promotions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (promotion == null)
            {
                return NotFound();
            }

            return View(promotion);
        }

        // GET: Promotions/Create
        [Authorize(Roles = "Administrateur")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Promotions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Type,TauxApplicable,DateDebut,DateFin,Description")] Promotion promotion)
        {
            if (ModelState.IsValid)
            {
                promotion.Id = !_context.Promotions.Any() ? 1 : _context.Promotions.Max(promo => promo.Id) + 1;
                _context.Add(promotion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(promotion);
        }

        // GET: Promotions/Edit/5
        [Authorize(Roles = "Administrateur")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var promotion = await _context.Promotions.FindAsync(id);
            if (promotion == null)
            {
                return NotFound();
            }
            return View(promotion);
        }

        // POST: Promotions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Type,TauxApplicable,DateDebut,DateFin,Description")] Promotion promotion)
        {
            if (id != promotion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(promotion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PromotionExists(promotion.Id))
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
            return View(promotion);
        }

        // GET: Promotions/Delete/5
        [Authorize(Roles = "Administrateur")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var promotion = await _context.Promotions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (promotion == null)
            {
                return NotFound();
            }

            return View(promotion);
        }

        // POST: Promotions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var promotion = await _context.Promotions.FindAsync(id);
            _context.Promotions.Remove(promotion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PromotionExists(int id)
        {
            return _context.Promotions.Any(e => e.Id == id);
        }
    }
}
