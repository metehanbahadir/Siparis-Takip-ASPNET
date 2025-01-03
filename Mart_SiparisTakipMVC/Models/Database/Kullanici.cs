using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mart_SiparisTakipMVC.Models.Database
{
    public class Kullanici
    {
        [Key]
        public int KayitID { get; set; }

        [Column(TypeName ="Nvarchar")]
        [StringLength(50)]
        [Display(Name ="Personel")]
        public string Personel {  get; set; }

        [Column(TypeName = "Nvarchar")]
        [StringLength(30)]
        [Display(Name = "Görev")]
        public string Gorev {  get; set; }

        [Column(TypeName = "Nvarchar")]
        [StringLength(1)]
        [Display(Name = "Yetki")]
        public string Yetki { get; set; }

        [Column(TypeName = "Nvarchar")]
        [StringLength(50)]
        [Display(Name = "Kullanıcı Adı")]
        public string KullaniciAdi { get; set; }

        [Column(TypeName = "Nvarchar")]
        [StringLength(50)]
        [Display(Name = "Şifre")]
        public string Sifre {  get; set; }

        public string Mail { get; set; }

    }
}