using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Zhao.Backend.Extra;
using Zhao.Backend.Models;

namespace Zhao.Backend.Controllers
{
    public static class ReservationsFiltre
    {
        public static bool EstFiltrer { get; set; }
    }

    public class ReservationsController : Controller
    {
        private readonly TpContext _context;
        private UserManager<IdentityUser> _userManager;

        public ReservationsController(TpContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Reservations
        [Authorize]
        public async Task<IActionResult> Index()
        {
            ReservationsFiltre.EstFiltrer = false;

            var user = await _userManager.GetUserAsync(User);
            var role = await _userManager.GetRolesAsync(user);

            var mesReservations = _context.Reservations.Where(e => e.Courriel == user.Email);

            if (role.First() == "Administrateur")
            {
                mesReservations = _context.Reservations;
            }

            if (!mesReservations.Any() && role.First() != "Administrateur")
            {
                return RedirectToAction("Create");
            }

            return View(await mesReservations.ToListAsync());
        }

        [Authorize(Roles = "Administrateur")]
        public async Task<IActionResult> FiltreConfirmer()
        {
            ReservationsFiltre.EstFiltrer = true;
            return View("Index", _context.Reservations.Where(r => r.Confirmation == "Confirmée"));
        }

        [Authorize(Roles = "Administrateur")]
        public async Task<IActionResult> FiltreNonConfirmer()
        {
            ReservationsFiltre.EstFiltrer = true;
            return View("Index", _context.Reservations.Where(r => r.Confirmation == "Non-confirmée"));
        }

        // GET: Reservations/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var role = await _userManager.GetRolesAsync(user);

            var mesReservations = _context.Reservations.Where(e => e.Courriel == user.Email);

            if (role.First() == "Administrateur")
            {
                mesReservations = _context.Reservations;
            }

            var reservation = await mesReservations
                .FirstOrDefaultAsync(m => m.Id == id);

            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);

            var reservation = new Reservation();

            if (!string.IsNullOrEmpty(user.PhoneNumber))
            {
                reservation.Telephone = user.PhoneNumber;
            }

            return View(reservation);
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Prenom,Nom,Type,Courriel,DateHeure,Telephone,NbPersonnes,Confirmation")] Reservation reservation)
        {
            var user = await _userManager.GetUserAsync(User);
            var role = await _userManager.GetRolesAsync(user);

            reservation.Id = !_context.Reservations.Any() ? 1 : _context.Reservations.Max(e => e.Id) + 1;


            if (role.First() != "Administrateur")
            {
                reservation.Confirmation = "Non-confirmée";
                reservation.Courriel = user.Email;
            }

            reservation.CourrielHtml = EmailHtml(reservation);

            _context.Add(reservation);
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
                Body = EmailHtml(reservation),
                Subject = "Zhào - Confirmation de la réservation"
            };
            mail.To.Add(new MailAddress(user.Email));
            smtpClient.Send(mail);

            Notification.Message = "Vous venez de réserver une table avec succès.";

            return RedirectToAction(nameof(Index));
        }
        private static string EmailHtml(Reservation reservation)
        {
            var nbPersonnesPluriel = reservation.NbPersonnes > 1 ? "personnes" : "personne";
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
	<h2 style=""line-height:1.2;font-family:Alegreya,serif;font-weight:500;text-align:center;font-size:2em;margin-top:0;margin-bottom:2rem;margin-right:0;margin-left:0;"" >Votre réservation à bien été enregistré</h2>
	<div>

	</div>
	<p style=""max-width:50ch;text-align:center;margin-left:auto;margin-right:auto;margin-top:0;margin-bottom:1.5rem;font-family:Alegreya,serif;"" >
		Bonjour {reservation.Prenom} {reservation.Nom}, votre réservation a <mark style=""background-color:transparent;background-image:linear-gradient(transparent 50%,#ecf0eb 0);"" >biens été placé</mark>.
		Voici les informations qui ont été <mark style=""background-color:transparent;background-image:linear-gradient(transparent 50%,#ecf0eb 0);"" >enregistré</mark> dans notre système:
	</p>
</div>
<div class=""info"" style=""clip-path:polygon(0 0,100% 75px,100% 100%,0 calc(100% - 75px));background-color:#181818;padding-bottom:5.25rem;text-align:center;color:#f8f8f8;"" >
	<div class=""info__list"" style=""padding-top:5.25rem;padding-left:0;max-width:32rem;width:calc(100% - 6.5rem);margin-right:auto;margin-left:auto;"" >
		<p class=""label"" style=""margin-top:0;margin-bottom:.75rem;margin-right:0;margin-left:0;font-size:1.2em;font-weight:500;font-family:Alegreya,serif;color:#8aa485;"" >Type de réservation</p>
		<p style=""margin-top:0;margin-bottom:.75rem;margin-right:0;margin-left:0;font-family:Alegreya,serif;"" >{reservation.Type}</p>
		<p class=""label"" style=""margin-bottom:.75rem;margin-right:0;margin-left:0;font-size:1.2em;margin-top:2rem;font-weight:500;font-family:Alegreya,serif;color:#8aa485;"" >Courriel</p>
		<p style=""margin-top:0;margin-bottom:.75rem;margin-right:0;margin-left:0;font-family:Alegreya,serif;"" >{reservation.Courriel}</p>
		<p class=""label"" style=""margin-bottom:.75rem;margin-right:0;margin-left:0;font-size:1.2em;margin-top:2rem;font-weight:500;font-family:Alegreya,serif;color:#8aa485;"" >Date et heure</p>
		<p style=""margin-top:0;margin-bottom:.75rem;margin-right:0;margin-left:0;font-family:Alegreya,serif;"" >{reservation.DateHeure:dd MMM yyyy à HH:mm}</p>
		<p class=""label"" style=""margin-bottom:.75rem;margin-right:0;margin-left:0;font-size:1.2em;margin-top:2rem;font-weight:500;font-family:Alegreya,serif;color:#8aa485;"" >Numéro de téléphone</p>
		<p style=""margin-top:0;margin-bottom:.75rem;margin-right:0;margin-left:0;font-family:Alegreya,serif;"" >{reservation.Telephone}</p>
		<p class=""label"" style=""margin-bottom:.75rem;margin-right:0;margin-left:0;font-size:1.2em;margin-top:2rem;font-weight:500;font-family:Alegreya,serif;color:#8aa485;"" >Nombre de personne(s)</p>
		<p style=""margin-top:0;margin-bottom:.75rem;margin-right:0;margin-left:0;font-family:Alegreya,serif;"" >{reservation.NbPersonnes} {nbPersonnesPluriel}</p>
	</div>
</div>
</body>
</html>
";
        }

        // GET: Reservations/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var role = await _userManager.GetRolesAsync(user);

            var mesReservations = _context.Reservations.Where(e => e.Courriel == user.Email);

            if (role.First() == "Administrateur")
            {
                mesReservations = _context.Reservations;
            }

            var reservation = await _context.Reservations.FindAsync(id);

            // Éviter les /Edit/1 etc..
            if (reservation == null && !mesReservations.Contains(reservation))
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Prenom,Nom,Type,Courriel,DateHeure,Telephone,NbPersonnes,Confirmation")] Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);
            var role = await _userManager.GetRolesAsync(user);

            if (role.First() != "Administrateur")
            {
                reservation.Courriel = user.Email;
                reservation.Confirmation = "Non-confirmée";
            }

            try
            {
                _context.Update(reservation);
                reservation.CourrielHtml = EmailHtml(reservation);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservationExists(reservation.Id))
                {
                    return NotFound();
                }

                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Reservations/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }
    }
}
