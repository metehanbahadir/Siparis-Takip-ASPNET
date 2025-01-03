using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mart_SiparisTakipMVC.Models.Database
{
    public class Receteler
    {
        [Key]
        public int ID { get; set; }
        public DateTime rec_create_date { get; set; }
        public DateTime? rec_lastup_date { get; set; }

        [Column(TypeName ="Nvarchar")]
        [StringLength(25)]
        public string rec_anakod {  get; set; }

        [Column(TypeName = "Nvarchar")]
        [StringLength(127)]
        public string sto_isim_mamul {  get; set; }
        public DateTime? rec_tarih { get; set; }

        [Column(TypeName = "Nvarchar")]
        [StringLength(127)]
        public string rec_aciklama { get; set; }

        [Column(TypeName = "Nvarchar")]
        [StringLength(25)]
        public string rec_tuketim_kod { get; set; }

        [Column(TypeName = "Nvarchar")]
        [StringLength(127)]
        public string sto_isim_yarimamul { get; set; }
        public double rec_tuketim_miktar {  get; set; }

        [Column(TypeName = "Nvarchar")]
        [StringLength(10)]
        public string sto_birim {  get; set; }
        public int rec_uretim_tuketim { get; set; }
        public int rec_satirno { get; set; }

        [Column(TypeName = "Nvarchar")]
        [StringLength(127)]
        public string rec_satir_acik {  get; set; }
        public int rec_depono { get; set; }
        public double rec_fireyuzde {  get; set; }
        public int rec_PlanlamaTipi { get; set; }

    }
}