using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WCFServer
{
    // NOTA: è possibile utilizzare il comando "Rinomina" del menu "Refactoring" per modificare il nome di interfaccia "IService1" nel codice e nel file di configurazione contemporaneamente.
    [ServiceContract]
    public interface IServerService
    {
        [OperationContract]
        bool DoWork();

        [OperationContract]
        bool RegisterUtente (string Nome, string Cognome, string Password, int IsAdmin);

        [OperationContract]
        int LoginUtente(string IDutente, string Password);

        [OperationContract]
        UtenteClass GetUtente(string IDutente);

        [OperationContract]
        bool SetUtenteisAdmin(string IDutente, int status);

        [OperationContract]
        List<UtenteClass> UtentiList();

        [OperationContract]
        List<FornitoreClass> FornitoriList();

        [OperationContract]
        bool InsertFornitore (string Nome, string PartitaIVA, string Email, string Indirizzo);

        [OperationContract]
        List<ClienteClass> ClientiList();

        [OperationContract]
        bool InsertCliente(string Nome, string PartitaIVA, string Codicefiscale, string Indirizzo, string Email);

        [OperationContract]
        List<ProdottoClass> ProdottiList();

        [OperationContract]
        bool InsertProdotto(string IDprodotto, string Nome, string Descrizione, double Prezzo, int Quantita);

        [OperationContract]
        bool Addquantitaprodotto(string IDprodotto, int Quantita);

        [OperationContract]
        bool Minusquantitaprodotto(string IDprodotto, int Quantita);

        [OperationContract]
        ProdottoClass Disprodotto(string Idprodotto);

        [OperationContract]
        bool AlterNomeProdotto(string IDprodotto, string Nome);

        [OperationContract]
        bool AlterPrezzoProdotto(string IDprodotto, double Prezzo);

        [OperationContract]
        List<FatturaAcquistoClass> FattureAcquistoList();

        [OperationContract]
        List<FatturaVenditaClass> FattureVenditaList();

        [OperationContract]
        bool AlterPagatoFattura(string Nfattura);

        [OperationContract]
        bool InsertFatturaVendita(string Nfattura, string CodUtente,string CodCliente,string CodProdotto, int Quantita, bool Pagamento);

        [OperationContract]
        bool InsertFatturaAcquisto(string Nfattura, string CodUtente, string CodFornitore, string CodProdotto, int Quantita, double PrezzoAcquisto);

        [OperationContract]
        string NFatturaVendita(); //Restitutisce l'ultimo Nfattura Vendita
    }
}
