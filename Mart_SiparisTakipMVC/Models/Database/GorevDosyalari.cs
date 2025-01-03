using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mart_SiparisTakipMVC.Models.Database
{
    public class GorevDosyalari
    {
        [Key]
        public int ID { get; set; }
        public string GorevID { get; set; }
        public string DosyaYolu { get; set; }
        public string EkleyenPersonelID { get; set; }
        public string EkleyenPersonel {  get; set; }
    }
}