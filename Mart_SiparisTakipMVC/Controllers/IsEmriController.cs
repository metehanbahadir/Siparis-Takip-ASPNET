using Mart_SiparisTakipMVC.Models.Database;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace Mart_SiparisTakipMVC.Controllers
{
    public class IsEmriController : Controller
    {
        // GET: IsEmri

        Context db = new Context();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SMDIsEmriViewer(int sayfa = 1)
        {
            //İş Emirleri tablosundaki SMD ye ait iş emirlerinin döndürüldüğü kısım
            List<IsEmirleri> isemirleri = new List<IsEmirleri>();
            string connstr = ConfigurationManager.ConnectionStrings["Context"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(connstr);
            string isemriserver = "SELECT IsEmirleris.msg_S_0349, IsEmirleris.msg_S_0352, IsEmirleris.msg_S_0353, IsEmirleris.msg_S_0080, IsEmirleris.msg_S_0247, IsEmirleris.msg_S_0354, IsEmirleris.msg_S_0355, IsEmirleris.msg_S_0360, IsEmirleris.msg_S_0361, IsEmirleris.msg_S_0366," +
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
                isemri.msg_S_0080 = dr["msg_S_0080"].ToString();
                isemri.msg_S_0247 = dr["msg_S_0247"].ToString();
                isemri.msg_S_0355 = dr["msg_S_0355"].ToString();
                isemri.msg_S_0360 = Convert.ToDateTime(dr["msg_S_0360"] != DBNull.Value ? (dr["msg_S_0360"]) : null);
                isemri.msg_S_0361 = Convert.ToDateTime(dr["msg_S_0361"]);
                isemri.msg_S_0366 = Convert.ToDateTime(dr["msg_S_0366"]);
                isemri.is_SorumlulukMerkezi = dr["is_SorumlulukMerkezi"].ToString();
                isemri.Not = dr["Not"].ToString();
                isemirleri.Add(isemri);
            }
            var smdisemirlericount = isemirleri.Where(x => x.is_SorumlulukMerkezi == "70" && x.msg_S_0355 == "Aktif").Count();
            ViewBag.SmdIsEmirleriCount = smdisemirlericount;

            return View(isemirleri.Where(x => x.is_SorumlulukMerkezi == "70" && x.msg_S_0355 == "Aktif").OrderByDescending(x => x.msg_S_0361).ToPagedList(sayfa, 13));
        }

        public ActionResult THTIsEmriViewer(int sayfa = 1)
        {
            //İş Emirleri tablosundaki THT ve Mekanik Montaj a ait iş emirlerinin döndürüldüğü kısım
            List<IsEmirleri> isemirleri = new List<IsEmirleri>();
            string connstr = ConfigurationManager.ConnectionStrings["Context"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(connstr);
            string isemriserver = "SELECT IsEmirleris.msg_S_0349, IsEmirleris.msg_S_0352, IsEmirleris.msg_S_0080, IsEmirleris.msg_S_0247, IsEmirleris.msg_S_0353, IsEmirleris.msg_S_0354, IsEmirleris.msg_S_0355, IsEmirleris.msg_S_0360, IsEmirleris.msg_S_0361, IsEmirleris.msg_S_0366," +
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
                isemri.msg_S_0080 = dr["msg_S_0080"].ToString();
                isemri.msg_S_0247 = dr["msg_S_0247"].ToString();
                isemri.msg_S_0355 = dr["msg_S_0355"].ToString();
                isemri.msg_S_0360 = Convert.ToDateTime(dr["msg_S_0360"] != DBNull.Value ? (dr["msg_S_0360"]) : null);
                isemri.msg_S_0361 = Convert.ToDateTime(dr["msg_S_0361"]);
                isemri.msg_S_0366 = Convert.ToDateTime(dr["msg_S_0366"]);
                isemri.is_SorumlulukMerkezi = dr["is_SorumlulukMerkezi"].ToString();
                isemri.Not = dr["Not"].ToString();
                isemirleri.Add(isemri);
            }
            var thtisemirlericount = isemirleri.Where(x => (x.is_SorumlulukMerkezi == "80" || x.is_SorumlulukMerkezi == "90") && (x.msg_S_0355 == "Aktif")).Count();
            ViewBag.ThtIsEmirleriCount = thtisemirlericount;

            return View(isemirleri.Where(x => (x.is_SorumlulukMerkezi == "80" || x.is_SorumlulukMerkezi == "90") && x.msg_S_0355 == "Aktif").OrderByDescending(x => x.msg_S_0361).ToPagedList(sayfa, 13));
        }

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
        public ActionResult PlanliIsEmirleri(int sayfa = 1)
        {
            if (Session["KullaniciAdi"] as string != null)
            {
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
                Dondur();
                return View(isemirleri.Where(x => x.msg_S_0355 == "Planlanmış").OrderBy(x => x.msg_S_0361).ToPagedList(sayfa, 20));
            }
            else
            {
                return RedirectToAction("KullaniciGirisi", "Giris");
            }
        }

        public ActionResult PlanliIsEmriFiltre(string planliisemrifiltre, int sayfa = 1)
        {
            if (Session["KullaniciAdi"] as string != null)
            {
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
                Dondur();
                return View("PlanliIsEmirleri", isemirleri.Where(x => x.msg_S_0355 == "Planlanmış" && (x.msg_S_0349.Contains(planliisemrifiltre) || x.msg_S_0352.Contains(planliisemrifiltre) || x.msg_S_0353.Contains(planliisemrifiltre) || x.msg_S_0354.Contains(planliisemrifiltre)
                                                                   || x.msg_S_0355.Contains(planliisemrifiltre) || x.is_SorumlulukMerkezi.Contains(planliisemrifiltre) || x.Not.Contains(planliisemrifiltre))).OrderBy(x => x.msg_S_0361).ToPagedList(sayfa, 20));
            }
            else
            {
                return RedirectToAction("KullaniciGirisi", "Giris");
            }
        }

        public ActionResult PlanliUrunAlfabetik(int sayfa = 1)
        {
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
            Dondur();
            return View("PlanliIsEmirleri", isemirleri.Where(x => x.msg_S_0355 == "Planlanmış").OrderBy(x => x.msg_S_0353).ToPagedList(sayfa, 20));
        }

        public ActionResult PlanliUrunTersAlfabetik(int sayfa = 1)
        {
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
            Dondur();
            return View("PlanliIsEmirleri", isemirleri.Where(x => x.msg_S_0355 == "Planlanmış").OrderByDescending(x => x.msg_S_0353).ToPagedList(sayfa, 20));
        }


        [Authorize(Roles = "1,2,4")]
        public ActionResult SMDIsEmirleri(int sayfa = 1)
        {
            if (Session["KullaniciAdi"] as string != null)
            {
                var tbl_isemriserver = db.IsEmirleri.ToList();
                var tbl_isemrinotdurum = db.IsEmirleriNotDurum.ToList();

                var eksikisemri = tbl_isemriserver.Select(x => x.msg_S_0349).Except(tbl_isemrinotdurum.Select(x => x.msg_S_0349));
                var fazlaisemri = tbl_isemrinotdurum.Select(x => x.msg_S_0349).Except(tbl_isemriserver.Select(x => x.msg_S_0349));

                foreach (var isemrino in eksikisemri)
                {
                    db.IsEmirleriNotDurum.Add(new IsEmirleri_NotDurum { msg_S_0349 = isemrino });
                    db.SaveChanges();
                }

                foreach (var isemrino in fazlaisemri)
                {
                    var silinecek = db.IsEmirleriNotDurum.First(x => x.msg_S_0349 == isemrino);
                    db.IsEmirleriNotDurum.Remove(silinecek);
                    db.SaveChanges();
                }

                List<IsEmirleri> isemirleri = new List<IsEmirleri>();
                string connstr = ConfigurationManager.ConnectionStrings["Context"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(connstr);
                string isemriserver = "SELECT IsEmirleris.msg_S_0349, IsEmirleris.msg_S_0352, IsEmirleris.msg_S_0080, IsEmirleris.msg_S_0247, IsEmirleris.msg_S_0353, IsEmirleris.msg_S_0354, IsEmirleris.msg_S_0355, IsEmirleris.msg_S_0361, IsEmirleris.msg_S_0366," +
                    " is_SorumlulukMerkezi, IsEmirleri_NotDurum.[Not] FROM IsEmirleris INNER JOIN IsEmirleri_NotDurum ON IsEmirleris.msg_S_0349 = IsEmirleri_NotDurum.msg_S_0349";
                SqlCommand sqlcmd = new SqlCommand(isemriserver, sqlcon);
                SqlDataAdapter sda = new SqlDataAdapter(sqlcmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    IsEmirleri isemri = new IsEmirleri();
                    isemri.msg_S_0349 = dr["msg_S_0349"].ToString();
                    isemri.msg_S_0352 = dr["msg_S_0352"].ToString();
                    isemri.msg_S_0080 = dr["msg_S_0080"].ToString();
                    isemri.msg_S_0247 = dr["msg_S_0247"].ToString();
                    isemri.msg_S_0353 = dr["msg_S_0353"].ToString();
                    isemri.msg_S_0354 = dr["msg_S_0354"].ToString();
                    isemri.msg_S_0355 = dr["msg_S_0355"].ToString();
                    isemri.msg_S_0361 = Convert.ToDateTime(dr["msg_S_0361"]);
                    isemri.msg_S_0366 = Convert.ToDateTime(dr["msg_S_0366"]);
                    isemri.is_SorumlulukMerkezi = dr["is_SorumlulukMerkezi"].ToString();
                    isemri.Not = dr["Not"].ToString();
                    isemirleri.Add(isemri);
                }
                Dondur();
                return View(isemirleri.Where(x => x.is_SorumlulukMerkezi == "70" && x.msg_S_0355 == "Aktif").OrderBy(x => Convert.ToDateTime(x.msg_S_0366)).ToPagedList(sayfa, 20));
            }
            else
            {
                return RedirectToAction("KullaniciGirisi", "Giris");
            }
        }

        [Authorize(Roles = "1,2,3")]
        public ActionResult THTIsEmirleri(int sayfa = 1)
        {
            if (Session["KullaniciAdi"] as string != null)
            {
                var tbl_isemriserver = db.IsEmirleri.ToList();
                var tbl_isemrinotdurum = db.IsEmirleriNotDurum.ToList();

                var eksikisemri = tbl_isemriserver.Select(x => x.msg_S_0349).Except(tbl_isemrinotdurum.Select(x => x.msg_S_0349));
                var fazlaisemri = tbl_isemrinotdurum.Select(x => x.msg_S_0349).Except(tbl_isemriserver.Select(x => x.msg_S_0349));

                foreach (var isemrino in eksikisemri)
                {
                    db.IsEmirleriNotDurum.Add(new IsEmirleri_NotDurum { msg_S_0349 = isemrino });
                    db.SaveChanges();
                }

                foreach (var isemrino in fazlaisemri)
                {
                    var silinecek = db.IsEmirleriNotDurum.First(x => x.msg_S_0349 == isemrino);
                    db.IsEmirleriNotDurum.Remove(silinecek);
                    db.SaveChanges();
                }

                List<IsEmirleri> isemirleri = new List<IsEmirleri>();
                string connstr = ConfigurationManager.ConnectionStrings["Context"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(connstr);
                string isemriserver = "SELECT IsEmirleris.msg_S_0349, IsEmirleris.msg_S_0352, IsEmirleris.msg_S_0080, IsEmirleris.msg_S_0247, IsEmirleris.msg_S_0353, IsEmirleris.msg_S_0354, IsEmirleris.msg_S_0355, IsEmirleris.msg_S_0361, IsEmirleris.msg_S_0366," +
                    " is_SorumlulukMerkezi, IsEmirleri_NotDurum.[Not] FROM IsEmirleris INNER JOIN IsEmirleri_NotDurum ON IsEmirleris.msg_S_0349 = IsEmirleri_NotDurum.msg_S_0349";
                SqlCommand sqlcmd = new SqlCommand(isemriserver, sqlcon);
                SqlDataAdapter sda = new SqlDataAdapter(sqlcmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    IsEmirleri isemri = new IsEmirleri();
                    isemri.msg_S_0349 = dr["msg_S_0349"].ToString();
                    isemri.msg_S_0352 = dr["msg_S_0352"].ToString();
                    isemri.msg_S_0080 = dr["msg_S_0080"].ToString();
                    isemri.msg_S_0247 = dr["msg_S_0247"].ToString();
                    isemri.msg_S_0353 = dr["msg_S_0353"].ToString();
                    isemri.msg_S_0354 = dr["msg_S_0354"].ToString();
                    isemri.msg_S_0355 = dr["msg_S_0355"].ToString();
                    isemri.msg_S_0361 = Convert.ToDateTime(dr["msg_S_0361"]);
                    isemri.msg_S_0366 = Convert.ToDateTime(dr["msg_S_0366"]);
                    isemri.is_SorumlulukMerkezi = dr["is_SorumlulukMerkezi"].ToString();
                    isemri.Not = dr["Not"].ToString();
                    isemirleri.Add(isemri);
                }
                Dondur();
                return View(isemirleri.Where(x => (x.is_SorumlulukMerkezi == "80" || x.is_SorumlulukMerkezi == "90") && (x.msg_S_0355 == "Aktif")).OrderBy(x => Convert.ToDateTime(x.msg_S_0366)).ToPagedList(sayfa, 20));
            }
            else
            {
                return RedirectToAction("KullaniciGirisi", "Giris");
            }
        }

        public ActionResult IsEmriGetir(string id)
        {
            if (Session["KullaniciAdi"] as string != null)
            {
                var isemri = db.IsEmirleri.First(x => x.msg_S_0349 == id);
                var isemrinotdurum = db.IsEmirleriNotDurum.First(x => x.msg_S_0349 == isemri.msg_S_0349);
                ViewBag.IsEmriSorumluluk = isemri.is_SorumlulukMerkezi;
                ViewBag.IsEmri_msgS0355 = isemri.msg_S_0355;
                isemri.Not = isemrinotdurum.Not;
                isemri.Kullanici = isemrinotdurum.Kullanici;
                ViewBag.UrunIsim = isemri.msg_S_0353;
                Dondur();
                return View(isemri);
            }
            else
            {
                return RedirectToAction("KullaniciGirisi", "Giris");
            }
        }

        public ActionResult IsEmriDuzenle(IsEmirleri ie, IsEmirleri_NotDurum isnot)
        {
            var admin = Session["Personel"] as string;
            var isemri = db.IsEmirleri.First(x => x.msg_S_0349 == ie.msg_S_0349);
            var isemrinotdurum = db.IsEmirleriNotDurum.First(x => x.msg_S_0349 == isemri.msg_S_0349);
            if (isemri.is_SorumlulukMerkezi == "70")
            {
                isemrinotdurum.Not = isnot.Not;
                isemrinotdurum.Kullanici = admin;
                db.SaveChanges();
                return RedirectToAction("SMDIsEmirleri");
            }
            else if (isemri.msg_S_0355 == "Planlanmış")
            {
                isemrinotdurum.Not = isnot.Not;
                isemrinotdurum.Kullanici = admin;
                db.SaveChanges();
                return RedirectToAction("PlanliIsEmirleri");
            }
            else
            {
                isemrinotdurum.Not = isnot.Not;
                isemrinotdurum.Kullanici = admin;
                db.SaveChanges();
                return RedirectToAction("THTIsEmirleri");
            }
        }

        public ActionResult SMDUrunAlfabetik(int sayfa = 1)
        {
            List<IsEmirleri> isemirleri = new List<IsEmirleri>();
            string connstr = ConfigurationManager.ConnectionStrings["Context"].ConnectionString;
            SqlConnection sqlcon = new SqlConnection(connstr);
            string isemriserver = "SELECT IsEmirleris.msg_S_0349, IsEmirleris.msg_S_0352, IsEmirleris.msg_S_0353, IsEmirleris.msg_S_0354, IsEmirleris.msg_S_0080, IsEmirleris.msg_S_0247, IsEmirleris.msg_S_0355, IsEmirleris.msg_S_0361, IsEmirleris.msg_S_0366," +
                " IsEmirleris.is_SorumlulukMerkezi, IsEmirleri_NotDurum.[Not] FROM IsEmirleris INNER JOIN IsEmirleri_NotDurum ON IsEmirleris.msg_S_0349 = IsEmirleri_NotDurum.msg_S_0349";
            SqlCommand sqlcmd = new SqlCommand(isemriserver, sqlcon);
            SqlDataAdapter sda = new SqlDataAdapter(sqlcmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                IsEmirleri isemri = new IsEmirleri();
                isemri.msg_S_0349 = dr["msg_S_0349"].ToString();
                isemri.msg_S_0352 = dr["msg_S_0352"].ToString();
                isemri.msg_S_0353 = dr["msg_S_0353"].ToString();
                isemri.msg_S_0354 = dr["msg_S_0354"].ToString();
                isemri.msg_S_0080 = dr["msg_S_0080"].ToString();
                isemri.msg_S_0247 = dr["msg_S_0247"].ToString();
                isemri.msg_S_0355 = dr["msg_S_0355"].ToString();
                isemri.msg_S_0361 = Convert.ToDateTime(dr["msg_S_0361"]);
                isemri.msg_S_0366 = Convert.ToDateTime(dr["msg_S_0366"]);
                isemri.is_SorumlulukMerkezi = dr["is_SorumlulukMerkezi"].ToString();
                isemri.Not = dr["Not"].ToString();
                isemirleri.Add(isemri);
            }
            Dondur();
            return View("SMDIsEmirleri", isemirleri.Where(x => x.is_SorumlulukMerkezi == "70" && x.msg_S_0355 == "Aktif").OrderBy(x => x.msg_S_0353).ToPagedList(sayfa, 20));
        }

        public ActionResult SMDUrunTersAlfabetik(int sayfa = 1)
        {
            List<IsEmirleri> isemirleri = new List<IsEmirleri>();
            string connstr = ConfigurationManager.ConnectionStrings["Context"].ConnectionString;
            SqlConnection sqlcon = new SqlConnection(connstr);
            string isemriserver = "SELECT IsEmirleris.msg_S_0349, IsEmirleris.msg_S_0352, IsEmirleris.msg_S_0353, IsEmirleris.msg_S_0080, IsEmirleris.msg_S_0247, IsEmirleris.msg_S_0354, IsEmirleris.msg_S_0355, IsEmirleris.msg_S_0361, IsEmirleris.msg_S_0366," +
                " IsEmirleris.is_SorumlulukMerkezi, IsEmirleri_NotDurum.[Not] FROM IsEmirleris INNER JOIN IsEmirleri_NotDurum ON IsEmirleris.msg_S_0349 = IsEmirleri_NotDurum.msg_S_0349";
            SqlCommand sqlcmd = new SqlCommand(isemriserver, sqlcon);
            SqlDataAdapter sda = new SqlDataAdapter(sqlcmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                IsEmirleri isemri = new IsEmirleri();
                isemri.msg_S_0349 = dr["msg_S_0349"].ToString();
                isemri.msg_S_0352 = dr["msg_S_0352"].ToString();
                isemri.msg_S_0353 = dr["msg_S_0353"].ToString();
                isemri.msg_S_0354 = dr["msg_S_0354"].ToString();
                isemri.msg_S_0080 = dr["msg_S_0080"].ToString();
                isemri.msg_S_0247 = dr["msg_S_0247"].ToString();
                isemri.msg_S_0355 = dr["msg_S_0355"].ToString();
                isemri.msg_S_0361 = Convert.ToDateTime(dr["msg_S_0361"]);
                isemri.msg_S_0366 = Convert.ToDateTime(dr["msg_S_0366"]);
                isemri.is_SorumlulukMerkezi = dr["is_SorumlulukMerkezi"].ToString();
                isemri.Not = dr["Not"].ToString();
                isemirleri.Add(isemri);
            }
            Dondur();
            return View("SMDIsEmirleri", isemirleri.Where(x => x.is_SorumlulukMerkezi == "70" && x.msg_S_0355 == "Aktif").OrderByDescending(x => x.msg_S_0353).ToPagedList(sayfa, 20));
        }

        public ActionResult THTUrunAlfabetik(int sayfa = 1)
        {
            List<IsEmirleri> isemirleri = new List<IsEmirleri>();
            string connstr = ConfigurationManager.ConnectionStrings["Context"].ConnectionString;
            SqlConnection sqlcon = new SqlConnection(connstr);
            string isemriserver = "SELECT IsEmirleris.msg_S_0349, IsEmirleris.msg_S_0352, IsEmirleris.msg_S_0353, IsEmirleris.msg_S_0354, IsEmirleris.msg_S_0355, IsEmirleris.msg_S_0080, IsEmirleris.msg_S_0247, IsEmirleris.msg_S_0355, IsEmirleris.msg_S_0361, IsEmirleris.msg_S_0366," +
                " IsEmirleris.is_SorumlulukMerkezi, IsEmirleri_NotDurum.[Not] FROM IsEmirleris INNER JOIN IsEmirleri_NotDurum ON IsEmirleris.msg_S_0349 = IsEmirleri_NotDurum.msg_S_0349";
            SqlCommand sqlcmd = new SqlCommand(isemriserver, sqlcon);
            SqlDataAdapter sda = new SqlDataAdapter(sqlcmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                IsEmirleri isemri = new IsEmirleri();
                isemri.msg_S_0349 = dr["msg_S_0349"].ToString();
                isemri.msg_S_0352 = dr["msg_S_0352"].ToString();
                isemri.msg_S_0353 = dr["msg_S_0353"].ToString();
                isemri.msg_S_0354 = dr["msg_S_0354"].ToString();
                isemri.msg_S_0355 = dr["msg_S_0355"].ToString();
                isemri.msg_S_0080 = dr["msg_S_0080"].ToString();
                isemri.msg_S_0247 = dr["msg_S_0247"].ToString();
                isemri.msg_S_0355 = dr["msg_S_0355"].ToString();
                isemri.msg_S_0361 = Convert.ToDateTime(dr["msg_S_0361"]);
                isemri.msg_S_0366 = Convert.ToDateTime(dr["msg_S_0366"]);
                isemri.is_SorumlulukMerkezi = dr["is_SorumlulukMerkezi"].ToString();
                isemri.Not = dr["Not"].ToString();
                isemirleri.Add(isemri);
            }
            Dondur();
            return View("THTIsEmirleri", isemirleri.Where(x => (x.is_SorumlulukMerkezi == "80" || x.is_SorumlulukMerkezi == "90") && (x.msg_S_0355 == "Aktif")).OrderBy(x => x.msg_S_0353).ToPagedList(sayfa, 20));
        }

        public ActionResult THTUrunTersAlfabetik(int sayfa = 1)
        {
            List<IsEmirleri> isemirleri = new List<IsEmirleri>();
            string connstr = ConfigurationManager.ConnectionStrings["Context"].ConnectionString;
            SqlConnection sqlcon = new SqlConnection(connstr);
            string isemriserver = "SELECT IsEmirleris.msg_S_0349, IsEmirleris.msg_S_0352, IsEmirleris.msg_S_0353, IsEmirleris.msg_S_0354, IsEmirleris.msg_S_0355, IsEmirleris.msg_S_0080, IsEmirleris.msg_S_0247, IsEmirleris.msg_S_0361, IsEmirleris.msg_S_0366," +
                " IsEmirleris.is_SorumlulukMerkezi, IsEmirleri_NotDurum.[Not] FROM IsEmirleris INNER JOIN IsEmirleri_NotDurum ON IsEmirleris.msg_S_0349 = IsEmirleri_NotDurum.msg_S_0349";
            SqlCommand sqlcmd = new SqlCommand(isemriserver, sqlcon);
            SqlDataAdapter sda = new SqlDataAdapter(sqlcmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                IsEmirleri isemri = new IsEmirleri();
                isemri.msg_S_0349 = dr["msg_S_0349"].ToString();
                isemri.msg_S_0352 = dr["msg_S_0352"].ToString();
                isemri.msg_S_0353 = dr["msg_S_0353"].ToString();
                isemri.msg_S_0354 = dr["msg_S_0354"].ToString();
                isemri.msg_S_0355 = dr["msg_S_0355"].ToString();
                isemri.msg_S_0080 = dr["msg_S_0080"].ToString();
                isemri.msg_S_0247 = dr["msg_S_0247"].ToString();
                isemri.msg_S_0361 = Convert.ToDateTime(dr["msg_S_0361"]);
                isemri.msg_S_0366 = Convert.ToDateTime(dr["msg_S_0366"]);
                isemri.is_SorumlulukMerkezi = dr["is_SorumlulukMerkezi"].ToString();
                isemri.Not = dr["Not"].ToString();
                isemirleri.Add(isemri);
            }
            Dondur();
            return View("THTIsEmirleri", isemirleri.Where(x => x.is_SorumlulukMerkezi == "80" || x.is_SorumlulukMerkezi == "90" && (x.msg_S_0355 == "Aktif")).OrderByDescending(x => x.msg_S_0353).ToPagedList(sayfa, 20));
        }

        static int smdtersisirala;

        public ActionResult SMDTersSiralama(int sayfa = 1)
        {
            Dondur();
            if (smdtersisirala == 0)
            {
                smdtersisirala = 1;
                List<IsEmirleri> isemirleri = new List<IsEmirleri>();
                string connstr = ConfigurationManager.ConnectionStrings["Context"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(connstr);
                string isemriserver = "SELECT IsEmirleris.msg_S_0349, IsEmirleris.ID, IsEmirleris.msg_S_0352, IsEmirleris.msg_S_0353, IsEmirleris.msg_S_0355, IsEmirleris.msg_S_0080, IsEmirleris.msg_S_0247, IsEmirleris.msg_S_0354, IsEmirleris.msg_S_0361, IsEmirleris.msg_S_0366," +
                    " IsEmirleris.is_SorumlulukMerkezi, IsEmirleri_NotDurum.[Not] FROM IsEmirleris INNER JOIN IsEmirleri_NotDurum ON IsEmirleris.msg_S_0349 = IsEmirleri_NotDurum.msg_S_0349";
                SqlCommand sqlcmd = new SqlCommand(isemriserver, sqlcon);
                SqlDataAdapter sda = new SqlDataAdapter(sqlcmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    IsEmirleri isemri = new IsEmirleri();
                    isemri.ID = Convert.ToInt16(dr["ID"]);
                    isemri.msg_S_0349 = dr["msg_S_0349"].ToString();
                    isemri.msg_S_0352 = dr["msg_S_0352"].ToString();
                    isemri.msg_S_0353 = dr["msg_S_0353"].ToString();
                    isemri.msg_S_0354 = dr["msg_S_0354"].ToString();
                    isemri.msg_S_0355 = dr["msg_S_0355"].ToString();
                    isemri.msg_S_0080 = dr["msg_S_0080"].ToString();
                    isemri.msg_S_0247 = dr["msg_S_0247"].ToString();
                    isemri.msg_S_0361 = Convert.ToDateTime(dr["msg_S_0361"]);
                    isemri.msg_S_0366 = Convert.ToDateTime(dr["msg_S_0366"]);
                    isemri.is_SorumlulukMerkezi = dr["is_SorumlulukMerkezi"].ToString();
                    isemri.Not = dr["Not"].ToString();
                    isemirleri.Add(isemri);
                }
                return View("SMDIsEmirleri", isemirleri.Where(x => x.is_SorumlulukMerkezi == "70" && x.msg_S_0355 == "Aktif").OrderByDescending(x => x.ID).ToPagedList(sayfa, 20));
            }
            else
            {
                smdtersisirala = 0;
                List<IsEmirleri> isemirleri = new List<IsEmirleri>();
                string connstr = ConfigurationManager.ConnectionStrings["Context"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(connstr);
                string isemriserver = "SELECT IsEmirleris.msg_S_0349, IsEmirleris.ID, IsEmirleris.msg_S_0352, IsEmirleris.msg_S_0353, IsEmirleris.msg_S_0355, IsEmirleris.msg_S_0080, IsEmirleris.msg_S_0247, IsEmirleris.msg_S_0354, IsEmirleris.msg_S_0361, IsEmirleris.msg_S_0366," +
                    " IsEmirleris.is_SorumlulukMerkezi, IsEmirleri_NotDurum.[Not] FROM IsEmirleris INNER JOIN IsEmirleri_NotDurum ON IsEmirleris.msg_S_0349 = IsEmirleri_NotDurum.msg_S_0349";
                SqlCommand sqlcmd = new SqlCommand(isemriserver, sqlcon);
                SqlDataAdapter sda = new SqlDataAdapter(sqlcmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    IsEmirleri isemri = new IsEmirleri();
                    isemri.ID = Convert.ToInt16(dr["ID"]);
                    isemri.msg_S_0349 = dr["msg_S_0349"].ToString();
                    isemri.msg_S_0352 = dr["msg_S_0352"].ToString();
                    isemri.msg_S_0353 = dr["msg_S_0353"].ToString();
                    isemri.msg_S_0354 = dr["msg_S_0354"].ToString();
                    isemri.msg_S_0355 = dr["msg_S_0355"].ToString();
                    isemri.msg_S_0080 = dr["msg_S_0080"].ToString();
                    isemri.msg_S_0247 = dr["msg_S_0247"].ToString();
                    isemri.msg_S_0361 = Convert.ToDateTime(dr["msg_S_0361"]);
                    isemri.msg_S_0366 = Convert.ToDateTime(dr["msg_S_0366"]);
                    isemri.is_SorumlulukMerkezi = dr["is_SorumlulukMerkezi"].ToString();
                    isemri.Not = dr["Not"].ToString();
                    isemirleri.Add(isemri);
                }
                return View("SMDIsEmirleri", isemirleri.Where(x => x.is_SorumlulukMerkezi == "70" && x.msg_S_0355 == "Aktif").OrderBy(x => x.ID).ToPagedList(sayfa, 20));
            }
        }

        static int thttersisirala;

        public ActionResult THTTersSiralama(int sayfa = 1)
        {
            Dondur();
            if (thttersisirala == 0)
            {
                thttersisirala = 1;
                List<IsEmirleri> isemirleri = new List<IsEmirleri>();
                string connstr = ConfigurationManager.ConnectionStrings["Context"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(connstr);
                string isemriserver = "SELECT IsEmirleris.msg_S_0349, IsEmirleris.ID, IsEmirleris.msg_S_0352, IsEmirleris.msg_S_0353, IsEmirleris.msg_S_0355, IsEmirleris.msg_S_0080, IsEmirleris.msg_S_0247, IsEmirleris.msg_S_0354, IsEmirleris.msg_S_0361, IsEmirleris.msg_S_0366," +
                    " is_SorumlulukMerkezi, IsEmirleri_NotDurum.[Not] FROM IsEmirleris INNER JOIN IsEmirleri_NotDurum ON IsEmirleris.msg_S_0349 = IsEmirleri_NotDurum.msg_S_0349";
                SqlCommand sqlcmd = new SqlCommand(isemriserver, sqlcon);
                SqlDataAdapter sda = new SqlDataAdapter(sqlcmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    IsEmirleri isemri = new IsEmirleri();
                    isemri.ID = Convert.ToInt16(dr["ID"]);
                    isemri.msg_S_0349 = dr["msg_S_0349"].ToString();
                    isemri.msg_S_0352 = dr["msg_S_0352"].ToString();
                    isemri.msg_S_0353 = dr["msg_S_0353"].ToString();
                    isemri.msg_S_0354 = dr["msg_S_0354"].ToString();
                    isemri.msg_S_0355 = dr["msg_S_0355"].ToString();
                    isemri.msg_S_0080 = dr["msg_S_0080"].ToString();
                    isemri.msg_S_0247 = dr["msg_S_0247"].ToString();
                    isemri.msg_S_0361 = Convert.ToDateTime(dr["msg_S_0361"]);
                    isemri.msg_S_0366 = Convert.ToDateTime(dr["msg_S_0366"]);
                    isemri.is_SorumlulukMerkezi = dr["is_SorumlulukMerkezi"].ToString();
                    isemri.Not = dr["Not"].ToString();
                    isemirleri.Add(isemri);
                }
                return View("THTIsEmirleri", isemirleri.Where(x => (x.is_SorumlulukMerkezi == "80" || x.is_SorumlulukMerkezi == "90") && (x.msg_S_0355 == "Aktif")).OrderByDescending(x => x.ID).ToPagedList(sayfa, 20));
            }
            else
            {
                thttersisirala = 0;
                List<IsEmirleri> isemirleri = new List<IsEmirleri>();
                string connstr = ConfigurationManager.ConnectionStrings["Context"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(connstr);
                string isemriserver = "SELECT IsEmirleris.msg_S_0349, IsEmirleris.ID, IsEmirleris.msg_S_0352, IsEmirleris.msg_S_0353, IsEmirleris.msg_S_0355, IsEmirleris.msg_S_0080, IsEmirleris.msg_S_0247, IsEmirleris.msg_S_0354, IsEmirleris.msg_S_0361, IsEmirleris.msg_S_0366," +
                    " is_SorumlulukMerkezi, IsEmirleri_NotDurum.[Not] FROM IsEmirleris INNER JOIN IsEmirleri_NotDurum ON IsEmirleris.msg_S_0349 = IsEmirleri_NotDurum.msg_S_0349";
                SqlCommand sqlcmd = new SqlCommand(isemriserver, sqlcon);
                SqlDataAdapter sda = new SqlDataAdapter(sqlcmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    IsEmirleri isemri = new IsEmirleri();
                    isemri.ID = Convert.ToInt16(dr["ID"]);
                    isemri.msg_S_0349 = dr["msg_S_0349"].ToString();
                    isemri.msg_S_0352 = dr["msg_S_0352"].ToString();
                    isemri.msg_S_0353 = dr["msg_S_0353"].ToString();
                    isemri.msg_S_0354 = dr["msg_S_0354"].ToString();
                    isemri.msg_S_0355 = dr["msg_S_0355"].ToString();
                    isemri.msg_S_0080 = dr["msg_S_0080"].ToString();
                    isemri.msg_S_0247 = dr["msg_S_0247"].ToString();
                    isemri.msg_S_0361 = Convert.ToDateTime(dr["msg_S_0361"]);
                    isemri.msg_S_0366 = Convert.ToDateTime(dr["msg_S_0366"]);
                    isemri.is_SorumlulukMerkezi = dr["is_SorumlulukMerkezi"].ToString();
                    isemri.Not = dr["Not"].ToString();
                    isemirleri.Add(isemri);
                }
                return View("THTIsEmirleri", isemirleri.Where(x => (x.is_SorumlulukMerkezi == "80" || x.is_SorumlulukMerkezi == "90") && (x.msg_S_0355 == "Aktif")).OrderBy(x => x.ID).ToPagedList(sayfa, 20));
            }
        }

        public ActionResult SMDFiltre(string smdfiltre, int sayfa = 1)
        {
            if (Session["KullaniciAdi"] as string != null)
            {
                List<IsEmirleri> isemirleri = new List<IsEmirleri>();
                string connstr = ConfigurationManager.ConnectionStrings["Context"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(connstr);
                string isemriserver = "SELECT IsEmirleris.msg_S_0349, IsEmirleris.msg_S_0080, IsEmirleris.msg_S_0247, IsEmirleris.msg_S_0352, IsEmirleris.msg_S_0353, IsEmirleris.msg_S_0354, IsEmirleris.msg_S_0355, IsEmirleris.msg_S_0360, IsEmirleris.msg_S_0361, IsEmirleris.msg_S_0366," +
                    " is_SorumlulukMerkezi, IsEmirleri_NotDurum.[Not] FROM IsEmirleris INNER JOIN IsEmirleri_NotDurum ON IsEmirleris.msg_S_0349 = IsEmirleri_NotDurum.msg_S_0349";
                SqlCommand sqlcmd = new SqlCommand(isemriserver, sqlcon);
                SqlDataAdapter sda = new SqlDataAdapter(sqlcmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    IsEmirleri isemri = new IsEmirleri();
                    isemri.msg_S_0349 = dr["msg_S_0349"].ToString();
                    isemri.msg_S_0080 = dr["msg_S_0080"].ToString();
                    isemri.msg_S_0247 = dr["msg_S_0247"].ToString();
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
                Dondur();
                return View("SMDIsEmirleri", isemirleri.Where(x => (x.is_SorumlulukMerkezi == "70") && (x.msg_S_0355 == "Aktif") &&
                                                             (x.msg_S_0349.Contains(smdfiltre) ||
                                                              x.msg_S_0352.Contains(smdfiltre) ||
                                                              x.msg_S_0353.ToLower().Contains(smdfiltre) || x.msg_S_0353.ToUpper().Contains(smdfiltre))).OrderBy(x => x.msg_S_0361).ToPagedList(sayfa, 20));
            }
            else
            {
                return RedirectToAction("KullaniciGirisi", "Giris");
            }
        }

        public ActionResult THTFiltre(string thtfiltre, int sayfa = 1)
        {
            if (Session["KullaniciAdi"] as string != null)
            {
                List<IsEmirleri> isemirleri = new List<IsEmirleri>();
                string connstr = ConfigurationManager.ConnectionStrings["Context"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(connstr);
                string isemriserver = "SELECT IsEmirleris.msg_S_0349, IsEmirleris.msg_S_0080, IsEmirleris.msg_S_0247, IsEmirleris.msg_S_0352, IsEmirleris.msg_S_0353, IsEmirleris.msg_S_0354, IsEmirleris.msg_S_0355, IsEmirleris.msg_S_0361, IsEmirleris.msg_S_0366," +
                    " is_SorumlulukMerkezi, IsEmirleri_NotDurum.[Not] FROM IsEmirleris INNER JOIN IsEmirleri_NotDurum ON IsEmirleris.msg_S_0349 = IsEmirleri_NotDurum.msg_S_0349";
                SqlCommand sqlcmd = new SqlCommand(isemriserver, sqlcon);
                SqlDataAdapter sda = new SqlDataAdapter(sqlcmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    IsEmirleri isemri = new IsEmirleri();
                    isemri.msg_S_0349 = dr["msg_S_0349"].ToString();
                    isemri.msg_S_0080 = dr["msg_S_0080"].ToString();
                    isemri.msg_S_0247 = dr["msg_S_0247"].ToString();
                    isemri.msg_S_0352 = dr["msg_S_0352"].ToString();
                    isemri.msg_S_0353 = dr["msg_S_0353"].ToString();
                    isemri.msg_S_0354 = dr["msg_S_0354"].ToString();
                    isemri.msg_S_0355 = dr["msg_S_0355"].ToString();
                    isemri.msg_S_0361 = Convert.ToDateTime(dr["msg_S_0361"]);
                    isemri.msg_S_0366 = Convert.ToDateTime(dr["msg_S_0366"]);
                    isemri.is_SorumlulukMerkezi = dr["is_SorumlulukMerkezi"].ToString();
                    isemri.Not = dr["Not"].ToString();
                    isemirleri.Add(isemri);
                }
                Dondur();
                return View("THTIsEmirleri", isemirleri.Where(x => (x.is_SorumlulukMerkezi == "80" || x.is_SorumlulukMerkezi == "90") && (x.msg_S_0355 == "Aktif") &&
                                                             (x.msg_S_0349.Contains(thtfiltre) ||
                                                              x.msg_S_0352.Contains(thtfiltre) ||
                                                              x.msg_S_0353.ToLower().Contains(thtfiltre) || x.msg_S_0353.ToUpper().Contains(thtfiltre))).OrderBy(x => x.msg_S_0361).ToPagedList(sayfa, 20));
            }
            else
            {
                return RedirectToAction("KullaniciGirisi", "Giris");
            }
        }

        [Authorize(Roles = "1,2")]
        public ActionResult IsEmirleriKapananlar(int sayfa = 1)
        {
            if (Session["KullaniciAdi"] as string != null)
            {
                List<IsEmirleri> isemirleri = new List<IsEmirleri>();
                string connstr = ConfigurationManager.ConnectionStrings["Context"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(connstr);
                string isemriserver = "SELECT IsEmirleris.msg_S_0349, IsEmirleris.msg_S_0352, IsEmirleris.msg_S_0353, IsEmirleris.msg_S_0354, IsEmirleris.msg_S_0355, IsEmirleris.msg_S_0360, IsEmirleris.msg_S_0361, IsEmirleris.msg_S_0366," +
                    " is_SorumlulukMerkezi, IsEmirleri_NotDurum.[Not] FROM IsEmirleris INNER JOIN IsEmirleri_NotDurum ON IsEmirleris.msg_S_0349 = IsEmirleri_NotDurum.msg_S_0349";
                SqlCommand sqlcmd = new SqlCommand(isemriserver, sqlcon);
                SqlDataAdapter sda = new SqlDataAdapter(sqlcmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow dr in dt.Rows)
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
                Dondur();
                return View(isemirleri.Where(x => x.msg_S_0355 == "Kapanmış").OrderByDescending(x => x.msg_S_0361).ToPagedList(sayfa, 20));
            }
            else
            {
                return RedirectToAction("KullaniciGirisi", "Giris");
            }
        }

        public ActionResult KapaliIsEmriUrunAlfabetik(int sayfa = 1)
        {
            List<IsEmirleri> isemirleri = new List<IsEmirleri>();
            string connstr = ConfigurationManager.ConnectionStrings["Context"].ConnectionString;
            SqlConnection sqlcon = new SqlConnection(connstr);
            string isemriserver = "SELECT IsEmirleris.msg_S_0349, IsEmirleris.msg_S_0352, IsEmirleris.msg_S_0353, IsEmirleris.msg_S_0354, IsEmirleris.msg_S_0355, IsEmirleris.msg_S_0361, IsEmirleris.msg_S_0366," +
                " IsEmirleris.is_SorumlulukMerkezi, IsEmirleri_NotDurum.[Not] FROM IsEmirleris INNER JOIN IsEmirleri_NotDurum ON IsEmirleris.msg_S_0349 = IsEmirleri_NotDurum.msg_S_0349";
            SqlCommand sqlcmd = new SqlCommand(isemriserver, sqlcon);
            SqlDataAdapter sda = new SqlDataAdapter(sqlcmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                IsEmirleri isemri = new IsEmirleri();
                isemri.msg_S_0349 = dr["msg_S_0349"].ToString();
                isemri.msg_S_0352 = dr["msg_S_0352"].ToString();
                isemri.msg_S_0353 = dr["msg_S_0353"].ToString();
                isemri.msg_S_0354 = dr["msg_S_0354"].ToString();
                isemri.msg_S_0355 = dr["msg_S_0355"].ToString();
                isemri.msg_S_0361 = Convert.ToDateTime(dr["msg_S_0361"]);
                isemri.msg_S_0366 = Convert.ToDateTime(dr["msg_S_0366"]);
                isemri.is_SorumlulukMerkezi = dr["is_SorumlulukMerkezi"].ToString();
                isemri.Not = dr["Not"].ToString();
                isemirleri.Add(isemri);
            }
            Dondur();
            return View("IsEmirleriKapananlar", isemirleri.Where(x => x.msg_S_0355 == "Kapanmış").OrderBy(x => x.msg_S_0353).ToPagedList(sayfa, 20));
        }

        public ActionResult KapaliIsEmriUrunTersAlfabetik(int sayfa = 1)
        {
            List<IsEmirleri> isemirleri = new List<IsEmirleri>();
            string connstr = ConfigurationManager.ConnectionStrings["Context"].ConnectionString;
            SqlConnection sqlcon = new SqlConnection(connstr);
            string isemriserver = "SELECT IsEmirleris.msg_S_0349, IsEmirleris.msg_S_0352, IsEmirleris.msg_S_0353, IsEmirleris.msg_S_0354, IsEmirleris.msg_S_0355, IsEmirleris.msg_S_0361, IsEmirleris.msg_S_0366," +
                " IsEmirleris.is_SorumlulukMerkezi, IsEmirleri_NotDurum.[Not] FROM IsEmirleris INNER JOIN IsEmirleri_NotDurum ON IsEmirleris.msg_S_0349 = IsEmirleri_NotDurum.msg_S_0349";
            SqlCommand sqlcmd = new SqlCommand(isemriserver, sqlcon);
            SqlDataAdapter sda = new SqlDataAdapter(sqlcmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                IsEmirleri isemri = new IsEmirleri();
                isemri.msg_S_0349 = dr["msg_S_0349"].ToString();
                isemri.msg_S_0352 = dr["msg_S_0352"].ToString();
                isemri.msg_S_0353 = dr["msg_S_0353"].ToString();
                isemri.msg_S_0354 = dr["msg_S_0354"].ToString();
                isemri.msg_S_0355 = dr["msg_S_0355"].ToString();
                isemri.msg_S_0361 = Convert.ToDateTime(dr["msg_S_0361"]);
                isemri.msg_S_0366 = Convert.ToDateTime(dr["msg_S_0366"]);
                isemri.is_SorumlulukMerkezi = dr["is_SorumlulukMerkezi"].ToString();
                isemri.Not = dr["Not"].ToString();
                isemirleri.Add(isemri);
            }
            Dondur();
            return View("IsEmirleriKapananlar", isemirleri.Where(x => x.msg_S_0355 == "Kapanmış").OrderByDescending(x => x.msg_S_0353).ToPagedList(sayfa, 20));
        }

        public ActionResult KapaliIsEmriFiltre(string kapaliisemrifiltre, int sayfa = 1)
        {
            if (Session["KullaniciAdi"] as string != null)
            {
                List<IsEmirleri> isemirleri = new List<IsEmirleri>();
                string connstr = ConfigurationManager.ConnectionStrings["Context"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(connstr);
                string isemriserver = "SELECT IsEmirleris.msg_S_0349, IsEmirleris.msg_S_0352, IsEmirleris.msg_S_0353, IsEmirleris.msg_S_0354, IsEmirleris.msg_S_0355, IsEmirleris.msg_S_0360, IsEmirleris.msg_S_0361, IsEmirleris.msg_S_0366," +
                    " is_SorumlulukMerkezi, IsEmirleri_NotDurum.[Not] FROM IsEmirleris INNER JOIN IsEmirleri_NotDurum ON IsEmirleris.msg_S_0349 = IsEmirleri_NotDurum.msg_S_0349";
                SqlCommand sqlcmd = new SqlCommand(isemriserver, sqlcon);
                SqlDataAdapter sda = new SqlDataAdapter(sqlcmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow dr in dt.Rows)
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
                Dondur();
                return View("IsEmirleriKapananlar", isemirleri.Where(x => x.msg_S_0355 == "Kapanmış" && (x.msg_S_0349.Contains(kapaliisemrifiltre) || x.msg_S_0352.Contains(kapaliisemrifiltre) || x.msg_S_0353.Contains(kapaliisemrifiltre) || x.msg_S_0354.Contains(kapaliisemrifiltre)
                || x.msg_S_0355.Contains(kapaliisemrifiltre) || x.is_SorumlulukMerkezi.Contains(kapaliisemrifiltre) || x.Not.Contains(kapaliisemrifiltre))).OrderBy(x => x.msg_S_0361).ToPagedList(sayfa, 30));
            }
            else
            {
                return RedirectToAction("KullaniciGirisi", "Giris");
            }
        }

        [Authorize(Roles = "1,2,3")]
        public ActionResult THTKapaliIsEmirleri()
        {
            Dondur();
            var tht_kapali_isemri = db.IsEmirleri.Where(x => (x.msg_S_0355 == "Kapanmış") && (x.is_SorumlulukMerkezi == "80" || x.is_SorumlulukMerkezi == "90")).ToList();
            return View(tht_kapali_isemri);
        }

        [Authorize(Roles = "1,2,3")]
        public ActionResult THTKapaliIsEmriFiltre(string query)
        {
            Dondur();
            var query_result = db.IsEmirleri.ToList();
            return View("THTKapaliIsEmirleri", query_result.Where(x => (x.msg_S_0349.Equals(query) || x.msg_S_0352.Equals(query) || x.msg_S_0353.ToUpper().Contains(query) || x.msg_S_0353.ToLower().Contains(query)) && (x.msg_S_0355 == "Kapanmış")).ToList());
        }

        [Authorize(Roles = "1,2,4")]
        public ActionResult SMDKapaliIsEmirleri()
        {
            Dondur();
            var smd_kapali_isemri = db.IsEmirleri.Where(x => (x.msg_S_0355 == "Kapanmış") && (x.is_SorumlulukMerkezi == "70")).ToList();
            return View(smd_kapali_isemri);
        }

        [Authorize(Roles = "1,2,4")]
        public ActionResult SMDKapaliIsEmriFiltre(string query)
        {
            Dondur();
            var query_result = db.IsEmirleri.ToList();
            return View("SMDKapaliIsEmirleri", query_result.Where(x => (x.msg_S_0349.Equals(query) || x.msg_S_0352.Equals(query) || x.msg_S_0353.ToUpper().Contains(query) || x.msg_S_0353.ToLower().Contains(query)) && (x.msg_S_0355 == "Kapanmış")).ToList());
        }

    }
}