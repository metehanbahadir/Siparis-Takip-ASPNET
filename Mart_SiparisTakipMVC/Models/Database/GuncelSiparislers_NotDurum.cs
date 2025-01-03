using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mart_SiparisTakipMVC.Models.Database
{
    public class GuncelSiparislers_NotDurum
    {
        [Key]
        public string UID { get; set; }

        [Column(TypeName = "Nvarchar")]
        [StringLength(200)]
        public string Not { get; set; }

        [Column(TypeName = "Nvarchar")]
        [StringLength(50)]
        public string Durum { get; set; }

        public string Tarih {  get; set; }

        public string Kullanici { get; set; }
    }
}