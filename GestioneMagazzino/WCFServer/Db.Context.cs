﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Codice generato da un modello.
//
//     Le modifiche manuali a questo file potrebbero causare un comportamento imprevisto dell'applicazione.
//     Se il codice viene rigenerato, le modifiche manuali al file verranno sovrascritte.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WCFServer
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MagazzinoConnection : DbContext
    {
        public MagazzinoConnection()
            : base("name=MagazzinoConnection")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CLIENTE> CLIENTE { get; set; }
        public virtual DbSet<FATTURAACQUISTO> FATTURAACQUISTO { get; set; }
        public virtual DbSet<FATTURAVENDITA> FATTURAVENDITA { get; set; }
        public virtual DbSet<FORNITORE> FORNITORE { get; set; }
        public virtual DbSet<PRODOTTO> PRODOTTO { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<UTENTE> UTENTE { get; set; }
    }
}
