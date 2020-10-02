using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GestioneMagazzino.ServiceReference1;
using GestioneMagazzino.Models;
namespace GestioneMagazzino.Controllers
{
    public class AdminController : Controller
    {
        //Lista dei clienti presenti db
        ServiceReference1.ServerServiceClient Cserver = new ServiceReference1.ServerServiceClient();

        [HttpGet]
        public ActionResult Listaclienti()
        {
            try
            { 
                List<ServiceReference1.ClienteClass> clienti = new List<ServiceReference1.ClienteClass>();
                clienti = Cserver.ClientiList().ToList();

                List<ModelClientiList> users = new List<ModelClientiList>();
                foreach (ClienteClass x in clienti)
                {
                    ModelClientiList cliente = new ModelClientiList()
                    {
                        Idcliente = x.Idcliente,
                        Nome = x.Nome,
                        Partitaiva = x.Partitaiva,
                        Codicefiscale = x.Codicefiscale,
                        Indirizzo = x.Indirizzo,
                        Email = x.Email,
                    };
                    users.Add(cliente);
                }
                return View(users);
            }
            catch (Exception) //Se il è server chiuso
            {
                ModelState.AddModelError("", "Connessione al server fallita.");
                return View();
            }
        }
        
        //Lista prodotti presenti sul db
        [HttpGet]
        public ActionResult Listaprodotti()
        {
            try
            {
                List<ServiceReference1.ProdottoClass> prodotti = new List<ServiceReference1.ProdottoClass>();
                prodotti = Cserver.ProdottiList().ToList();

                List<ModelProdottiList> prod = new List<ModelProdottiList>();
                foreach (ProdottoClass x in prodotti)
                {
                    ModelProdottiList prodotto = new ModelProdottiList()
                    {
                        Idprodotto = x.Idprodotto,
                        Nome = x.Nome,
                        Descrizione = x.Descrizione,
                        Prezzo = x.Prezzo,
                        Quantita = x.Quantita,
                    };
                    if (prodotto.Quantita <= 5) {
                        prodotto.Inesaurimento = true;
                    }
                    else {
                        prodotto.Inesaurimento = false;
                    }
                    prod.Add(prodotto);
                }
                return View(prod);
            }
            catch (Exception) //Se il è server chiuso
            {
                ModelState.AddModelError("", "Connessione al server fallita.");
                return View();
            }
        }



        //Lista fornitori presenti sul db
        [HttpGet]
        public ActionResult Listafornitori()
        {
            try
            {
            List<ServiceReference1.FornitoreClass> fornitori = new List<ServiceReference1.FornitoreClass>();
            fornitori = Cserver.FornitoriList().ToList();

            List<ModelFornitoriList> users = new List<ModelFornitoriList>();
            foreach (FornitoreClass x in fornitori)
            {
                ModelFornitoriList fornitore = new ModelFornitoriList()
                {
                    Idfornitore = x.Idfornitore,
                    Nome = x.Nome,
                    Partitaiva = x.Partitaiva,
                    Indirizzo = x.Indirizzo,
                    Email = x.Email,
                };
                users.Add(fornitore);
            }
            return View(users);
            }
            catch (Exception) //Se il è server chiuso
            {
                ModelState.AddModelError("", "Connessione al server fallita.");
                return View();
            }
        }
        //lista utenti presenti sul db
        [HttpGet]
        public ActionResult Listautenti()
        {
            try
            {
                List<ServiceReference1.UtenteClass> utenti = new List<ServiceReference1.UtenteClass>();
                utenti = Cserver.UtentiList().ToList();

                List<ModelUtenteList> users = new List<ModelUtenteList>();
                foreach (UtenteClass x in utenti)
                {
                    ModelUtenteList user = new ModelUtenteList()
                    {
                        Idutente = x.Idutente,
                        Nome = x.Nome,
                        Cognome = x.Cognome,
                        Password = x.Password,
                        IsAdmin = x.IsAdmin,
                    };
                    users.Add(user);
                }
                return View(users);
            }
            catch (Exception) //Se il è server chiuso
            {
                ModelState.AddModelError("", "Connessione al server fallita.");
                return View();
            }
        }

