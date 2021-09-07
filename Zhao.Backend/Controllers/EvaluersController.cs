using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zhao.Backend.Extra;
using Zhao.Backend.Models;

namespace Zhao.Backend.Controllers
{
    public static class EvaluersFiltre
    {
        public static bool EstFiltrer { get; set; }
    }

    public class EvaluersController : Controller
    {
        private readonly TpContext _context;
        private UserManager<IdentityUser> _userManager;

        public EvaluersController(TpContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }



        // GET: Evaluers
        [Authorize]
        public async Task<IActionResult> Index()
        {
            EvaluersFiltre.EstFiltrer = false;
            var user = await _userManager.GetUserAsync(User);
            var role = await _userManager.GetRolesAsync(user);

            var mesEvaluations = _context.Evaluations.Where(e => e.Courriel == user.Email);

            if (role.First() == "Administrateur")
            {
                mesEvaluations = _context.Evaluations;
            }

            if (!mesEvaluations.Any() && role.First() != "Administrateur")
            {
                return RedirectToAction("Create");
            }

            return View(await mesEvaluations.ToListAsync());
        }

        // GET: Evaluers/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var role = await _userManager.GetRolesAsync(user);

            var mesEvaluations = _context.Evaluations.Where(e => e.Courriel == user.Email);

            if (role.First() == "Administrateur")
            {
                mesEvaluations = _context.Evaluations;
            }

            var evaluer = await mesEvaluations
                .FirstOrDefaultAsync(m => m.Id == id);

            if (evaluer == null)
            {
                return NotFound();
            }

            return View(evaluer);
        }

        [Authorize(Roles = "Administrateur")]
        public async Task<IActionResult> Filtrer(int nbEtoiles)
        {
            EvaluersFiltre.EstFiltrer = true;
            return View("Index", await _context.Evaluations.Where(e => e.QualiteRepas == nbEtoiles).ToListAsync());
        }

        [Authorize(Roles = "Administrateur")]
        public async Task<IActionResult> FiltrerService(int nbEtoiles)
        {
            EvaluersFiltre.EstFiltrer = true;
            return View("Index", await _context.Evaluations.Where(e => e.QualiteService == nbEtoiles).ToListAsync());
        }

        // GET: Evaluers/Create
        [Authorize(Policy = "EstUtilisateur")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Evaluers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Prenom,Nom,Type,Courriel,DateVisite,QualiteRepas,QualiteService,Commentaires")] Evaluer evaluer)
        {
            var user = await _userManager.GetUserAsync(User);
            evaluer.Id = !_context.Evaluations.Any() ? 1 : _context.Evaluations.Max(e => e.Id) + 1;
            evaluer.Courriel = user.Email;
            evaluer.CourrielHtml = EmailHtml(evaluer);

            _context.Add(evaluer);

            await _context.SaveChangesAsync();

            var smtpClient = new SmtpClient("smtp.office365.com", 587)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("courrielTI@cstjean.qc.ca", "zzx274Qg=c%r"),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true
            };

            var mail = new MailMessage
            {
                From = new MailAddress("courrielTI@cstjean.qc.ca", "Mon site Internet"),
                IsBodyHtml = true,
                Body = EmailHtml(evaluer),
                Subject = "Zhào - Confirmation de l'évaluation"
            };
            mail.To.Add(new MailAddress(user.Email));
            smtpClient.Send(mail);

            Notification.Message = "Vous venez d'évaluer votre visite avec succès.";

            return RedirectToAction(nameof(Index));
        }

