using Mart_SiparisTakipMVC.Models.Database;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using ClosedXML.Excel;
using System.IO;

namespace Mart_SiparisTakipMVC.Controllers
{
    public class TedarikSiparisleriController : Controller
    {
        Context db = new Context();

        // GET: TedarikSiparisleri

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

        public ActionResult Index(int sayfa = 1)
        {
            Dondur();
            if (Session["KullaniciAdi"] as string != null)
            {
                var tedariksiparisler = db.GuncelTedarikSiparislers.OrderBy(x => x.Teslim_tarihi).ToPagedList(sayfa, 50);
                return View(tedariksiparisler);
            }
            else
            {
                return RedirectToAction("KullaniciGirisi", "Giris");
            }
        }

        public ActionResult TedarikSiparisFilter(GuncelTedarikSiparisler ts, string f, int sayfa = 1)
        {
            Dondur();
            var serisirano = ts.Evrak_seri_no + " " + ts.Evrak_sira_no;
            var tedariksip = db.GuncelTedarikSiparislers.ToList();
            var filter = tedariksip.Where(x => x.Stok_kodu.ToUpper().Contains(f) || x.Stok_kodu.ToLower().Contains(f)
                                                          || x.Cari_ismi.ToUpper().Contains(f) || x.Cari_ismi.ToLower().Contains(f)
                                                          || x.Stok_isim.ToUpper().Contains(f) || x.Stok_isim.ToLower().Contains(f)).OrderBy(x => x.Teslim_tarihi).ToPagedList(sayfa, 50);
            return View("Index", filter);
        }

        public ActionResult ExcelExport()
        {
            Dondur();
            string tarih = System.DateTime.Now.ToString("dd.MM.yyyy_HH.mm");

            using (var workbook = new XLWorkbook())
            {
                var tedariksip = db.GuncelTedarikSiparislers.ToList();
                var ws = workbook.Worksheets.Add("TedarikSiparisleriv");
                ws.Cell(1, 1).Value = "Kodu";
                ws.Cell(1, 2).Value = "Ürün İsmi";
                ws.Cell(1, 3).Value = "Sipariş Tarihi";
                ws.Cell(1, 4).Value = "Teslim Tarihi";
                ws.Cell(1, 5).Value = "Seri No";
                ws.Cell(1, 6).Value = "Sıra No";
                ws.Cell(1, 7).Value = "Cari";
                ws.Cell(1, 8).Value = "Sipariş Miktar";
                ws.Cell(1, 9).Value = "Tamamlanan Miktar";
                ws.Cell(1, 10).Value = "Kalan Sipariş Miktar";
                ws.Cell(1, 11).Value = "Birimi";
                ws.Cell(1, 12).Value = "Sipariş Cinsi";
                int row = 2;
                foreach (var item in tedariksip)
                {
                    ws.Cell(row, 1).Value = item.Stok_kodu;
                    ws.Cell(row, 2).Value = item.Stok_isim;
                    ws.Cell(row, 3).Value = item.Siparis_tarihi;
                    ws.Cell(row, 4).Value = item.Teslim_tarihi;
                    ws.Cell(row, 5).Value = item.Evrak_seri_no;
                    ws.Cell(row, 6).Value = item.Evrak_sira_no;
                    ws.Cell(row, 7).Value = item.Cari_ismi;
                    ws.Cell(row, 8).Value = item.Siparis_miktar;
                    ws.Cell(row, 9).Value = item.Tamamlanan_miktar;
                    ws.Cell(row, 10).Value = item.Kalan_miktar;
                    ws.Cell(row, 11).Value = item.Birim;
                    ws.Cell(row, 12).Value = item.Siparis_cinsi;
                    row++;
                }
                var titles = ws.Cells("A1:M1").Style.Font.Bold = true;

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "TedarikSiparisleri (" + tarih + ").xlsx");
                }
            }
        }

        static int order;
        public ActionResult TersSiralama(int sayfa = 1)
        {
            Dondur();
            if (order == 0)
            {
                order = 1;
                return View("Index", db.GuncelTedarikSiparislers.OrderByDescending(x => x.Teslim_tarihi).ToPagedList(sayfa, 50));
            }
            else if (order == 1)
            {
                order = 0;
                return View("Index", db.GuncelTedarikSiparislers.OrderBy(x => x.Teslim_tarihi).ToPagedList(sayfa, 50));
            }
            else
            {
                return View("Index");
            }
        }
    }
}