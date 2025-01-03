using Mart_SiparisTakipMVC.Models.Database;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mart_SiparisTakipMVC.Controllers
{
    public class KullaniciController : Controller
    {
        // GET: Kullanici

        Context db = new Context();

        //Her sayfada döndürülmesi gereken data count larını ve SQL den UID ler eşleştirilerek alınan verileri içeren fonksiyon
        public void Dondur()
        {
            //Güncel Siparişlerle ilgili verilerin döndürüldüğü kısım
            List<GuncelSiparisler> guncels = new List<GuncelSiparisler>();
            string sqlconnstr = ConfigurationManager.ConnectionStrings["Context"].ConnectionString;
            SqlConnection sqlcon = new SqlConnection(sqlconnstr);
            string gunceldb = "SELECT GuncelSiparislers.UID, GuncelSiparislers.sip_stok_kod, GuncelSiparislers.sto_isim, GuncelSiparislers.sip_teslim_tarih, GuncelSiparislers.sip_evrakno_seri," +
                              "GuncelSiparislers.sip_evrakno_sira, GuncelSiparislers.sip_belge_no, GuncelSiparislers.sip_musteri_kod, GuncelSiparislers.cari_unvan, GuncelSiparislers.sip_miktar, GuncelSiparislers.sip_teslim_miktar, GuncelSiparislers.Teslimden_Kalan, GuncelSiparislers.sip_cins, " +
                              "GuncelSiparislers_NotDurum.Durum, GuncelSiparislers_NotDurum.[Not] FROM GuncelSiparislers INNER JOIN GuncelSiparislers_NotDurum ON GuncelSiparislers.UID = GuncelSiparislers_NotDurum.UID";
            SqlCommand sqlcmd = new SqlCommand(gunceldb, sqlcon);
            SqlDataAdapter sda = new SqlDataAdapter(sqlcmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                GuncelSiparisler gs = new GuncelSiparisler();
                gs.UID = dr["UID"].ToString();
                gs.cari_unvan = dr["cari_unvan"].ToString();
                gs.sip_teslim_tarih = Convert.ToDateTime(dr["sip_teslim_tarih"]);
                gs.sip_stok_kod = dr["sip_stok_kod"].ToString();
                gs.sto_isim = dr["sto_isim"].ToString();
                gs.sip_evrakno_seri = dr["sip_evrakno_seri"].ToString();
                gs.sip_evrakno_sira = Convert.ToInt32(dr["sip_evrakno_sira"]);
                gs.sip_belge_no = dr["sip_belge_no"].ToString();
                gs.sip_musteri_kod = dr["sip_musteri_kod"].ToString();
                gs.sip_miktar = Convert.ToInt32(dr["sip_miktar"]);
                gs.sip_teslim_miktar = Convert.ToInt32(dr["sip_teslim_miktar"]);
                gs.Teslimden_Kalan = Convert.ToInt32(dr["Teslimden_Kalan"]);
                gs.sip_cins = dr["sip_cins"].ToString();
                gs.Not = dr["Not"].ToString();
                gs.Durum = dr["Durum"].ToString();
                guncels.Add(gs);
            }

            var personel = Session["Personel"] as string;
            ViewBag.Personel = personel;
            var sipariscount = guncels.Where(x => x.Durum != "Askıya Alındı").Count();
            ViewBag.Count = sipariscount;
            var askiyaalinancount = guncels.Where(x => x.Durum == "Askıya Alındı").Count();
            ViewBag.AskiyaAlinanCount = askiyaalinancount;
            var tumsiparisler = db.GuncelSiparislers.Count();
            ViewBag.TumSiparislerCount = tumsiparisler;

            //İş Emirleriyle ilgili verilerin döndürüldüğü kısım
            List<IsEmirleri> isemirleri = new List<IsEmirleri>();
            string connstr = ConfigurationManager.ConnectionStrings["Context"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(connstr);
            string isemriserver = "SELECT IsEmirleris.msg_S_0349, IsEmirleris.msg_S_0352, IsEmirleris.msg_S_0353, IsEmirleris.msg_S_0354, IsEmirleris.msg_S_0355, IsEmirleris.msg_S_0360, IsEmirleris.msg_S_0361, IsEmirleris.msg_S_0366," +
                " is_SorumlulukMerkezi, IsEmirleri_NotDurum.[Not] FROM IsEmirleris INNER JOIN IsEmirleri_NotDurum ON IsEmirleris.msg_S_0349 = IsEmirleri_NotDurum.msg_S_0349";
            SqlCommand sqlcmdd = new SqlCommand(isemriserver, sqlconn);
            SqlDataAdapter sdaa = new SqlDataAdapter(sqlcmdd);
            DataTable dtt = new DataTable();
            sdaa.Fill(dtt);
            foreach (DataRow dr in dtt.Rows)
            {
                IsEmirleri isemri = new IsEmirleri();
                isemri.msg_S_0349 = dr["msg_S_0349"].ToString();
                isemri.msg_S_0352 = dr["msg_S_0352"].ToString();
                isemri.msg_S_0353 = dr["msg_S_0353"].ToString();
                isemri.msg_S_0354 = dr["msg_S_0354"].ToString();
                isemri.msg_S_0355 = dr["msg_S_0355"].ToString();
                isemri.msg_S_0360 = Convert.ToDateTime(dr["msg_S_0360"] != DBNull.Value ? (dr["msg_S_0360"]) : null);
                isemri.msg_S_0361 = Convert.ToDateTime(dr["msg_S_0361"]);
                isemri.msg_S_0366 = Convert.ToDateTime(dr["msg_S_0366"]);
                isemri.is_SorumlulukMerkezi = dr["is_SorumlulukMerkezi"].ToString();
                isemri.Not = dr["Not"].ToString();
                isemirleri.Add(isemri);
            }
            var personelid = Session["KullaniciAdi"] as string;

            var smdisemirlericount = isemirleri.Where(x => x.is_SorumlulukMerkezi == "70" && x.msg_S_0355 == "Aktif").Count();
            ViewBag.SmdIsEmirleriCount = smdisemirlericount;
            var thtisemirlericount = isemirleri.Where(x => (x.is_SorumlulukMerkezi == "80" || x.is_SorumlulukMerkezi == "90") && (x.msg_S_0355 == "Aktif")).Count();
            ViewBag.ThtIsEmirleriCount = thtisemirlericount;
            var kapaliisemirleri = isemirleri.Where(x => x.msg_S_0355 == "Kapanmış").Count();
            ViewBag.KapaliIsEmirleriCount = kapaliisemirleri;
            var planliisemirleri = isemirleri.Where(x => x.msg_S_0355 == "Planlanmış").Count();
            ViewBag.PlanliIsEmirleriCount = planliisemirleri;
            var tumisemirleri = db.IsEmirleri.Count();
            ViewBag.IsEmirleriCount = tumisemirleri;
            ViewBag.TumGorevlerCount = db.Gorevler.Where(x => x.Durum == "Aktif").Count();
            ViewBag.KendiGorevlerimCount = db.Gorevler.Where(x => (x.Atanan1ID == personelid || x.Atanan2ID == personelid || x.Atanan3ID == personelid || x.AtayanID == personelid) && (x.Durum == "Aktif")).Count();
            ViewBag.TedarikSiparisCount = db.GuncelTedarikSiparislers.Count();
        }

        [Authorize(Roles = "1,2")]
        public ActionResult Index()
        {
            var logged_staff = Session["KullaniciAdi"] as string;
            var orkunokur = db.Kullanici.First(x => x.Mail == "orkun@martelektronik.com");
            var cevdetdikiciler = db.Kullanici.First(x => x.Mail == "cevdet.dikiciler@martelektronik.com");
            if (Session["KullaniciAdi"] as string != null)
            {
                if (Session["KullaniciAdi"] as string  == orkunokur.KullaniciAdi || Session["KullaniciAdi"] as string  == cevdetdikiciler.KullaniciAdi)
                {
                    Dondur();
                    ViewBag.KullaniciCount = db.Kullanici.Count();
                    var kullanici = db.Kullanici.OrderBy(x => x.Personel).ToList();
                    return View(kullanici);
                }
                else
                {
                    Dondur();
                    ViewBag.KullaniciCount = db.Kullanici.Where(x=>x.Gorev =="Müşteri").Count()+1;
                    var users = db.Kullanici.ToList();
                    var kullanici = users.Where(x => x.KullaniciAdi == Session["KullaniciAdi"] as string || x.Gorev == "Müşteri").OrderBy(x => x.Personel).ToList();
                    return View(kullanici);
                }
            }
            else
            {
                return RedirectToAction("KullaniciGirisi", "Giris");
            }
        }

        [HttpGet]
        public ActionResult YeniKullanici()
        {
            return View();
        }

        [HttpPost]
        public ActionResult YeniKullanici(Kullanici k, FormCollection form)
        {
            var kullanici_check = db.Kullanici.Any(x => x.KullaniciAdi == k.KullaniciAdi);
            var kullaniciyetki = form["KullaniciYetki"].ToString();
            if (kullaniciyetki != "Seçiniz")
            {
                if (kullanici_check != true)
                {
                    db.Kullanici.Add(k);
                    k.Yetki = kullaniciyetki;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.KullaniciCheckErrMsg = "Bir kullanıcı adı birden fazla kişiye verilemez";
                    return View();
                }
            }
            else
            {
                ViewBag.Msg = "Yetki Boş Bırakılamaz";
                return View();
            }
        }

        public ActionResult KullaniciGetir(int id)
        {
            Dondur();
            var kullanici = db.Kullanici.Find(id);
            ViewBag.Yetki = kullanici.Yetki;
            return View("KullaniciGetir", kullanici);
        }

        public ActionResult KullaniciDuzenle(Kullanici k, FormCollection form)
        {
            var kullaniciyetki = form["KullaniciYetki"].ToString();
            var kullanici = db.Kullanici.Find(k.KayitID);
            if (kullaniciyetki != "Seçiniz")
            {
                kullanici.Personel = k.Personel;
                kullanici.Gorev = k.Gorev;
                kullanici.Yetki = kullaniciyetki;
                kullanici.Sifre = k.Sifre;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Msg = "Yetki Boş Bırakılamaz";
                return View("KullaniciGetir", kullanici);
            }
        }

        [Authorize(Roles = "1")]
        public ActionResult KullaniciSil(int id)
        {
            var kullanici = db.Kullanici.Find(id);
            db.Kullanici.Remove(kullanici);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}