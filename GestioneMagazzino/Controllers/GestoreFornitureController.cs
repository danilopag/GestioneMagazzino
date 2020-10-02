using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GestioneMagazzino.ServiceReference1;
using GestioneMagazzino.Models;
using System.ServiceModel.Description;

namespace GestioneMagazzino.Controllers
{
    public class GestoreFornitureController : Controller
    {
        //Lista dei clienti presenti db
        ServiceReference1.ServerServiceClient Cserver = new ServiceReference1.ServerServiceClient();

       
        //lista prodotti presenti sul db
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
                    if (prodotto.Quantita <= 5)
                    {
                        prodotto.Inesaurimento = true;
                    }
                    else
                    {
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



        //lista fornitori presenti sul db
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

        //Lista Fatture Acquisto 
        [HttpGet]
        public ActionResult ListaFattureacquisto()
        {
            try
            {
                List<ServiceReference1.FatturaAcquistoClass> fattureAcquisto = new List<ServiceReference1.FatturaAcquistoClass>();
                fattureAcquisto = Cserver.FattureAcquistoList().ToList();
                double totale = 0; //Variabile fittizzia per calcolare il saldo
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

        //Aggiungi fornitore view
        [HttpGet]
        public ActionResult Aggiungifornitore()
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

        //Aggiungi fornitore al db
        [HttpPost]
        public ActionResult Aggiungifornitore(ModelFornitoriList fornitore)
        {
            Session["Aggiungifornitore"] = false;
            try
            {
                ServiceReference1.FornitoreClass forn = new ServiceReference1.FornitoreClass();
                bool doRedirect = false;
                forn.Nome = fornitore.Nome;
                forn.Partitaiva = fornitore.Partitaiva;
                forn.Indirizzo = fornitore.Indirizzo;
                forn.Email = fornitore.Email;
                doRedirect = Cserver.InsertFornitore(forn.Nome, forn.Partitaiva, forn.Email, forn.Indirizzo);
                if (doRedirect == true)
                {
                    Session["Aggiungifornitore"] = true; //flag per dire all'utente se l' inserimento è andato a buon fine
                    return RedirectToAction("Aggiungifornitore", "GestoreForniture");
                }
                if (doRedirect == false)
                {
                    ModelState.AddModelError("", "Errore: fornitore non caricato !!");
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

        //Index addetto forniture view
        [HttpGet]
        public ActionResult IndexAddettoForniture()
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

        //Aggiungi fattura acquisto view
        [HttpGet]
        public ActionResult Aggiungifatturaacquisto()
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

        //Inizializza fattura acquisto view
        [HttpGet]
        public ActionResult Inizializzafatturaacquisto()
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

        //Termina fattura acquisto view
        [HttpGet]
        public ActionResult Terminafatturaacquisto()
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

        //Aggiunge prodotti a fattura nel db
        [HttpPost]
        public ActionResult Aggiungifatturaacquisto(ModelFattureAcquisto fatture)
        {
            try
            { 
                bool doRedirect = false;
                bool doRedirect1 = false;
                ServiceReference1.FatturaAcquistoClass fatt = new ServiceReference1.FatturaAcquistoClass();
                fatt.Nfattura = Session["Nfattura"].ToString();
                fatt.CodFornitore = Session["CodFornitore"].ToString();
                fatt.Quantita = fatture.Quantita;
                fatt.PrezzoAcquisto = fatture.PrezzoAcquisto;
                fatt.CodUtente = Session["Idutente"].ToString();
                fatt.CodProdotto = fatture.CodProdotto;
                if (fatt.CodProdotto != null && fatt.Quantita > 0 && fatt.PrezzoAcquisto > 0)
                { //Controllo errori nel campo per non intaccare il DB
                    doRedirect = Cserver.InsertFatturaAcquisto(fatt.Nfattura, fatt.CodUtente, fatt.CodFornitore, fatt.CodProdotto, fatt.Quantita, fatt.PrezzoAcquisto);
                    if (doRedirect == true)
                    {
                        Session["Aggiungifattura"] = true; //flag per dire all'utente se l' inserimento è andato a buon fine
                        doRedirect1 = Cserver.Addquantitaprodotto(fatt.CodProdotto, fatt.Quantita);
                        if (doRedirect1 == false)
                        {
                            ModelState.AddModelError("", "Errore aggiunta prodotto al DB");
                            return View();
                        }
                        else
                        {
                            return RedirectToAction("Aggiungifatturaacquisto", "GestoreForniture");
                        }
                    }
                    ModelState.AddModelError("", "Errore inserimento prodotto nella fattura");
                    return View();
                }
                else
                {
                    ModelState.AddModelError("", "Errore inserimento prodotto nella fattura");
                    return View();
                }
            }
            catch (Exception) //Se il è server chiuso
            {
                ModelState.AddModelError("", "Connessione al server fallita.");
                return View();
            }
        }

        //Inizializza i campi che servono per la fattura nelle var. di Sessione
        [HttpPost]
        public ActionResult InizializzaFatturaacquisto(ModelFattureAcquisto modFat)
        {
            try
            { 
                Session["Aggiungifattura"] = false; //flag per dire all'utente se l' inserimento è andato a buon fine
                //Prendo tutti i fornitori dal Db per controllare e l'id inserito dall utente è corretto
                List<ServiceReference1.FornitoreClass> fornitori = new List<ServiceReference1.FornitoreClass>();
                fornitori = Cserver.FornitoriList().ToList();
                bool control = false;
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
                //se la sessione è popolata la svuoto 
                if (Session["Nfattura"]!=null)
                {
                    Session["Nfattura"]=null;
                }

                if (Session["CodFornitore"] != null)
                {
                    Session["CodFornitore"] = null;
                }
                List<ServiceReference1.FatturaAcquistoClass> fatt = new List<ServiceReference1.FatturaAcquistoClass>();
                //popolo i campi della sessione 
                if (modFat.Nfattura != null && modFat.CodFornitore != null)
                {
                    Session["Nfattura"] = modFat.Nfattura.ToString();
                    Session["CodFornitore"] = modFat.CodFornitore.ToString();
                    foreach (var x in users)
                    {
                        if (Session["CodFornitore"].ToString().ToUpper() == Convert.ToString(x.Idfornitore))
                        {
                            control = true;
                        }
                    }
                    if (control == true)
                    {
                        return RedirectToAction("Aggiungifatturaacquisto", "GestoreForniture");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Errore: Codice Fornitore non presente nel Db");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Errore: Inserisci tutti i campi");
                    return View();
                }
            }
            catch (Exception) //Se il è server chiuso
            {
                ModelState.AddModelError("", "Connessione al server fallita.");
                return View();
            }
        }

    }
}
