using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mart_SiparisTakipMVC.Models.Database
{
    public class IsEmirleri_NotDurum
    {
        [Key]
        public int ID { get; set; }
        public string msg_S_0349 { get; set; }
        public string Not { get; set; }        
        public string Kullanici { get; set; }
    }
}