        //Lista fatture vendita
        [HttpGet]
        public ActionResult ListaFatturevendita()
        {
            try
            {
                List<ServiceReference1.FatturaVenditaClass> fattureVendita = new List<ServiceReference1.FatturaVenditaClass>();
                fattureVendita = Cserver.FattureVenditaList().ToList();
                double totale = 0; //Serve per la classe fittizzia per ritornare il saldo
                List<ModelFattureVendita> fatture = new List<ModelFattureVendita>();
                foreach (FatturaVenditaClass x in fattureVendita)
                {
                    ModelFattureVendita fatt = new ModelFattureVendita()
                    {
                        Nfattura = x.Nfattura,
                        DataVendita = x.DataVendita,
                        Quantita = x.Quantita,
                        Pagamento = x.Pagamento,
                        CodCliente = x.CodCliente,
                        CodUtente = x.CodUtente,
                        PrezzoVendita = x.PrezzoVendita,
                    };
                    if (fatt.Pagamento == true)
                    {
                        totale += fatt.PrezzoVendita;
                    }
                    fatture.Add(fatt);
                }
                ModelFattureVendita f = new ModelFattureVendita() // Oggetto Fittizio per Saldo
                {
                    Quantita = -1,
                    PrezzoVendita = totale,
                };
                fatture.Add(f);
                return View(fatture);
            }
            catch (Exception) //Se il è server chiuso
            {
                ModelState.AddModelError("", "Connessione al server fallita.");
                return View();
            }
        }

        //Lista fatture acquisto
        [HttpGet]
        public ActionResult ListaFattureacquisto()
        {
            try
            {
                List<ServiceReference1.FatturaAcquistoClass> fattureAcquisto = new List<ServiceReference1.FatturaAcquistoClass>();
                fattureAcquisto = Cserver.FattureAcquistoList().ToList();
                double totale = 0; //Serve per la classe fittizzia per ritornare il saldo
                List<ModelFattureAcquisto> fatture = new List<ModelFattureAcquisto>();
                foreach (FatturaAcquistoClass x in fattureAcquisto)
                {
                    ModelFattureAcquisto fatt = new ModelFattureAcquisto()
                    {
                        Nfattura = x.Nfattura,
                        DataAcquisto = x.DataAcquisto,
                        PrezzoAcquisto = x.PrezzoAcquisto,
                        Quantita = x.Quantita,
                        CodFornitore = x.CodFornitore,
                        CodUtente = x.CodUtente,
                  
                    };
                    totale += fatt.PrezzoAcquisto;
                    fatture.Add(fatt);
                }
                ModelFattureAcquisto f = new ModelFattureAcquisto() // Oggetto Fittizio per Saldo
                {
                    Quantita = -1,
                    PrezzoAcquisto = totale,
                };
                fatture.Add(f);
                return View(fatture);
            }
            catch (Exception) //Se il è server chiuso
            {
                ModelState.AddModelError("", "Connessione al server fallita.");
                return View();
            }
        }

        //Logout view
        [HttpGet]
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

        //Admin index view
        [HttpGet]
        public ActionResult Adminindex()
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

        //Aggiungi utente view
        [HttpGet]
        public ActionResult Aggiungiutente()
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

