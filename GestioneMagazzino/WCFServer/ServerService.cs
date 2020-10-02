using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

namespace WCFServer
{
    // NOTA: è possibile utilizzare il comando "Rinomina" del menu "Refactoring" per modificare il nome di classe "Service1" nel codice e nel file di configurazione contemporaneamente.
    public class ServerService : IServerService
    {
        private static readonly string conn = "Server=DESKTOP-SLQDO06\\SQLEXPRESS;Database=Magazzino;integrated security=True;";

        public bool DoWork()
        {
            return true;
        }

        //REGISTRAZIONE DELL'UTENTE NEL DB
        public bool RegisterUtente(string Nome, string Cognome, string Password, int IsAdmin)
        {
            int tmp=0; //Variabile d'appoggio per il codice
            try
            {
                if (Nome != null && Cognome != null && Password != null)
                {
                    SqlConnection connection = new SqlConnection(conn);
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        //Cerco sul DB l'ultimo indice
                        string maxstring = "SELECT MAX(IDUTENTE) AS ID FROM UTENTE";
                        SqlCommand query = new SqlCommand(maxstring, connection);
                        SqlDataReader reader = query.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                tmp = Convert.ToInt32(reader["ID"]) + 1;
                            }
                            reader.Close();
                        }
                        else
                        {
                            reader.Close();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Connessione DB fallita\n");
                        connection.Close();
                        connection.Dispose();
                        return false;
                    }
                    if (connection.State == ConnectionState.Open)
                    {
                        SqlTransaction transaction = connection.BeginTransaction();
                        SqlCommand query = connection.CreateCommand();
                        query.Connection = connection;
                        query.Transaction = transaction;
                        try
                        {
                            query.CommandText = "INSERT INTO UTENTE (IDUTENTE, NOME, COGNOME, PASSWORD, ISADMIN) VALUES ('" + tmp.ToString() + "', '" + Nome.ToUpper() + "', '" + Cognome.ToUpper() + "', '" + Password + "', '" + IsAdmin + "')";
                            query.ExecuteNonQuery();
                            transaction.Commit();
                            connection.Close();
                            connection.Dispose();
                            return true;
                        }
                        catch (Exception ex) //eventuali errori della transazione
                        {
                            Console.WriteLine(ex.ToString());
                            transaction.Rollback();
                            connection.Close();
                            connection.Dispose();
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Connessione DB fallita\n");
                        connection.Close();
                        connection.Dispose();
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        //LOGIN UTENTE Restituisce gli accessi in base al tipo
        public int LoginUtente(string IDutente, string Password)
        {
            int tmp = -100;
            try
            {
                SqlConnection connection = new SqlConnection(conn);
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    //Cerco sul DB se credenziali corrette 
                    string login_string = "SELECT * FROM UTENTE WHERE IDUTENTE = '" + IDutente + "' AND PASSWORD = '" + Password + "'";
                    SqlCommand query = new SqlCommand(login_string, connection);
                    SqlDataReader reader = query.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            tmp = Convert.ToInt32(reader["ISADMIN"]);
                        }
                        reader.Close();
                        connection.Close();
                        connection.Dispose();
                        return tmp;
                    }
                    else
                    {
                        connection.Close();
                        connection.Dispose();
                        return -1;
                    }
                }
                else
                {
                    Console.WriteLine("Connessione DB fallita\n");
                    connection.Close();
                    connection.Dispose();
                    return -2;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return -100;
            }

        }

        //RESTITUISCE OGGETTO DI TIPO UTENTE in base alla stringa Idutente passata come parametro
        public UtenteClass GetUtente(string IDutente)
        {
            UtenteClass Utente = new UtenteClass();
            try
            {
                SqlConnection connection = new SqlConnection(conn);
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    string strQuery = "SELECT * FROM UTENTE WHERE IDUTENTE = '" + IDutente + "'";
                    SqlCommand cmd = new SqlCommand(strQuery, connection);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Utente.Idutente = reader.GetString(0);
                        Utente.Nome = reader.GetString(1);
                        Utente.Cognome = reader.GetString(2);
                        Utente.Password = reader.GetString(3);
                        Utente.IsAdmin = Convert.ToInt32(reader.GetString(4));
                    }
                    reader.Close();
                    connection.Close();
                    connection.Dispose();
                    return Utente;
                }
                else
                {
                    connection.Close();
                    connection.Dispose();
                    return Utente;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return Utente;
            }
        }

