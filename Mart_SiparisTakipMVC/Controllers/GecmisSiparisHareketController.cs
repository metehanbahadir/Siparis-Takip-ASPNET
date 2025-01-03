using Mart_SiparisTakipMVC.Models.Database;
using PagedList;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Mart_SiparisTakipMVC.Controllers
{
    public class GecmisSiparisHareketController : Controller
    {
        // GET: GecmisSiparisHareket

        Context db = new Context();

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
        public ActionResult Index(GuncelSiparisler gs, int sayfa = 1)
        {
            if (Session["KullaniciAdi"] as string != null)
            {
                Dondur();
                var gecmishareketcount = db.GecmisSiparisHarekets.Count();
                ViewBag.GecmisCount = gecmishareketcount;
                var gecmishareket = db.GecmisSiparisHarekets.OrderByDescending(x => x.ID).ToPagedList(sayfa, 50);
                return View(gecmishareket);
            }
            else
            {
                return RedirectToAction("KullaniciGirisi", "Giris");
            }
        }

        static int gecmishareketorder;
        public ActionResult TersSiralama(GuncelSiparisler gs, int sayfa = 1)
        {
            if (gecmishareketorder == 0)
            {
                gecmishareketorder = 1;
                Dondur();
                var gecmishareket = db.GecmisSiparisHarekets.OrderByDescending(x => x.ID).ToPagedList(sayfa, 50);
                return View("Index", gecmishareket);
            }
            else if (gecmishareketorder == 1)
            {
                gecmishareketorder = 0;
                Dondur();
                var gecmishareket = db.GecmisSiparisHarekets.OrderBy(x => x.ID).ToPagedList(sayfa, 50);
                return View("Index", gecmishareket);
            }
            else
            {
                Dondur();
                var gecmishareket = db.GecmisSiparisHarekets.OrderBy(x => x.ID).ToPagedList(sayfa, 50);
                return View("Index", gecmishareket);
            }
        }

        public ActionResult CariTersAlfabetik(GuncelSiparisler gs, int sayfa = 1)
        {
            Dondur();
            var cariters = db.GecmisSiparisHarekets.OrderByDescending(x => x.cari_unvan).ToPagedList(sayfa, 50);
            return View("Index", cariters);
        }

        public ActionResult CariAlfabetik(GuncelSiparisler gs, int sayfa = 1)
        {
            Dondur();
            var cari = db.GecmisSiparisHarekets.OrderBy(x => x.cari_unvan).ToPagedList(sayfa, 50);
            return View("Index", cari);
        }

        public ActionResult UrunTersAlfabetik(GuncelSiparisler gs, int sayfa = 1)
        {
            Dondur();
            var urunters = db.GecmisSiparisHarekets.OrderByDescending(x => x.sto_isim).ToPagedList(sayfa, 50);
            return View("Index", urunters);
        }

        public ActionResult UrunAlfabetik(GuncelSiparisler gs, int sayfa = 1)
        {
            Dondur();
            var urun = db.GecmisSiparisHarekets.OrderBy(x => x.sto_isim).ToPagedList(sayfa, 50);
            return View("Index", urun);
        }

        public ActionResult SiparisFiltrele(GuncelSiparisler gs, string siparisfiltre, int sayfa = 1)
        {
            Dondur();
            ViewBag.GecmisCount = db.GecmisSiparisHarekets.Count();
            var sayims = from s in db.GecmisSiparisHarekets
                         select s;
            if (!string.IsNullOrEmpty(siparisfiltre))
            {
                sayims = sayims.Where(x => x.cari_unvan.Contains(siparisfiltre) || x.UID.Equals(siparisfiltre) || x.sip_evrakno_seri.Contains(siparisfiltre) || x.sip_belge_no.Contains(siparisfiltre) || x.sip_stok_kod.Contains(siparisfiltre) || x.sto_isim.Contains(siparisfiltre) ||
                                      x.Durum.Contains(siparisfiltre) || x.Not.Contains(siparisfiltre));
            }
            return View("Index", sayims.Where(x => x.cari_unvan.ToLower().Contains(siparisfiltre) || x.cari_unvan.ToUpper().Contains(siparisfiltre) ||
                                      x.Durum.ToLower().Contains(siparisfiltre) || x.Durum.ToUpper().Contains(siparisfiltre) ||
                                      x.UID.Equals(siparisfiltre) ||
                                      x.sip_evrakno_seri.Contains(siparisfiltre) ||
                                      x.sip_belge_no.Contains(siparisfiltre) || x.sip_stok_kod.Contains(siparisfiltre) ||
                                      x.sto_isim.ToLower().Contains(siparisfiltre) || x.sto_isim.ToUpper().Contains(siparisfiltre) ||
                                      x.Durum.ToLower().Contains(siparisfiltre) || x.Durum.ToUpper().Contains(siparisfiltre) ||
                                      x.Not.ToLower().Contains(siparisfiltre) || x.Not.ToUpper().Contains(siparisfiltre)).OrderBy(x => x.sip_teslim_tarih).ToPagedList(sayfa, 50));
        }
    }
}