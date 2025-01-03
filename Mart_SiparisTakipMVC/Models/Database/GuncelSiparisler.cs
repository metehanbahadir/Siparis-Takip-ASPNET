using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;

namespace Mart_SiparisTakipMVC.Models.Database
{
    public class GuncelSiparisler
    {
        [Key]        
        public string UID { get; set; }        

        [Column(TypeName = "Nvarchar")]
        [StringLength(25)]
        public string sip_stok_kod { get; set; }

        [Column(TypeName = "Nvarchar")]
        [StringLength(127)]
        public string sto_isim { get; set; }
        public DateTime sip_teslim_tarih { get; set; }

        [Column(TypeName = "Nvarchar")]
        [StringLength(20)]
        public string sip_evrakno_seri { get; set; }
        public int sip_evrakno_sira { get; set; }

        [Column(TypeName = "Nvarchar")]
        [StringLength(50)]
        public string sip_belge_no { get; set; }

        [Column(TypeName = "Nvarchar")]
        [StringLength(25)]
        public string sip_musteri_kod { get; set; }

        [Column(TypeName = "Nvarchar")]
        [StringLength(127)]
        public string cari_unvan { get; set; }
        public float sip_miktar { get; set; }
        public float sip_teslim_miktar { get; set; }

        [Column(TypeName = "Nvarchar")]
        [StringLength(200)]
        [Display(Name = "Not")]
        public string Not { get; set; }

        [Column(TypeName = "Nvarchar")]
        [StringLength(20)]
        [Display(Name = "Durum")]
        public string Durum { get; set; }
        public float Teslimden_Kalan { get; set; }        
        
        public string sip_cins {  get; set; }
    }
}