        //CAMBIA AUTORIZZAZIONI/RUOLO ALL'UTENTE in base alle disposizioni dell'Admin
        public bool SetUtenteisAdmin(string IDutente, int status)
        {
            try
            {
                SqlConnection connection = new SqlConnection(conn);
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    SqlTransaction transaction = connection.BeginTransaction();
                    SqlCommand query = connection.CreateCommand();
                    query.Connection = connection;
                    query.Transaction = transaction;
                    try
                    {
                        query.CommandText = "UPDATE UTENTE SET ISADMIN = '" + Convert.ToInt32(status) + "' WHERE UTENTE.IDUTENTE = '" + IDutente +"'";
                        query.ExecuteNonQuery();
                        transaction.Commit();
                        connection.Close();
                        connection.Dispose();
                        return true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                        transaction.Rollback();
                        connection.Close();
                        connection.Dispose();
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Connessione DB fallita\n");
                    Console.WriteLine();
                    connection.Close();
                    connection.Dispose();
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        //LISTA UTENTI
        public List<UtenteClass> UtentiList()
        {
            List<UtenteClass> utenti = new List<UtenteClass>();

            try
            {
                SqlConnection connection = new SqlConnection(conn);
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    string query = "SELECT * FROM UTENTE";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        UtenteClass utente = new UtenteClass();
                        utente.Idutente = reader.GetString(0);
                        utente.Nome = reader.GetString(1);
                        utente.Cognome = reader.GetString(2);
                        utente.Password = reader.GetString(3);
                        utente.IsAdmin = reader.GetInt32(4);
                        utenti.Add(utente);
                    }
                    reader.Close();
                    connection.Close();
                    connection.Dispose();
                    return utenti;
                }
                else
                {
                    Console.WriteLine("Connessione DB fallita\n");
                    connection.Close();
                    connection.Dispose();
                    return utenti;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return utenti;
            }
        }
        
        //LISTA FORNITORI
        public List<FornitoreClass> FornitoriList()
        {
            List<FornitoreClass> fornitori = new List<FornitoreClass>();

            try
            {
                SqlConnection connection = new SqlConnection(conn);
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    string query = "SELECT * FROM FORNITORE";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        FornitoreClass fornitore = new FornitoreClass();
                        fornitore.Idfornitore=reader.GetString(0);
                        fornitore.Nome = reader.GetString(1);
                        fornitore.Partitaiva = reader.GetString(2);
                        fornitore.Indirizzo = reader.GetString(3);
                        fornitore.Email=reader.GetString(4);
                        fornitori.Add(fornitore);
                    }
                    reader.Close();
                    connection.Close();
                    connection.Dispose();
                    return fornitori;
                }
                else
                {
                    Console.WriteLine("Connessione DB fallita\n");
                    connection.Close();
                    connection.Dispose();
                    return fornitori;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return fornitori;
            }
        }

        //INSERIMENTO FORNITORE
        public bool InsertFornitore(string Nome, string PartitaIVA, string Email, string Indirizzo)
        {
            int tmp = 0;
            try
            {
                if (Nome != null && PartitaIVA != null && Indirizzo != null && Email != null)
                {
                    if (Nome.Trim() != "" && PartitaIVA.Trim() != "" && Indirizzo.Trim() != "" && Email.Trim() != "")
                    {
                        SqlConnection connection = new SqlConnection(conn);
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            //Cerco sul DB l'ultimo indice
                            string maxstring = "SELECT MAX(IDFORNITORE) AS ID FROM FORNITORE";
                            SqlCommand query = new SqlCommand(maxstring, connection);
                            SqlDataReader reader = query.ExecuteReader();
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    tmp = Convert.ToInt32(reader["ID"]) + 1;
                                }
                                reader.Close();
                            }
                            else
                            {
                                reader.Close();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Connessione DB fallita\n");
                            connection.Close();
                            connection.Dispose();
                            return false;
                        }
                        if (connection.State == ConnectionState.Open)
                        {
                            SqlTransaction transaction = connection.BeginTransaction();
                            SqlCommand query = connection.CreateCommand();
                            query.Connection = connection;
                            query.Transaction = transaction;
                            try
                            {
                                query.CommandText = "INSERT INTO FORNITORE (IDFORNITORE, NOME, PARTITAIVA, INDIRIZZO, EMAIL) VALUES ('" + tmp.ToString() + "', '" + Nome.ToUpper() + "', '" + PartitaIVA.ToUpper() + "', '" + Indirizzo.ToUpper() + "', '" + Email.ToUpper() + "')";
                                query.ExecuteNonQuery();
                                transaction.Commit();
                                connection.Close();
                                connection.Dispose();
                                return true;
                            }
                            catch (Exception ex) //eventuali errori della transazione
                            {
                                Console.WriteLine(ex.ToString());
                                transaction.Rollback();
                                connection.Close();
                                connection.Dispose();
                                return false;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Connessione DB fallita\n");
                            connection.Close();
                            connection.Dispose();
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        //LISTA CLIENTI
        public List<ClienteClass> ClientiList()
        {
            List<ClienteClass> clienti = new List<ClienteClass>();

            try
            {
                SqlConnection connection = new SqlConnection(conn);
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    string query = "SELECT * FROM CLIENTE";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        ClienteClass cliente = new ClienteClass();
                        cliente.Idcliente = reader.GetString(0);
                        cliente.Nome = reader.GetString(1);
                        cliente.Partitaiva = reader.GetString(2);
                        cliente.Codicefiscale = reader.GetString(3);
                        cliente.Indirizzo = reader.GetString(4);
                        cliente.Email = reader.GetString(5); 
                        clienti.Add(cliente);
                    }
                    reader.Close();
                    connection.Close();
                    connection.Dispose();
                    return clienti;
                }
                else
                {
                    Console.WriteLine("Connessione DB fallita\n");
                    connection.Close();
                    connection.Dispose();
                    return clienti;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return clienti;
            }
        }

        //INSERIMENTO CLIENTE
        public bool InsertCliente (string Nome, string PartitaIVA, string Codicefiscale, string Indirizzo, string Email)
        {
            int tmp = 0;
            try
            {
                if (Nome != null && PartitaIVA != null && Codicefiscale !=null && Indirizzo != null && Email != null)
                {
                    if (Nome.Trim() != "" && PartitaIVA.Trim() != "" && Codicefiscale.Trim() != "" && Indirizzo.Trim() != "" && Email.Trim() != "")
                    {
                        SqlConnection connection = new SqlConnection(conn);
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            //Cerco sul DB l'ultimo indice
                            string maxstring = "SELECT MAX(IDCLIENTE) AS ID FROM CLIENTE";
                            SqlCommand query = new SqlCommand(maxstring, connection);
                            SqlDataReader reader = query.ExecuteReader();
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    tmp = Convert.ToInt32(reader["ID"]) + 1;
                                }
                                reader.Close();
                            }
                            else
                            {
                                reader.Close();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Connessione DB fallita\n");
                            connection.Close();
                            connection.Dispose();
                            return false;
                        }
                            if (connection.State == ConnectionState.Open)
                            {
                                SqlTransaction transaction = connection.BeginTransaction();
                                SqlCommand query = connection.CreateCommand();
                                query.Connection = connection;
                                query.Transaction = transaction;
                                try
                                {
                                    query.CommandText = "INSERT INTO CLIENTE (IDCLIENTE, NOME, PARTITAIVA, CODICEFISCALE, INDIRIZZO, EMAIL) VALUES ('" + tmp.ToString() + "', '" + Nome.ToUpper() + "', '" + PartitaIVA.ToUpper() + "', '" + Codicefiscale.ToUpper() + "', '" + Indirizzo.ToUpper() + "', '" + Email.ToUpper() + "')";
                                    query.ExecuteNonQuery();
                                    transaction.Commit();
                                    connection.Close();
                                    connection.Dispose();
                                    return true;
                                }
                                catch (Exception ex) //eventuali errori della transazione
                                {
                                    Console.WriteLine(ex.ToString());
                                    transaction.Rollback();
                                    connection.Close();
                                    connection.Dispose();
                                    return false;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Connessione DB fallita\n");
                                connection.Close();
                                connection.Dispose();
                                return false;
                            }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        //INVENTARIO
        public List<ProdottoClass> ProdottiList()
        {

            List<ProdottoClass> prodotti = new List<ProdottoClass>();
            try
            {
                SqlConnection connection = new SqlConnection(conn);
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    string query = "SELECT * FROM PRODOTTO";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        ProdottoClass prodotto = new ProdottoClass();
                        prodotto.Idprodotto = reader["IDPRODOTTO"].ToString();
                        prodotto.Nome = reader["NOME"].ToString();
                        prodotto.Descrizione = reader["DESCRIZIONE"].ToString();
                        prodotto.Prezzo = Convert.ToDouble(reader["PREZZO"]);
                        prodotto.Quantita=Convert.ToInt32(reader["QUANTITA"]);
                        prodotti.Add(prodotto);
                    }
                    reader.Close();
                    connection.Close();
                    connection.Dispose();
                    return prodotti;
                }
                else
                {
                    Console.WriteLine("Connessione DB fallita\n");
                    connection.Close();
                    connection.Dispose();
                    return prodotti;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return prodotti;
            }
        }

        //INSERIMENTO PRODOTTO
        public bool InsertProdotto(string IDprodotto, string Nome, string Descrizione, double Prezzo, int Quantita)
        {

            try
            {
                SqlConnection connection = new SqlConnection(conn);
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    SqlTransaction transaction = connection.BeginTransaction();
                    SqlCommand query = connection.CreateCommand();
                    query.Connection = connection;
                    query.Transaction = transaction;
                    try
                    {
                        query.CommandText = "INSERT INTO PRODOTTO (IDPRODOTTO, NOME, DESCRIZIONE, PREZZO, QUANTITA) VALUES ('" + IDprodotto.Trim().ToUpper() + "', '" + Nome.ToUpper() + "', '" + Descrizione.ToUpper() + "', '" + Prezzo + "', '" + Quantita + "')";
                        query.ExecuteNonQuery();
                        transaction.Commit();
                        connection.Close();
                        connection.Dispose();
                        return true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                        transaction.Rollback();
                        connection.Close();
                        connection.Dispose();
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Connessione DB fallita\n");
                    connection.Close();
                    connection.Dispose();
                    return false;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        //AGGIUNGI QUANTITA PRODOTTO
        public bool Addquantitaprodotto(string Idprodotto, int Quantita)
        {
            ProdottoClass prodotto = new ProdottoClass();
            try
            {
                SqlConnection connection = new SqlConnection(conn);
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    string query = "SELECT * FROM PRODOTTO WHERE IDPRODOTTO=" + Idprodotto;
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        prodotto.Idprodotto = reader["IDPRODOTTO"].ToString();
                        prodotto.Nome = reader["NOME"].ToString();
                        prodotto.Descrizione = reader["DESCRIZIONE"].ToString();
                        prodotto.Prezzo = Convert.ToDouble(reader["PREZZO"]);
                        prodotto.Quantita = Convert.ToInt32(reader["QUANTITA"]);
                    }
                    reader.Close();
                }
                else
                {
                    Console.WriteLine("Connessione DB fallita\n");
                    connection.Close();
                    connection.Dispose();
                    return false;
                }
                if (connection.State == ConnectionState.Open)
                {
                    int sum = Quantita + prodotto.Quantita;
                    SqlTransaction transaction = connection.BeginTransaction();
                    SqlCommand query = connection.CreateCommand();
                    query.Connection = connection;
                    query.Transaction = transaction;
                    try
                    {
                        query.CommandText = "UPDATE PRODOTTO SET QUANTITA ='" + sum + "' WHERE IDprodotto = '" + Idprodotto + "'";
                        query.ExecuteNonQuery();
                        transaction.Commit();
                        connection.Close();
                        connection.Dispose();
                        return true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                        transaction.Rollback();
                        connection.Close();
                        connection.Dispose();
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Connessione DB fallita\n");
                    connection.Close();
                    connection.Dispose();
                    return false;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        //SOTTRAE A QUANTITA PRODOTTO
        public bool Minusquantitaprodotto(string Idprodotto, int Quantita)
        {
            ProdottoClass prodotto = new ProdottoClass();
            try
            {
                SqlConnection connection = new SqlConnection(conn);
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    string query = "SELECT * FROM PRODOTTO WHERE IDPRODOTTO=" + Idprodotto;
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        prodotto.Idprodotto = reader["IDPRODOTTO"].ToString();
                        prodotto.Nome = reader["NOME"].ToString();
                        prodotto.Descrizione = reader["DESCRIZIONE"].ToString();
                        prodotto.Prezzo = Convert.ToDouble(reader["PREZZO"]);
                        prodotto.Quantita = Convert.ToInt32(reader["QUANTITA"]);
                    }
                    reader.Close();
                }
                else
                {
                    Console.WriteLine("Connessione DB fallita\n");
                    connection.Close();
                    connection.Dispose();
                    return false;
                }
                if (connection.State == ConnectionState.Open)
                {
                    int minus = prodotto.Quantita - Quantita;
                    Console.WriteLine(minus);
                    if (minus >= 0)
                    {
                        SqlTransaction transaction = connection.BeginTransaction();
                        SqlCommand query = connection.CreateCommand();
                        query.Connection = connection;
                        query.Transaction = transaction;
                        try
                        {
                            query.CommandText = "UPDATE PRODOTTO SET QUANTITA ='" + minus + "' WHERE IDPRODOTTO = '" + Idprodotto + "'";
                            query.ExecuteNonQuery();
                            transaction.Commit();
                            connection.Close();
                            connection.Dispose();
                            return true;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                            transaction.Rollback();
                            connection.Close();
                            connection.Dispose();
                            return false;
                        }
                    }
                    else
                    {
                        connection.Close();
                        connection.Dispose();
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Connessione DB fallita\n");
                    connection.Close();
                    connection.Dispose();
                    return false;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        //DISPONIBILITA' PRODOTTO
        public ProdottoClass Disprodotto(string Idprodotto)
        {
            ProdottoClass prodotto = new ProdottoClass();
            prodotto.Quantita = -1; //Nel caso di oggetto non trovato controllo la quantità negativa
            try
            {
                SqlConnection connection = new SqlConnection(conn);
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    string query = "SELECT * FROM PRODOTTO WHERE IDPRODOTTO=" + Idprodotto;
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        prodotto.Idprodotto = reader["IDPRODOTTO"].ToString();
                        prodotto.Nome = reader["NOME"].ToString();
                        prodotto.Descrizione = reader["DESCRIZIONE"].ToString();
                        prodotto.Prezzo = Convert.ToDouble(reader["PREZZO"]);
                        prodotto.Quantita = Convert.ToInt32(reader["QUANTITA"]);
                    }
                    reader.Close();
                    connection.Close();
                    connection.Dispose();
                    return prodotto;
                }
                else
                {
                    Console.WriteLine("Connessione DB fallita\n");
                    connection.Close();
                    connection.Dispose();
                    return prodotto;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return prodotto;
            }
        }

        //Modifica Nome prodotto al Idprodotto selezionato (attraverso i parametri)
        public bool AlterNomeProdotto(string IDprodotto, string Nome)
        {
            try
            {
                SqlConnection connection = new SqlConnection(conn);
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    SqlTransaction transaction = connection.BeginTransaction();
                    SqlCommand query = connection.CreateCommand();
                    query.Connection = connection;
                    query.Transaction = transaction;
                    try
                    {
                        query.CommandText = "UPDATE PRODOTTO SET NOME ='" + Nome + "' WHERE IDprodotto = '" + IDprodotto + "'";
                        query.ExecuteNonQuery();
                        transaction.Commit();
                        connection.Close();
                        connection.Dispose();
                        return true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                        transaction.Rollback();
                        connection.Close();
                        connection.Dispose();
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Connessione DB fallita\n");
                    connection.Close();
                    connection.Dispose();
                    return false;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        //MODIFICA PREZZO PRODOTTO al Idprodotto selezionato (attraverso i parametri)
        public bool AlterPrezzoProdotto(string IDprodotto, double Prezzo)
        {
            try
            {
                SqlConnection connection = new SqlConnection(conn);
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    SqlTransaction transaction = connection.BeginTransaction();
                    SqlCommand query = connection.CreateCommand();
                    query.Connection = connection;
                    query.Transaction = transaction;
                    try
                    {
                        query.CommandText = "UPDATE PRODOTTO SET PREZZO ='" + Prezzo.ToString().Replace(",",".") + "' WHERE IDprodotto = '" + IDprodotto + "'";
                        query.ExecuteNonQuery();
                        transaction.Commit();
                        connection.Close();
                        connection.Dispose();
                        return true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                        transaction.Rollback();
                        connection.Close();
                        connection.Dispose();
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Connessione DB fallita\n");
                    connection.Close();
                    connection.Dispose();
                    return false;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        //LISTA FATTURE ACQUISTO 
        public List<FatturaAcquistoClass> FattureAcquistoList() 
        {
            List<FatturaAcquistoClass> fattureacquisto = new List<FatturaAcquistoClass>();
            List<FatturaAcquistoClass> fattura = new List<FatturaAcquistoClass>(); //lista d'appoggio per prelevare i numeri di fattura nel db
            double spesafattura = 0; //variabile d'appoggio per calcolare la spesa tot. di ogni fattura
            int quantitatot = 0; //variabile d'appoggio per calcolare quantita totale
            try
            {
                SqlConnection connection = new SqlConnection(conn);
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    string query = "SELECT NFATTURA FROM FATTURAACQUISTO GROUP BY NFATTURA";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        FatturaAcquistoClass fattura1 = new FatturaAcquistoClass(); //Classe di  appoggio
                        fattura1.Nfattura = reader.GetString(0);
                        fattura.Add(fattura1);
                    }
                    reader.Close();
                    foreach (var ft in fattura)
                    { 
                        //Azzeramento var di appoggio
                        quantitatot = 0;
                        spesafattura = 0; 
                        FatturaAcquistoClass fatturaacquisto = new FatturaAcquistoClass();
                        string query1 = "SELECT * FROM FATTURAACQUISTO WHERE NFATTURA='" + ft.Nfattura + "'";
                        SqlCommand command1 = new SqlCommand(query1, connection);
                        SqlDataReader reader1 = command1.ExecuteReader();
                        while (reader1.Read())
                        {
                            fatturaacquisto.Nfattura = reader1["NFATTURA"].ToString();
                            spesafattura = spesafattura + (Convert.ToInt32(reader1["QUANTITA"]) * Convert.ToDouble(reader1["PREZZOACQUISTO"]));
                            quantitatot = quantitatot + Convert.ToInt32(reader1["QUANTITA"]);
                            fatturaacquisto.DataAcquisto = Convert.ToDateTime(reader1["DATAACQUISTO"]);
                            fatturaacquisto.CodFornitore = reader1["CODFORNITORE"].ToString();
                            fatturaacquisto.CodUtente = reader1["CODUTENTE"].ToString();
                            fatturaacquisto.CodProdotto = reader1["CODPRODOTTO"].ToString();
                        }
                        fatturaacquisto.PrezzoAcquisto = spesafattura;
                        fatturaacquisto.Quantita = quantitatot;
                        reader1.Close();
                        fattureacquisto.Add(fatturaacquisto);
                    }
                    connection.Close();
                    connection.Dispose();
                    return fattureacquisto;
                }
                else
                {
                    Console.WriteLine("Connessione DB fallita\n");
                    connection.Close();
                    connection.Dispose();
                    return fattureacquisto;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return fattureacquisto;
            }
        }

        //LISTA FATTURE VENDITA
        public List<FatturaVenditaClass> FattureVenditaList()
        {
            List<FatturaVenditaClass> fatturevendita = new List<FatturaVenditaClass>();
            List<FatturaVenditaClass> fattura = new List<FatturaVenditaClass>(); //lista d'appoggio per prelevare i numeri di fattura nel db
            double spesafattura = 0; //variabile d'appoggio per calcolare la spesa tot. di ogni fattura
            int quantitatot = 0; //variabile d'appoggio per calcolare quantita totale
            try
            {
                SqlConnection connection = new SqlConnection(conn);
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    string query = "SELECT NFATTURA FROM FATTURAVENDITA GROUP BY NFATTURA";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        FatturaVenditaClass fattura1 = new FatturaVenditaClass(); //Classe di appoggio
                        fattura1.Nfattura = reader.GetString(0);
                        fattura.Add(fattura1);
                    }
                    reader.Close();
                    foreach (var ft in fattura)
                    {
                        //Azzeramento var di appoggio
                        quantitatot = 0;
                        spesafattura = 0;
                        FatturaVenditaClass fatturavendita = new FatturaVenditaClass();
                        string query1 = "SELECT * " +
                                        "FROM FATTURAVENDITA INNER JOIN PRODOTTO " +
                                        "ON FATTURAVENDITA.CODPRODOTTO=PRODOTTO.IDPRODOTTO " +
                                        "WHERE NFATTURA='" + ft.Nfattura + "'";
                        SqlCommand command1 = new SqlCommand(query1, connection);
                        SqlDataReader reader1 = command1.ExecuteReader();
                        while (reader1.Read())
                        {
                            fatturavendita.Nfattura = reader1["NFATTURA"].ToString();
                            spesafattura = spesafattura + (Convert.ToInt32(reader1["QUANTITA"]) * Convert.ToDouble(reader1["PREZZO"]));
                            quantitatot = quantitatot + Convert.ToInt32(reader1["QUANTITA"]);
                            fatturavendita.DataVendita = Convert.ToDateTime(reader1["DATAVENDITA"]);
                            fatturavendita.Pagamento = Convert.ToBoolean(reader1["PAGAMENTO"]);
                            fatturavendita.CodCliente = reader1["CODCLIENTE"].ToString();
                            fatturavendita.CodUtente = reader1["CODUTENTE"].ToString();
                            fatturavendita.CodProdotto = reader1["CODPRODOTTO"].ToString();
                        }
                        fatturavendita.PrezzoVendita = spesafattura;
                        fatturavendita.Quantita = quantitatot;
                        reader1.Close();
                        fatturevendita.Add(fatturavendita);
                    }
                    connection.Close();
                    connection.Dispose();
                    return fatturevendita;
                }
                else
                {
                    Console.WriteLine("Connessione DB fallita\n");
                    connection.Close();
                    connection.Dispose();
                    return fatturevendita;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return fatturevendita;
            }
        }

        //INSERIMENTO FATTURE ACQUISTO
        public bool InsertFatturaAcquisto(string Nfattura, string CodUtente, string CodFornitore, string CodProdotto, int Quantita, double PrezzoAcquisto)
        {
            try
            {
                SqlConnection connection = new SqlConnection(conn);
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    SqlTransaction transaction = connection.BeginTransaction();
                    SqlCommand query = connection.CreateCommand();
                    query.Connection = connection;
                    query.Transaction = transaction;
                    try
                    {
                        query.CommandText = "INSERT INTO FATTURAACQUISTO (NFATTURA, DATAACQUISTO, QUANTITA, PREZZOACQUISTO, CODFORNITORE, CODPRODOTTO, CODUTENTE)" +
                                            " VALUES ('" + Nfattura.ToUpper() + "', CURRENT_TIMESTAMP, '" + Quantita + "', '" + PrezzoAcquisto + "', '" + CodFornitore.ToUpper() + "', '" + CodProdotto.ToUpper() + "', '" + CodUtente.ToUpper() + "')";
                        query.ExecuteNonQuery();
                        transaction.Commit();
                        connection.Close();
                        connection.Dispose();
                        return true;
                    }
                    catch (Exception ex) //eventuali errori della transazione
                    {
                        Console.WriteLine(ex.ToString());
                        transaction.Rollback();
                        connection.Close();
                        connection.Dispose();
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Connessione DB fallita\n");
                    connection.Close();
                    connection.Dispose();
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        //INSERIMENTO FATTURE VENDITA
        public bool InsertFatturaVendita(string Nfattura, string CodUtente, string CodCliente, string CodProdotto, int Quantita, bool Pagamento)
        {
            try
            {
                SqlConnection connection = new SqlConnection(conn);
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    SqlTransaction transaction = connection.BeginTransaction();
                    SqlCommand query = connection.CreateCommand();
                    query.Connection = connection;
                    query.Transaction = transaction;
                    try
                    {
                        query.CommandText = "INSERT INTO FATTURAVENDITA (NFATTURA, DATAVENDITA, QUANTITA, PAGAMENTO, CODCLIENTE, CODPRODOTTO, CODUTENTE)" +
                                            " VALUES ('" + Nfattura.ToUpper() + "', CURRENT_TIMESTAMP, '" + Quantita + "', '" + Convert.ToInt32(Pagamento) + "', '" + CodCliente.ToUpper() + "', '" + CodProdotto.ToUpper() + "', '" + CodUtente.ToUpper() + "')";
                        query.ExecuteNonQuery();
                        transaction.Commit();
                        connection.Close();
                        connection.Dispose();
                        return true;
                    }
                    catch (Exception ex) //eventuali errori della transazione
                    {
                        Console.WriteLine(ex.ToString());
                        transaction.Rollback();
                        connection.Close();
                        connection.Dispose();
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Connessione DB fallita\n");
                    connection.Close();
                    connection.Dispose();
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        //Restituisce l'ultimo Nfattura di Vendita (Metodo per l'MVC)
        public string NFatturaVendita()
        {
            int tmp=1;
            try
            {
                SqlConnection connection = new SqlConnection(conn);
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    //Cerco sul DB l'ultimo indice
                    string maxstring = "SELECT MAX(NFATTURA) AS ID FROM FATTURAVENDITA";
                    SqlCommand query = new SqlCommand(maxstring, connection);
                    SqlDataReader reader = query.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            tmp =Convert.ToInt32(reader["ID"].ToString())+1;
                        }
                        reader.Close();
                        connection.Close();
                        connection.Dispose();
                        return tmp.ToString();
                    }
                    else
                    {
                        reader.Close();
                        connection.Close();
                        connection.Dispose();
                        return "1";
                    }
                }
                else
                {
                    Console.WriteLine("Connessione DB fallita\n");
                    connection.Close();
                    connection.Dispose();
                    return "-1";
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        //Modifica una fattura da "Da Pagare" -> "Pagata"
        public bool AlterPagatoFattura(string Nfattura)
        {
            try
            {
                SqlConnection connection = new SqlConnection(conn);
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    SqlTransaction transaction = connection.BeginTransaction();
                    SqlCommand query = connection.CreateCommand();
                    query.Connection = connection;
                    query.Transaction = transaction;
                    try
                    {
                        query.CommandText = "UPDATE FATTURAVENDITA SET PAGAMENTO ='" + 1 + "' WHERE NFATTURA = '" + Nfattura + "'";
                        query.ExecuteNonQuery();
                        transaction.Commit();
                        connection.Close();
                        connection.Dispose();
                        return true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                        transaction.Rollback();
                        connection.Close();
                        connection.Dispose();
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Connessione DB fallita\n");
                    connection.Close();
                    connection.Dispose();
                    return false;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
    }   
}
