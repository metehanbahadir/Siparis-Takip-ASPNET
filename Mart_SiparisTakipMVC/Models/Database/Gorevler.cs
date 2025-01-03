using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mart_SiparisTakipMVC.Models.Database
{
    public class Gorevler
    {
        [Key]
        public int ID { get; set; }
        public string GorevDosyaID { get; set; }
        public string EklenmeTarih { get; set; }
        public string Gorev { get; set; }
        public string Baslama_Tarihi { get; set; }
        public string Bitis_Tarihi { get; set; }
        public string Tamamlanma_Tarihi { get; set; }
        public string AtayanID { get; set; }
        public string Atayan { get; set; }
        public string Atanan1ID { get; set; }
        public string Atanan1 { get; set; }
        public string Atanan2ID { get; set; }
        public string Atanan2 { get; set; }
        public string Atanan3ID { get; set; }
        public string Atanan3 { get; set; }
        public string Durum { get; set; }
        public string Not_Atayan {  get; set; }              
        public string Not1 {  get; set; }              
        public string Not2 {  get; set; }              
        public string Not3 {  get; set; }                  
    }
}