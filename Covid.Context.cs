//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace F_W2021_991577657
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CovidDBEntities : DbContext
    {
        public CovidDBEntities()
            : base("name=CovidDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Continent> Continents { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Covid> Covids { get; set; }
    }
}
