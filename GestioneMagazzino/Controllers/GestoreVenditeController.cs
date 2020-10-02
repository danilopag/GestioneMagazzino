using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GestioneMagazzino.ServiceReference1;
using GestioneMagazzino.Models;
namespace GestioneMagazzino.Controllers
{
    public class GestoreVenditeController : Controller
    {
        ServiceReference1.ServerServiceClient Cserver = new ServiceReference1.ServerServiceClient();

        //Lista dei clienti presenti nel db
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

        //Lista fatture vendita 
        [HttpGet]
        public ActionResult ListaFatturevendita()
        {
            try
            {
                List<ServiceReference1.FatturaVenditaClass> fattureVendita = new List<ServiceReference1.FatturaVenditaClass>();
                fattureVendita = Cserver.FattureVenditaList().ToList();
                double totale = 0; //Variabile fittizzia per calcolare il saldo
                List<ModelFattureVendita> fatture = new List<ModelFattureVendita>();
                foreach (FatturaVenditaClass x in fattureVendita)
                {
                    ModelFattureVendita fatt = new ModelFattureVendita();
                    fatt.Nfattura = x.Nfattura;
                    fatt.DataVendita = x.DataVendita;
                    fatt.Quantita = x.Quantita;
                    fatt.Pagamento = x.Pagamento;
                    fatt.CodCliente = x.CodCliente;
                    fatt.CodUtente = x.CodUtente;
                    fatt.PrezzoVendita = x.PrezzoVendita;
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

        //Aggiungi cliente view
        [HttpGet]
        public ActionResult Aggiungicliente()
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
        public ActionResult Aggiungicliente(ModelClientiList cliente)
        {
            Session["Aggiungicliente"] = false;
            try
            {
                ServiceReference1.ClienteClass cli = new ServiceReference1.ClienteClass();
                bool doRedirect = false;
                cli.Nome = cliente.Nome;
                cli.Partitaiva = cliente.Partitaiva;
                cli.Codicefiscale = cliente.Codicefiscale;
                cli.Indirizzo = cliente.Indirizzo;
                cli.Email = cliente.Email;
                doRedirect = Cserver.InsertCliente(cli.Nome, cli.Partitaiva, cli.Codicefiscale, cli.Indirizzo, cli.Email);
                if (doRedirect == true)
                {
                    Session["Aggiungicliente"] = true; //flag per dire all'utente se l' inserimento è andato a buon fine
                    return RedirectToAction("Aggiungicliente", "GestoreVendite");
                }
                if (doRedirect == false)
                {
                    ModelState.AddModelError("", "Errore: cliente non caricato !!");
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

        //IndexAddettoVendite

        [HttpGet]
        public ActionResult IndexAddettoVendite()
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

        //Aggiungi fattura vendita view
        [HttpGet]
        public ActionResult Aggiungifatturavendita()
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

        //Inizializza fattura view
        [HttpGet]
        public ActionResult Inizializzafatturavendita()
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

        //Termina fattura view
        [HttpGet]
        public ActionResult Terminafatturavendita()
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

        //Aggiungi prodotti alla fattura nel db
        [HttpPost]
        public ActionResult Aggiungifatturavendita(ModelFattureVendita fatture)
        {
            try
            { 
                bool doRedirect = false;
                bool doRedirect1 = false;
                ServiceReference1.ProdottoClass prod = new ServiceReference1.ProdottoClass(); //Var per controllo della Disponibilità
                ServiceReference1.FatturaVenditaClass fatt = new ServiceReference1.FatturaVenditaClass();
                if (Session["Nfattura"] != null)
                {
                    fatt.Nfattura = Session["Nfattura"].ToString();
                }
                else
                {
                    fatt.Nfattura = "1";
                }
                fatt.CodCliente = Session["CodCliente"].ToString();
                fatt.Quantita = fatture.Quantita;
                fatt.Pagamento = Convert.ToBoolean(Session["Pagamento"]);
                fatt.CodUtente = Session["Idutente"].ToString();
                fatt.CodProdotto = fatture.CodProdotto;
                if (fatt.CodProdotto != null && fatt.Quantita > 0 && fatt.Nfattura!=null)
                { //Controllo errori nel campo per non intaccare il DB
                    prod = Cserver.Disprodotto(fatt.CodProdotto);
                    if (prod.Quantita != -1 && prod.Quantita >= fatt.Quantita)
                    {
                        doRedirect = Cserver.InsertFatturaVendita(fatt.Nfattura, fatt.CodUtente, fatt.CodCliente, fatt.CodProdotto, fatt.Quantita, fatt.Pagamento);
                        if (doRedirect == true)
                        {
                            Session["Aggiungifattura"] = true; //flag per dire all'utente se l' inserimento è andato a buon fine
                            doRedirect1 = Cserver.Minusquantitaprodotto(fatt.CodProdotto, fatt.Quantita);
                            if (doRedirect1 == false)
                            {
                                ModelState.AddModelError("", "Errore: Quantita richiesta non disponibile in magazzino");
                                return View();
                            }
                            else
                            {
                                return RedirectToAction("Aggiungifatturavendita", "GestoreVendite");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Errore inserimento prodotto nella fattura");
                            return View();
                        }
                    }
                    else
                    {
                        if (prod.Quantita == -1)
                        {
                            ModelState.AddModelError("", "Errore inserimento prodotto nella fattura");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Errore: Quantita richiesta non disponibile in magazzino");
                        }
                        return View();
                    }
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

        //Inizializza campi fattura salvandoli in var. di Sessione
        [HttpPost]
        public ActionResult InizializzaFatturavendita(ModelFattureVendita modFat)
        {
            try 
            { 
                Session["Aggiungifattura"] = false; //flag per dire all'utente se l' inserimento è andato a buon fine
                //Prendo tutti i Clienti dal Db per controllare e l'id inserito dall utente è corretto
                List<ServiceReference1.ClienteClass> clienti = new List<ServiceReference1.ClienteClass>();
                clienti = Cserver.ClientiList().ToList();
                bool control = false;
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
                //se la sessione è popolata la svuoto 
                if (Convert.ToBoolean(Session["Pagamento"]) == true)
                {
                    Session["Pagamento"] = false;
                }
                if (Session["CodCliente"] != null)
                {
                    Session["CodCliente"] = null;
                }
                List<ServiceReference1.FatturaAcquistoClass> fatt = new List<ServiceReference1.FatturaAcquistoClass>();
                //popolo i campi della sessione 
                if (modFat.CodCliente != null)
                {
                    Session["Nfattura"]= Cserver.NFatturaVendita();
                    Session["Pagamento"] = modFat.Pagamento;
                    Session["CodCliente"] = modFat.CodCliente.ToString();
                    foreach (var x in users)
                    {
                        if (Session["CodCliente"].ToString() == Convert.ToString(x.Idcliente))
                        {
                            control = true;
                        }
                    }
                    if (control == true)
                    {
                        return RedirectToAction("Aggiungifatturavendita", "GestoreVendite");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Errore: Codice Cliente non presente nel Db");
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

        //Set status pagamento Fattura
        [HttpPost]
        public ActionResult Alterpagatofattura(ModelFattureVendita fattura)
        {
            try
            {
                bool doRedirect = false;
                ServiceReference1.FatturaVenditaClass fatt = new ServiceReference1.FatturaVenditaClass();
                fatt.Nfattura = fattura.Nfattura;
                //Prendo tutti le Fattura Vendita dal Db per controllare e l'id inserito dall utente è corretto
                List<ServiceReference1.FatturaVenditaClass> fat = new List<ServiceReference1.FatturaVenditaClass>();
                fat = Cserver.FattureVenditaList().ToList();
                bool control = false; //controlla che il numero di fattura sia presente
                bool control1 = false; //per vedere se risulta già pagata
                List<ModelFattureVendita> fats = new List<ModelFattureVendita>();
                foreach (var x in fat)
                {
                    ModelFattureVendita cliente = new ModelFattureVendita()
                    {
                        Nfattura = x.Nfattura,
                        Pagamento = x.Pagamento,
                    };
                    fats.Add(cliente);
                }
                if (fatt.Nfattura != null)
                {
                    if (fatt.Nfattura.Trim() != "")
                    {
                        foreach (var x in fats)
                        {
                            if (fatt.Nfattura.ToString() == Convert.ToString(x.Nfattura))
                            {
                                control = true;
                                if (x.Pagamento == false)
                                {
                                    control1 = true; 
                                }
                            }
                        }
                        if (control == true && control1 == true)
                        {
                            doRedirect = Cserver.AlterPagatoFattura(fatt.Nfattura);
                            if (doRedirect == true)
                            {
                                return RedirectToAction("Listafatturevendita", "GestoreVendite");
                            }
                            if (doRedirect == false)
                            {
                                ModelState.AddModelError("", "Errore: impossibile cambiare status della fattura.");
                                return View();
                            }
                        }
                        if(control == false)
                        {
                            ModelState.AddModelError("", "Errore: il numero fattura non esiste nel DB.");
                            return View();
                        }
                        else
                        {
                            ModelState.AddModelError("", "Errore: La fattura risulta già pagata!");
                            return View();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Errore: impossibile cambiare status della fattura.");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Errore: impossibile cambiare status della fattura.");
                    return View();
                }

            }
            catch (Exception) //Se il è server chiuso
            {
                ModelState.AddModelError("", "Connessione al server fallita.");
                return View();
            }
        }

        //Alter Set Pagamento fattura view
        [HttpGet]
        public ActionResult Alterpagatofattura()
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