        //Aggiungi utente al db
        [HttpPost]
        public ActionResult Aggiungiutente(ModelUtenteList utente)
        {
            try
            {
                ServiceReference1.UtenteClass user = new ServiceReference1.UtenteClass();
                bool doRedirect = false;
                int tmp = 5;
                user.Nome = utente.Nome;
                user.Cognome = utente.Cognome;
                user.Password = utente.Password;
                user.IsAdmin = utente.IsAdmin;
                tmp = user.IsAdmin;
                if (tmp > -1 && tmp < 3)
                {
                    doRedirect = Cserver.RegisterUtente(user.Nome, user.Cognome, user.Password, user.IsAdmin);
                    if (doRedirect == true)
                    {
                        return RedirectToAction("Aggiungiutente", "Admin");
                    }
                    if (doRedirect == false)
                    {
                        ModelState.AddModelError("", "Errore: utente non caricato !!");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Errore: utente non caricato !!");
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

        //Aggiungi prodotto view
        [HttpGet]
        public ActionResult Aggiungiprodotto()
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

        //Aggiungi prodotto al db
        [HttpPost]
         public ActionResult Aggiungiprodotto(ModelProdottiList prodotto)
        {
            try
            { 
                ServiceReference1.ProdottoClass prod = new ServiceReference1.ProdottoClass();
                bool doRedirect = false;
                prod.Idprodotto = prodotto.Idprodotto;
                prod.Nome = prodotto.Nome;
                prod.Descrizione = prodotto.Descrizione;
                prod.Prezzo = prodotto.Prezzo;
                prod.Quantita = prodotto.Quantita;
                if (prod.Prezzo > 0 && prod.Quantita > -1)
                {
                    doRedirect = Cserver.InsertProdotto(prod.Idprodotto, prod.Nome, prod.Descrizione, prod.Prezzo, prod.Quantita);
                    if (doRedirect == true)
                    {
                        return RedirectToAction("Aggiungiprodotto", "Admin");
                    }
                    if (doRedirect == false)
                    {
                        ModelState.AddModelError("", "Errore: prodotto non caricato !!");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Errore: prodotto non caricato !!");
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

        //Modifica prodotto nome view
        [HttpGet]
        public ActionResult Modificaprodottonome()
        {
            ViewBag.Message = " Modifica nome prodotto";
            return View();

        }
        //Modifica nome di prodotto nel Db
        [HttpPost]
        public ActionResult Modificaprodottonome(ModelProdottiList prodotto)
        {
            try
            {
                bool doRedirect = false;
                ServiceReference1.ProdottoClass prod = new ServiceReference1.ProdottoClass();
                prod.Idprodotto = prodotto.Idprodotto;
                prod.Nome = prodotto.Nome;
                if (prod.Nome !=null)
                {
                    if (prod.Nome.Trim() != "")
                    {
                        doRedirect = Cserver.AlterNomeProdotto(prod.Idprodotto, prod.Nome);
                        if (doRedirect == true)
                        {
                            return RedirectToAction("Listaprodotti", "Admin");
                        }
                        if (doRedirect == false)
                        {
                            ModelState.AddModelError("", "Errore: impossibile modificare il nome del prodotto.");
                            return View();
                        }
                        return View();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Errore: impossibile modificare il nome del prodotto.");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Errore: impossibile modificare il nome del prodotto.");
                    return View();
                }
                
            }
            catch (Exception) //Se il è server chiuso
            {
                ModelState.AddModelError("", "Connessione al server fallita.");
                return View();
            }
        }

        //Modica prezzo prodotto View
        [HttpGet]
        public ActionResult Modificaprodottoprezzo()
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

        //Modicica ATTRIBUTO prezzo di un Prodotto nel DB
        [HttpPost]
        public ActionResult Modificaprodottoprezzo(ModelProdottiList prodotto)
        {
            try
            {
                bool doRedirect = false;
                ServiceReference1.ProdottoClass prod = new ServiceReference1.ProdottoClass();
                prod.Idprodotto = prodotto.Idprodotto;
                prod.Prezzo = prodotto.Prezzo;
                if (prod.Prezzo > 0)
                {
                    doRedirect = Cserver.AlterPrezzoProdotto(prod.Idprodotto, prod.Prezzo);
                    if (doRedirect == true)
                    {
                        return RedirectToAction("Listaprodotti", "Admin");
                    }
                    if (doRedirect == false)
                    {
                        ModelState.AddModelError("", "Errore: impossibile modificare il prezzo del prodotto.");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Errore: impossibile modificare il prezzo del prodotto.");
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

        //modifica Admin view
        [HttpGet]
        public ActionResult Modificaisadmin()
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

        //Modica ATTRIBUTO isAdmin nel DB di un Utente
        [HttpPost]
        public ActionResult Modificaisadmin(ModelUtenteList utente)
        {
            try 
            { 
                bool doRedirect = false;
                ServiceReference1.UtenteClass ut = new ServiceReference1.UtenteClass();
                ut.Idutente = utente.Idutente;
                ut.IsAdmin = utente.IsAdmin;
                if (ut.IsAdmin > -1 && ut.IsAdmin<3)
                {
                    doRedirect = Cserver.SetUtenteisAdmin(ut.Idutente, ut.IsAdmin);
                    if (doRedirect == true)
                    {
                        return RedirectToAction("Listautenti", "Admin");
                    }
                    if (doRedirect == false)
                    {
                        ModelState.AddModelError("", "Errore: impossibile modificare la tipologia dell'utente");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Errore: impossibile modificare la tipologia dell'utente");
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
    }
}
