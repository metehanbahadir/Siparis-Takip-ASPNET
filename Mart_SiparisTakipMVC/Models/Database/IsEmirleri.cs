using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mart_SiparisTakipMVC.Models.Database
{
    public class IsEmirleri
    {
        /*
is_SorumlulukMerkezi  70-> SMD 80-> THT 90-> Mekanik Montaj
,msg_S_0349   İş Emri No 
,msg_S_0350   Evrak Nolu Sipariş ile Üretilen
,msg_S_0351   Açılış Tarihi
,msg_S_0352   Stok Kodu 
,msg_S_0353   Stok Adı 
,msg_S_0354   Üretim Miktarı 
,#msg_S_0080  Üretilen Miktar 
,#msg_S_0247  Kalan Miktar 
,msg_S_4107  İşEmri Termin Tarihi 
,msg_S_0359  Aktifleşme Tarihi 
,msg_S_0361  Planlanan İş Başlangıç Tarihi 
,msg_S_0366  Planlanan İş Bitiş Tarihi 
,msg_S_0256  Siparişi Onaylayan Kullanıcı 
,msg_S_0369  Sipariş Seri
,msg_S_0370  Sipariş Sıra 
,msg_S_0200  Cari Kodu 
,msg_S_0390  Cari Adı 
,msg_S_0355  Açık/Kapanmış İş Emri
,msg_S_0360 Kapanma Tarihi
*/

        [Key]
        public int ID { get; set; }
        public string msg_S_0349 { get; set; }
        public string msg_S_0350 { get; set; }
        public DateTime msg_S_0351 { get; set; }
        public string msg_S_0352 { get; set; }
        public string msg_S_0353 { get; set; }
        public string msg_S_0354 { get; set; }
        public string msg_S_0080 { get; set; }
        public string msg_S_0247 { get; set; }
        public DateTime msg_S_4107 { get; set; }
        public DateTime? msg_S_0359 { get; set; }
        public DateTime msg_S_0361 { get; set; }
        public DateTime msg_S_0366 { get; set; }
        public string msg_S_0256 { get; set; }
        public string msg_S_0369 { get; set; }
        public string msg_S_0370 { get; set; }
        public string msg_S_0200 { get; set; }
        public string msg_S_0390 { get; set; }
        public string msg_S_0355 { get; set; }
        public DateTime? msg_S_0360 { get; set; }
        public string is_SorumlulukMerkezi { get; set; }        
        public string Not { get; set; }
        public string Kullanici { get; set; }
    }
}