        private static string EmailHtml(Evaluer evaluer)
        {
            var aCommentaire = evaluer.Commentaires ?? "";
            var estCommentaire = aCommentaire.Length > 0;
            return @$"
<!DOCTYPE html>
<html lang=""en"">
<head>
	<meta charset=""UTF-8"">
	<title>Title</title>
	<link rel=""preconnect"" href=""https://fonts.gstatic.com"">
	<link href=""https://fonts.googleapis.com/css2?family=Alegreya:wght@400;500&display=swap"" rel=""stylesheet"">
</head>
<body style=""margin-top:0;margin-bottom:0;margin-right:0;margin-left:0; font-size: 1.15em;"" >
<div class=""container"" style=""width:calc(100% - 6.5rem);margin-right:auto;margin-left:auto;font-family:Alegreya,serif;"" >
	<h1 style=""line-height:1.2;font-family:Alegreya,serif;font-weight:500;font-size:5em;margin-top:0;margin-bottom:.75rem;margin-right:0;margin-left:0;padding-top:1.25rem;text-align:center;"" >Zhào</h1>
	<h2 style=""line-height:1.2;font-family:Alegreya,serif;font-weight:500;text-align:center;font-size:2em;margin-top:0;margin-bottom:2rem;margin-right:0;margin-left:0;"" >Votre évaluation à bien été enregistré</h2>
	<div>

	</div>
	<p style=""max-width:50ch;text-align:center;margin-left:auto;margin-right:auto;margin-top:0;margin-bottom:1.5rem;font-family:Alegreya,serif;"" >
		Bonjour <mark style="">{evaluer.Prenom} {evaluer.Nom}</mark>, votre évaluation a <mark style=""background-color:transparent;background-image:linear-gradient(transparent 50%,#ecf0eb 0);"" >biens été placé</mark>.
		Voici les informations qui ont été <mark style=""background-color:transparent;background-image:linear-gradient(transparent 50%,#ecf0eb 0);"" >enregistré</mark> dans notre système:
	</p>
</div>
<div class=""info"" style=""clip-path:polygon(0 0,100% 75px,100% 100%,0 calc(100% - 75px));background-color:#181818;padding-bottom:5.25rem;text-align:center;color:#f8f8f8;"" >
	<div class=""info__list"" style=""padding-top:5.25rem;padding-left:0;max-width:32rem;width:calc(100% - 6.5rem);margin-right:auto;margin-left:auto;"" >
		<p class=""label"" style=""margin-top:0;margin-bottom:.75rem;margin-right:0;margin-left:0;font-size:1.2em;font-weight:500;font-family:Alegreya,serif;color:#8aa485;"" >Type de réservation</p>
		<p style=""margin-top:0;margin-bottom:.75rem;margin-right:0;margin-left:0;font-family:Alegreya,serif;"" >{evaluer.Type}</p>
		<p class=""label"" style=""margin-bottom:.75rem;margin-right:0;margin-left:0;font-size:1.2em;margin-top:2rem;font-weight:500;font-family:Alegreya,serif;color:#8aa485;"" >Courriel</p>
		<p style=""margin-top:0;margin-bottom:.75rem;margin-right:0;margin-left:0;font-family:Alegreya,serif;"" >{evaluer.Courriel}</p>
		<p class=""label"" style=""margin-bottom:.75rem;margin-right:0;margin-left:0;font-size:1.2em;margin-top:2rem;font-weight:500;font-family:Alegreya,serif;color:#8aa485;"" >Date de la visite</p>
		<p style=""margin-top:0;margin-bottom:.75rem;margin-right:0;margin-left:0;font-family:Alegreya,serif;"" >{evaluer.DateVisite:dd MMM yyyy}</p>
		<p class=""label"" style=""margin-bottom:.75rem;margin-right:0;margin-left:0;font-size:1.2em;margin-top:2rem;font-weight:500;font-family:Alegreya,serif;color:#8aa485;"" >Qualité du repas (1 à 5)</p>
		<p style=""margin-top:0;margin-bottom:.75rem;margin-right:0;margin-left:0;font-family:Alegreya,serif;"" >{evaluer.QualiteRepas} sur 5</p>
		<p class=""label"" style=""margin-bottom:.75rem;margin-right:0;margin-left:0;font-size:1.2em;margin-top:2rem;font-weight:500;font-family:Alegreya,serif;color:#8aa485;"" >Qualité du service (1 à 5)</p>
		<p style=""margin-top:0;margin-bottom:.75rem;margin-right:0;margin-left:0;font-family:Alegreya,serif;"" >{evaluer.QualiteService} sur 5</p>
		{(estCommentaire ? $"<p style=\"margin-bottom:.75rem;margin-right:0;margin-left:0;font-size:1.2em;margin-top:2rem;font-weight:500;font-family:Alegreya,serif;color:#8aa485;\" >Commentaires</p><p style=\"margin-top:0;margin-bottom:.75rem;margin-right:0;margin-left:0;font-family:Alegreya,serif;\" >{evaluer.Commentaires}</p>" : string.Empty)}
	</div>
</div>
</body>
</html>
";
        }

        // GET: Evaluers/Edit/5
        [Authorize(Policy = "EstUtilisateur")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var role = await _userManager.GetRolesAsync(user);

            var mesEvaluations = _context.Evaluations.Where(e => e.Courriel == user.Email);

            if (role.First() == "Administrateur")
            {
                mesEvaluations = _context.Evaluations;
            }

            var evaluer = await _context.Evaluations.FindAsync(id);

            // Éviter les /Edit/1 etc..
            if (evaluer == null && !mesEvaluations.Contains(evaluer))
            {
                return NotFound();
            }


            return View(evaluer);
        }

        // POST: Evaluers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Prenom,Nom,Type,Courriel,DateVisite,QualiteRepas,QualiteService,Commentaires")] Evaluer evaluer)
        {
            if (id != evaluer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(evaluer);
                    evaluer.CourrielHtml = EmailHtml(evaluer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EvaluerExists(evaluer.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(evaluer);
        }

        // GET: Evaluers/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evaluer = await _context.Evaluations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (evaluer == null)
            {
                return NotFound();
            }

            return View(evaluer);
        }

        // POST: Evaluers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var evaluer = await _context.Evaluations.FindAsync(id);
            _context.Evaluations.Remove(evaluer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EvaluerExists(int id)
        {
            return _context.Evaluations.Any(e => e.Id == id);
        }
    }
}
