using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GestioneMagazzino.Models;

namespace GestioneMagazzino.Controllers
{
    public class LoginController : Controller
    {
        //connessione al servizio,serve per usare i metodi del server
        ServiceReference1.ServerServiceClient Cserver = new ServiceReference1.ServerServiceClient();
        
        [HttpGet]
        public ActionResult Login()
        {
            try
            {
                return View();
            }
            catch (Exception) //Se il è server chiuso
            {
                ModelState.AddModelError("", "Connessione al server fallita.");
                return View();
            }
        }

        [HttpPost]
        public ActionResult Login(ModelLogin modlog)
        {
            try
            {
                int checkTypeOfUser = Cserver.LoginUtente(modlog.Idutente, modlog.Password);

                //amministratore
                if (checkTypeOfUser == 0)
                {
                    Session["idUtente"] = modlog.Idutente.ToString();
                    Session["isAdmin"] = 0; //Codice Amministratore
                    return RedirectToAction("Adminindex", "Admin");
                }
                //addetto alle vendite
                if (checkTypeOfUser == 1)
                {
                    Session["idUtente"] = modlog.Idutente.ToString();
                    Session["isAdmin"] = 1; //Codice addetto vendite
                    return RedirectToAction("IndexAddettoVendite", "GestoreVendite");
                }
                //addetto alle forniture
                if (checkTypeOfUser == 2)
                {

                    Session["idUtente"] = modlog.Idutente.ToString();
                    Session["isAdmin"] = 2; //Codice addetto forniture
                    return RedirectToAction("IndexAddettoForniture", "GestoreForniture");
                }
                //credenziali errate
                if (checkTypeOfUser == -1)
                {

                    ModelState.AddModelError("", "Tentativo di accesso non valido.");
                    return View();

                }

                //connessione server fallita
                if (checkTypeOfUser == -2)
                {

                    ModelState.AddModelError("", "Connessione al server fallita.");
                    return View();

                }
                //errore generico
                if (checkTypeOfUser == -100)
                {

                    ModelState.AddModelError("", "Error.");
                    return View();

                }
                return View();
            }
            catch (Exception) //Se il è server chiuso
            {
                ModelState.AddModelError("", "Connessione al server fallita.");
                return View();
            }
        }
        //le variabili di sessione vengono svuotate direttamente dalla vista "logout.cshtml"
        public ActionResult Logout()
        {
            try
            {
                return View();
            }
            catch (Exception) //Se il è server chiuso
            {
                ModelState.AddModelError("", "Connessione al server fallita.");
                return View();
            }
        }
    }
}