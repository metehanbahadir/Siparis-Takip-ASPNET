using System.Data.Entity;

namespace Mart_SiparisTakipMVC.Models.Database
{
    public class Context : DbContext
    {                
        public DbSet<Kullanici> Kullanici { get; set; }        
        public DbSet<GuncelSiparisler> GuncelSiparislers { get; set; }
        public DbSet<GuncelSiparislers_NotDurum> GuncelSiparislersNotDurum { get; set; }        
        public DbSet<GecmisSiparisHareket> GecmisSiparisHarekets { get; set; }
        public DbSet<IsEmirleri> IsEmirleri { get; set; }
        public DbSet<IsEmirleri_NotDurum> IsEmirleriNotDurum { get; set; }      
        public DbSet<Gorevler> Gorevler { get; set; }      
        public DbSet<Receteler> Recetelers { get; set; }
        public DbSet<Dosyalar> Dosyalars { get; set; }
        public DbSet<GorevDosyalari> GorevDosyalaris { get; set; }
        public DbSet<GuncelTedarikSiparisler> GuncelTedarikSiparislers { get; set; }
    }
}