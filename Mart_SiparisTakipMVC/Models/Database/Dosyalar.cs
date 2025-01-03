using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mart_SiparisTakipMVC.Models.Database
{
    public class Dosyalar
    {
        [Key]
        public int ID { get; set; }
        public string sip_stok_kod { get; set; }
        public string Dosya {  get; set; }
        public string EklenmeTarih { get; set; }
        public string Ekleyen { get; set; }
    }
}