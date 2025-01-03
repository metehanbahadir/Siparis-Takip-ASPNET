using Mart_SiparisTakipMVC.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Mart_SiparisTakipMVC.Controllers
{
    public class GirisController : Controller
    {
        // GET: Giris

        Context db = new Context();
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult KullaniciGirisi()
        {
            return View();
        }

        [HttpPost]
        public ActionResult KullaniciGirisi(Kullanici k)
        {
            var user = db.Kullanici.FirstOrDefault(x => x.KullaniciAdi == k.KullaniciAdi && x.Sifre == k.Sifre);

            if (user != null)
            {
                if (user.KullaniciAdi.StartsWith("M.") && user.Gorev == "Müşteri")
                {
                    FormsAuthentication.SetAuthCookie(user.KullaniciAdi, false);
                    Session["MusteriUnvan"] = user.Personel;
                    Session["MusteriKullaniciAdi"] = user.KullaniciAdi;
                    return RedirectToAction("MusteriSiparisIndex", "Siparis");
                }
                else if (user.Gorev =="SMD Sorumlusu")
                {
                    FormsAuthentication.SetAuthCookie(user.KullaniciAdi, false);
                    Session["Personel"] = user.Personel;
                    Session["Gorev"] = user.Gorev;
                    Session["KullaniciAdi"] = user.KullaniciAdi;
                    Session["KayitID"] = user.KayitID;

                    return RedirectToAction("SMDIsEmirleri", "IsEmri");
                }
                else if (user.Gorev == "THT Sorumlusu")
                {
                    FormsAuthentication.SetAuthCookie(user.KullaniciAdi, false);
                    Session["Personel"] = user.Personel;
                    Session["Gorev"] = user.Gorev;
                    Session["KullaniciAdi"] = user.KullaniciAdi;
                    Session["KayitID"] = user.KayitID;

                    return RedirectToAction("THTIsEmirleri", "IsEmri");
                }
                else if(user.KullaniciAdi =="t" && user.Sifre == "t")
                {
                    FormsAuthentication.SetAuthCookie(user.KullaniciAdi, false);
                    Session["Personel"] = user.Personel;
                    Session["Gorev"] = user.Gorev;
                    Session["KullaniciAdi"] = user.KullaniciAdi;
                    Session["KayitID"] = user.KayitID;

                    return RedirectToAction("THTIsEmriViewer", "IsEmri");
                }
                else if (user.KullaniciAdi == "s" && user.Sifre == "s")
                {
                    FormsAuthentication.SetAuthCookie(user.KullaniciAdi, false);
                    Session["Personel"] = user.Personel;
                    Session["Gorev"] = user.Gorev;
                    Session["KullaniciAdi"] = user.KullaniciAdi;
                    Session["KayitID"] = user.KayitID;

                    return RedirectToAction("SMDIsEmriViewer", "IsEmri");
                }
                else if(user.Yetki == "9")
                {
                    FormsAuthentication.SetAuthCookie(user.KullaniciAdi, false);
                    Session["Personel"] = user.Personel;
                    Session["Gorev"] = user.Gorev;
                    Session["KullaniciAdi"] = user.KullaniciAdi;
                    Session["KayitID"] = user.KayitID;

                    return RedirectToAction("Gorevlerim", "Gorev");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(user.KullaniciAdi, false);
                    Session["Personel"] = user.Personel;
                    Session["Gorev"] = user.Gorev;
                    Session["KullaniciAdi"] = user.KullaniciAdi;
                    Session["KayitID"] = user.KayitID;

                    return RedirectToAction("Index", "Siparis");
                }
            }
            else
            {
                ViewBag.BasarisizGiris = "Hatalı Giriş Bilgileri !";
                return View();
            }
        }

        public ActionResult CikisYap()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("KullaniciGirisi", "Giris");
        }
    }
}