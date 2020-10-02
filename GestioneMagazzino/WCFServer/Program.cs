using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;//Liberia per fornire un Host di Servizi
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace WCFServer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ServiceHost svcHost = new ServiceHost(typeof(ServerService));      //Inizializzo un Server Host per i servizi implementati in "ServerServices"
                svcHost.Open();                                                     //Apre Il Server

                Console.WriteLine("Server Attivo.\nPremere invio per interrompere...");
                Console.ReadLine();
                
                svcHost.Close();
                Console.WriteLine("Server Interrotto.");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Errore: " + ex.ToString());
                Console.ReadLine();
            }
        }
    }
}
