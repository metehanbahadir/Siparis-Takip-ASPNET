using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mart_SiparisTakipMVC.Models.Database
{
    public class GuncelTedarikSiparisler
    {
        [Key]
        public int KayitID { get; set; }

        [Column(TypeName = "Nvarchar")]
        [StringLength(50)]
        public string Cinsi { get; set; }

        [Column(TypeName = "Nvarchar")]
        [StringLength(50)]
        public string Stok_kodu { get; set; }
        public string Stok_isim { get; set; }

        [Required]
        public DateTime Siparis_tarihi { get; set; }

        [Required]
        public DateTime Teslim_tarihi { get; set; }

        [Column(TypeName = "Nvarchar")]
        [StringLength(50)]
        [Required]
        public string Siparis_cinsi { get; set; }

        [Column(TypeName = "Nvarchar")]
        [StringLength(50)]
        public string Evrak_seri_no { get; set; }

        [Column(TypeName = "Nvarchar")]
        [StringLength(50)]
        public string Evrak_sira_no { get; set; }

        [Required]
        public string Cari_ismi { get; set; }

        [Required]
        public float Siparis_miktar { get; set; }

        [Required]
        public float Tamamlanan_miktar { get; set; }

        [Required]
        public float Kalan_miktar { get; set; }

        [Column(TypeName = "Nvarchar")]
        [StringLength(50)]
        public string Birim { get; set; }


    }
}