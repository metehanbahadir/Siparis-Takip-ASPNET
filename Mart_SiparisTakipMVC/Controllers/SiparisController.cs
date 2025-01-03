using DocumentFormat.OpenXml.Spreadsheet;
using Mart_SiparisTakipMVC.Models.Database;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using ClosedXML.Excel;

namespace Mart_SiparisTakipMVC.Controllers
{
    public class SiparisController : Controller
    {
        // GET: Siparis     
        Context db = new Context();

        public ActionResult NullError()
        {
            Dondur();
            return View();
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

        // action içerisinde sadece MailGonder ile birlikte parametreleri yazıp mail gönderebileceğimiz fonksyion
        public void MailGonder(string to, string subject, string body)
        {
            var from = "bildirim@martelektronik.com";
            MailMessage newMail = new MailMessage(from, to);
            newMail.From = new MailAddress(from, "Mart Elektronik");
            newMail.Subject = subject;
            newMail.Body = body;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "mail.martelektronik.com";
            smtp.Port = 587;
            smtp.EnableSsl = false;

            NetworkCredential credential = new NetworkCredential("bildirim@martelektronik.com", "MartBildir24!!");
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = credential;
            smtp.Send(newMail);
        }

        [Authorize(Roles = "1,2")]
        public ActionResult Index(GuncelSiparisler gs, int sayfa = 1)
        {
            if (Session["KullaniciAdi"] as string != null)
            {
                var tbl_guncelserver = db.GuncelSiparislers.ToList();
                var tbl_guncelnot = db.GuncelSiparislersNotDurum.ToList();

                var serino = tbl_guncelserver.Select(x => x.sip_evrakno_seri);
                var sirano = tbl_guncelserver.Select(x => x.sip_evrakno_sira);

                var eksikUID = tbl_guncelserver.Select(x => x.UID).Except(tbl_guncelnot.Select(x => x.UID));
                var fazlaUID = tbl_guncelnot.Select(x => x.UID).Except(tbl_guncelserver.Select(x => x.UID));

                foreach (var uid in eksikUID)
                {
                    db.GuncelSiparislersNotDurum.Add(new GuncelSiparislers_NotDurum { UID = uid });
                    db.SaveChanges();
                }

                foreach (var uid in fazlaUID)
                {
                    var silinecek = db.GuncelSiparislersNotDurum.Find(uid);
                    db.GuncelSiparislersNotDurum.Remove(silinecek);
                    db.SaveChanges();
                }

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
                    gs = new GuncelSiparisler();
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
                    if (gs.Durum != "")
                    {
                        gs.Durum = gs.Durum;
                    }
                    else
                    {
                        gs.Durum = "İşleme Alındı";
                    }
                    guncels.Add(gs);
                }
                Dondur();
                return View(guncels.Where(x => x.Durum != "Askıya Alındı").OrderBy(x => x.sip_teslim_tarih).ToPagedList(sayfa, 50));
            }
            else
            {
                return RedirectToAction("KullaniciGirisi", "Giris");
            }
        }

        public ActionResult GorevMailHatirlat(ExceptionContext filtercontext)
        {
            var today = System.DateTime.Today;
            var tasks = db.Gorevler.ToList();
            var task_today = tasks.Where(x => (Convert.ToDateTime(x.Bitis_Tarihi) == today) && (x.Tamamlanma_Tarihi == null)).ToList();
            var task_expired = tasks.Where(x => (Convert.ToDateTime(x.Bitis_Tarihi) < today) && (x.Tamamlanma_Tarihi == null)).ToList();
            //var task_last3days = tasks.Where(x => ((Convert.ToDateTime(x.Bitis_Tarihi) - today).Days < 4 && (Convert.ToDateTime(x.Bitis_Tarihi) - today).Days > 0) && (x.Tamamlanma_Tarihi == null)).ToList();

            var tasktodaycount = task_today.Count();
            var taskexpiredcount = task_expired.Count();
            //var tasklast3days_count = task_last3days.Count();

            try
            {
                foreach (var task in task_today)
                {
                    var from = "bildirim@martelektronik.com";
                    var appointee1 = db.Kullanici.FirstOrDefault(x => x.KullaniciAdi == task.Atanan1ID);
                    var appointee2 = db.Kullanici.FirstOrDefault(x => x.KullaniciAdi == task.Atanan2ID);
                    var appointee3 = db.Kullanici.FirstOrDefault(x => x.KullaniciAdi == task.Atanan3ID);
                    var appointed = db.Kullanici.FirstOrDefault(x => x.KullaniciAdi == task.AtayanID);

                    if (appointee1 != null)
                    {
                        MailMessage gorevmail_toappointee1 = new MailMessage(from, appointee1.Mail);
                        gorevmail_toappointee1.From = new MailAddress(from, "Mart Elektronik");
                        gorevmail_toappointee1.Subject = "Görev Hatırlatma";
                        gorevmail_toappointee1.Body = "Sayın " + task.Atanan1 + ", " + "\r\n" + "\r\n" + "'" + task.Atayan + "'" + " tarafından atanan '" + task.Gorev + "' görevinizin Bitiş Tarihi bugündür.";

                        SmtpClient smtp1 = new SmtpClient();
                        smtp1.Host = "mail.martelektronik.com";
                        smtp1.Port = 587;
                        smtp1.EnableSsl = false;

                        NetworkCredential credential1 = new NetworkCredential("bildirim@martelektronik.com", "MartBildir24!!");
                        smtp1.UseDefaultCredentials = true;
                        smtp1.Credentials = credential1;
                        smtp1.Send(gorevmail_toappointee1);
                    }
                    if (appointee2 != null)
                    {
                        MailMessage gorevmail_toappointee2 = new MailMessage(from, appointee2.Mail);
                        gorevmail_toappointee2.From = new MailAddress(from, "Mart Elektronik");
                        gorevmail_toappointee2.Subject = "Görev Hatırlatma";
                        gorevmail_toappointee2.Body = "Sayın " + task.Atanan2 + ", " + "\r\n" + "\r\n" + "'" + task.Atayan + "'" + " tarafından atanan '" + task.Gorev + "' görevinizin Bitiş Tarihi bugündür.";

                        SmtpClient smtp2 = new SmtpClient();
                        smtp2.Host = "mail.martelektronik.com";
                        smtp2.Port = 587;
                        smtp2.EnableSsl = false;

                        NetworkCredential credential2 = new NetworkCredential("bildirim@martelektronik.com", "MartBildir24!!");
                        smtp2.UseDefaultCredentials = true;
                        smtp2.Credentials = credential2;
                        smtp2.Send(gorevmail_toappointee2);
                    }
                    if (appointee3 != null)
                    {
                        MailMessage gorevmail_toappointee3 = new MailMessage(from, appointee3.Mail);
                        gorevmail_toappointee3.From = new MailAddress(from, "Mart Elektronik");
                        gorevmail_toappointee3.Subject = "Görev Hatırlatma";
                        gorevmail_toappointee3.Body = "Sayın " + task.Atanan3 + ", " + "\r\n" + "\r\n" + "'" + task.Atayan + "'" + " tarafından atanan '" + task.Gorev + "' görevinizin Bitiş Tarihi bugündür.";

                        SmtpClient smtp3 = new SmtpClient();
                        smtp3.Host = "mail.martelektronik.com";
                        smtp3.Port = 587;
                        smtp3.EnableSsl = false;

                        NetworkCredential credential3 = new NetworkCredential("bildirim@martelektronik.com", "MartBildir24!!");
                        smtp3.UseDefaultCredentials = true;
                        smtp3.Credentials = credential3;
                        smtp3.Send(gorevmail_toappointee3);
                    }

                    MailMessage gorevmail_toappointed = new MailMessage(from, appointed.Mail);
                    gorevmail_toappointed.From = new MailAddress(from, "Mart Elektronik");
                    gorevmail_toappointed.Subject = "Görev Hatırlatma";
                    gorevmail_toappointed.Body = "Sayın " + task.Atayan + ", " + "\r\n" + "\r\n" + "'" + task.Gorev + "' görevinin Bitiş Tarihi bugündür.";

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "mail.martelektronik.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = false;

                    NetworkCredential credential = new NetworkCredential("bildirim@martelektronik.com", "MartBildir24!!");
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = credential;
                    smtp.Send(gorevmail_toappointed);
                }

                foreach (var task in task_expired)
                {
                    var from = "bildirim@martelektronik.com";
                    var appointee1 = db.Kullanici.FirstOrDefault(x => x.KullaniciAdi == task.Atanan1ID);
                    var appointee2 = db.Kullanici.FirstOrDefault(x => x.KullaniciAdi == task.Atanan2ID);
                    var appointee3 = db.Kullanici.FirstOrDefault(x => x.KullaniciAdi == task.Atanan3ID);
                    var appointed = db.Kullanici.FirstOrDefault(x => x.KullaniciAdi == task.AtayanID);

                    var gunfarki = (Convert.ToDateTime(task.Bitis_Tarihi) - Convert.ToDateTime(System.DateTime.Today)).Days;

                    if (appointee1 != null)
                    {
                        MailMessage gorevmail_toappointee1 = new MailMessage(from, appointee1.Mail);
                        gorevmail_toappointee1.From = new MailAddress(from, "Mart Elektronik");
                        gorevmail_toappointee1.Subject = "Görev Hatırlatma";
                        gorevmail_toappointee1.Body = "Sayın " + task.Atanan1 + ", " + "\r\n" + "\r\n" + "'" + task.Atayan + "'" + " tarafından atanan '" + task.Gorev + "' görevinizin Bitiş Tarihi üzerinden " + gunfarki + " gün geçmiştir";

                        SmtpClient smtp1 = new SmtpClient();
                        smtp1.Host = "mail.martelektronik.com";
                        smtp1.Port = 587;
                        smtp1.EnableSsl = false;

                        NetworkCredential credential1 = new NetworkCredential("bildirim@martelektronik.com", "MartBildir24!!");
                        smtp1.UseDefaultCredentials = true;
                        smtp1.Credentials = credential1;
                        smtp1.Send(gorevmail_toappointee1);
                    }
                    if (appointee2 != null)
                    {
                        MailMessage gorevmail_toappointee2 = new MailMessage(from, appointee2.Mail);
                        gorevmail_toappointee2.From = new MailAddress(from, "Mart Elektronik");
                        gorevmail_toappointee2.Subject = "Görev Hatırlatma";
                        gorevmail_toappointee2.Body = "Sayın " + task.Atanan2 + ", " + "\r\n" + "\r\n" + "'" + task.Atayan + "'" + " tarafından atanan '" + task.Gorev + "' görevinizin Bitiş Tarihi üzerinden " + gunfarki + " gün geçmiştir";

                        SmtpClient smtp2 = new SmtpClient();
                        smtp2.Host = "mail.martelektronik.com";
                        smtp2.Port = 587;
                        smtp2.EnableSsl = false;

                        NetworkCredential credential2 = new NetworkCredential("bildirim@martelektronik.com", "MartBildir24!!");
                        smtp2.UseDefaultCredentials = true;
                        smtp2.Credentials = credential2;
                        smtp2.Send(gorevmail_toappointee2);
                    }
                    if (appointee3 != null)
                    {
                        MailMessage gorevmail_toappointee3 = new MailMessage(from, appointee3.Mail);
                        gorevmail_toappointee3.From = new MailAddress(from, "Mart Elektronik");
                        gorevmail_toappointee3.Subject = "Görev Hatırlatma";
                        gorevmail_toappointee3.Body = "Sayın " + task.Atanan3 + ", " + "\r\n" + "\r\n" + "'" + task.Atayan + "'" + " tarafından atanan '" + task.Gorev + "' görevinizin Bitiş Tarihi üzerinden " + gunfarki + " gün geçmiştir";

                        SmtpClient smtp3 = new SmtpClient();
                        smtp3.Host = "mail.martelektronik.com";
                        smtp3.Port = 587;
                        smtp3.EnableSsl = false;

                        NetworkCredential credential3 = new NetworkCredential("bildirim@martelektronik.com", "MartBildir24!!");
                        smtp3.UseDefaultCredentials = true;
                        smtp3.Credentials = credential3;
                        smtp3.Send(gorevmail_toappointee3);
                    }


                    MailMessage gorevmail_toappointed = new MailMessage(from, appointed.Mail);
                    gorevmail_toappointed.From = new MailAddress(from, "Mart Elektronik");
                    gorevmail_toappointed.Subject = "Görev Hatırlatma";
                    gorevmail_toappointed.Body = "Sayın " + task.Atayan + ", " + "\r\n" + "\r\n" + "'" + task.Gorev + "' görevin Bitiş Tarihi üzerinden " + gunfarki + " gün geçmiştir";

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "mail.martelektronik.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = false;

                    NetworkCredential credential = new NetworkCredential("bildirim@martelektronik.com", "MartBildir24!!");
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = credential;
                    smtp.Send(gorevmail_toappointed);
                }

                //foreach (var task in task_last3days)
                //{
                //    var from = "bildirim@martelektronik.com";
                //    var appointee1 = db.Kullanici.FirstOrDefault(x => x.KullaniciAdi == task.Atanan1ID);
                //    var appointee2 = db.Kullanici.FirstOrDefault(x => x.KullaniciAdi == task.Atanan2ID);
                //    var appointee3 = db.Kullanici.FirstOrDefault(x => x.KullaniciAdi == task.Atanan3ID);
                //    var appointed = db.Kullanici.FirstOrDefault(x => x.KullaniciAdi == task.AtayanID);

                //    var gunfarki = (Convert.ToDateTime(task.Bitis_Tarihi) - Convert.ToDateTime(System.DateTime.Today)).Days;

                //    if (appointee1 != null)
                //    {
                //        MailMessage gorevmail_toappointee1 = new MailMessage(from, appointee1.Mail);
                //        gorevmail_toappointee1.From = new MailAddress(from, "Mart Elektronik");
                //        gorevmail_toappointee1.Subject = "Görev Hatırlatma";
                //        gorevmail_toappointee1.Body = "Sayın " + task.Atanan1 + ", " + "\r\n" + "\r\n" + "'" + task.Atayan + "'" + " tarafından atanan '" + task.Gorev + "' görevinizin Bitiş Tarihi'ne " + gunfarki + " gün kalmıştır";

                //        SmtpClient smtp1 = new SmtpClient();
                //        smtp1.Host = "mail.martelektronik.com";
                //        smtp1.Port = 587;
                //        smtp1.EnableSsl = false;

                //        NetworkCredential credential1 = new NetworkCredential("bildirim@martelektronik.com", "MartBildir24!!");
                //        smtp1.UseDefaultCredentials = true;
                //        smtp1.Credentials = credential1;
                //        smtp1.Send(gorevmail_toappointee1);
                //    }
                //    if (appointee2 != null)
                //    {
                //        MailMessage gorevmail_toappointee2 = new MailMessage(from, appointee2.Mail);
                //        gorevmail_toappointee2.From = new MailAddress(from, "Mart Elektronik");
                //        gorevmail_toappointee2.Subject = "Görev Hatırlatma";
                //        gorevmail_toappointee2.Body = "Sayın " + task.Atanan2 + ", " + "\r\n" + "\r\n" + "'" + task.Atayan + "'" + " tarafından atanan '" + task.Gorev + "' görevinizin Bitiş Tarihi'ne " + gunfarki + " gün kalmıştır";

                //        SmtpClient smtp2 = new SmtpClient();
                //        smtp2.Host = "mail.martelektronik.com";
                //        smtp2.Port = 587;
                //        smtp2.EnableSsl = false;

                //        NetworkCredential credential2 = new NetworkCredential("bildirim@martelektronik.com", "MartBildir24!!");
                //        smtp2.UseDefaultCredentials = true;
                //        smtp2.Credentials = credential2;
                //        smtp2.Send(gorevmail_toappointee2);
                //    }
                //    if (appointee3 != null)
                //    {
                //        MailMessage gorevmail_toappointee3 = new MailMessage(from, appointee3.Mail);
                //        gorevmail_toappointee3.From = new MailAddress(from, "Mart Elektronik");
                //        gorevmail_toappointee3.Subject = "Görev Hatırlatma";
                //        gorevmail_toappointee3.Body = "Sayın " + task.Atanan3 + ", " + "\r\n" + "\r\n" + "'" + task.Atayan + "'" + " tarafından atanan '" + task.Gorev + "' görevinizin Bitiş Tarihi'ne " + gunfarki + " gün kalmıştır";

                //        SmtpClient smtp3 = new SmtpClient();
                //        smtp3.Host = "mail.martelektronik.com";
                //        smtp3.Port = 587;
                //        smtp3.EnableSsl = false;

                //        NetworkCredential credential3 = new NetworkCredential("bildirim@martelektronik.com", "MartBildir24!!");
                //        smtp3.UseDefaultCredentials = true;
                //        smtp3.Credentials = credential3;
                //        smtp3.Send(gorevmail_toappointee3);
                //    }


                //    MailMessage gorevmail_toappointed = new MailMessage(from, appointed.Mail);
                //    gorevmail_toappointed.From = new MailAddress(from, "Mart Elektronik");
                //    gorevmail_toappointed.Subject = "Görev Hatırlatma";
                //    gorevmail_toappointed.Body = "Sayın " + task.Atayan + ", " + "\r\n" + "\r\n" + "'" + task.Gorev + "' görevin Bitiş Tarihi'ne " + gunfarki + " gün kalmıştır";

                //    SmtpClient smtp = new SmtpClient();
                //    smtp.Host = "mail.martelektronik.com";
                //    smtp.Port = 587;
                //    smtp.EnableSsl = false;

                //    NetworkCredential credential = new NetworkCredential("bildirim@martelektronik.com", "MartBildir24!!");
                //    smtp.UseDefaultCredentials = true;
                //    smtp.Credentials = credential;
                //    smtp.Send(gorevmail_toappointed);
                //}
            }
            catch
            {
                ViewBag.MailErrorMsg = "Mail Gönderilemedi";
            }
            return View();
        }

        [Authorize(Roles = "7")]
        public ActionResult MusteriSiparisIndex(int sayfa = 1)
        {
            if (Session["MusteriKullaniciAdi"] != null)
            {
                //Güncel Siparişlerle ilgili verilerin döndürüldüğü kısım
                List<GuncelSiparisler> guncels = new List<GuncelSiparisler>();
                string sqlconnstr = ConfigurationManager.ConnectionStrings["Context"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(sqlconnstr);
                string gunceldb = "SELECT GuncelSiparislers.UID, GuncelSiparislers.sip_stok_kod, GuncelSiparislers.sip_musteri_kod, GuncelSiparislers.sto_isim, GuncelSiparislers.sip_teslim_tarih, GuncelSiparislers.sip_evrakno_seri," +
                                  "GuncelSiparislers.sip_evrakno_sira, GuncelSiparislers.sip_belge_no, GuncelSiparislers.sip_miktar, GuncelSiparislers.sip_teslim_miktar, GuncelSiparislers.Teslimden_Kalan, " +
                                  "GuncelSiparislers_NotDurum.Durum FROM GuncelSiparislers INNER JOIN GuncelSiparislers_NotDurum ON GuncelSiparislers.UID = GuncelSiparislers_NotDurum.UID";
                SqlCommand sqlcmd = new SqlCommand(gunceldb, sqlcon);
                SqlDataAdapter sda = new SqlDataAdapter(sqlcmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    GuncelSiparisler gs = new GuncelSiparisler();
                    gs.sip_musteri_kod = dr["sip_musteri_kod"].ToString();
                    gs.UID = dr["UID"].ToString();
                    gs.sip_teslim_tarih = Convert.ToDateTime(dr["sip_teslim_tarih"]);
                    gs.sip_stok_kod = dr["sip_stok_kod"].ToString();
                    gs.sto_isim = dr["sto_isim"].ToString();
                    gs.sip_miktar = Convert.ToInt16(dr["sip_miktar"]);
                    gs.sip_teslim_miktar = Convert.ToInt16(dr["sip_teslim_miktar"]);
                    gs.Teslimden_Kalan = Convert.ToInt16(dr["Teslimden_Kalan"]);
                    gs.sip_evrakno_seri = dr["sip_evrakno_seri"].ToString();
                    gs.sip_evrakno_sira = Convert.ToInt32(dr["sip_evrakno_sira"]);
                    gs.sip_belge_no = dr["sip_belge_no"].ToString();
                    gs.Durum = dr["Durum"].ToString();
                    if (gs.Durum != "")
                    {
                        gs.Durum = gs.Durum;
                    }
                    else
                    {
                        gs.Durum = "İşleme Alındı";
                    }
                    guncels.Add(gs);
                }

                var musteri = Session["MusteriUnvan"] as string;
                var musterikod = Session["MusteriKullaniciAdi"] as string;
                ViewBag.MusteriPageTitle = "Sipariş Takip Sistemi";
                ViewBag.Title = "Siparişlerim";
                ViewBag.Musteri = musteri;
                ViewBag.Page = "Siparişlerim";
                var musterisipariscount = guncels.Where(x => x.sip_musteri_kod == musterikod).Count();
                ViewBag.MusteriSiparisCount = musterisipariscount;
                var musterisiparis = guncels.Where(x => x.sip_musteri_kod == musterikod).OrderBy(x => x.sip_teslim_tarih).ToPagedList(sayfa, 20);
                return View(musterisiparis);
            }
            else
            {
                return RedirectToAction("KullaniciGirisi", "Giris");
            }
        }

        public ActionResult TumSiparislerExcel()
        {
            var bugun = System.DateTime.Now.ToString("dd-MM-yyyy");
            var guncelsiparisler_count = db.GuncelSiparislers.Count();
            using (var workbook = new XLWorkbook())
            {
                var guncelsiparisler = db.GuncelSiparislers.ToList();
                var ws = workbook.Worksheets.Add("Açık Siparişler");
                ws.Cell(1, 1).Value = "Stok Kod";
                ws.Cell(1, 2).Value = "Ürün İsmi";
                ws.Cell(1, 3).Value = "Teslim Tarihi";
                ws.Cell(1, 4).Value = "Seri - Sıra No";
                ws.Cell(1, 5).Value = "Belge No";
                ws.Cell(1, 6).Value = "Müşteri Kod";
                ws.Cell(1, 7).Value = "Cari Unvan";
                ws.Cell(1, 8).Value = "Sipariş Miktar";
                ws.Cell(1, 9).Value = "Sipariş Teslim Miktar";
                ws.Cell(1, 10).Value = "Teslimden Kalan";
                ws.Cell(1, 11).Value = "Sipariş Cinsi";
                ws.Cell(1, 12).Value = "UID";
                int row = 2;
                foreach (var item in guncelsiparisler)
                {
                    ws.Cell(row, 1).Value = item.sip_stok_kod;
                    ws.Cell(row, 2).Value = item.sto_isim;
                    ws.Cell(row, 3).Value = item.sip_teslim_tarih;
                    ws.Cell(row, 4).Value = item.sip_evrakno_seri + item.sip_evrakno_sira;
                    ws.Cell(row, 5).Value = item.sip_belge_no;
                    ws.Cell(row, 6).Value = item.sip_musteri_kod;
                    ws.Cell(row, 7).Value = item.cari_unvan;
                    ws.Cell(row, 8).Value = item.sip_miktar;
                    ws.Cell(row, 9).Value = item.sip_teslim_miktar;
                    ws.Cell(row, 10).Value = item.Teslimden_Kalan;
                    ws.Cell(row, 11).Value = item.sip_cins;
                    ws.Cell(row, 12).Value = item.UID;
                    row++;
                }
                ws.Cells("A1:L1").Style.Font.Bold = true;

                ws.Column(1).Width = 15; // 1. sütun
                ws.Column(2).Width = 70; // 2. sütun
                ws.Column(3).Width = 15; // 3. sütun
                ws.Column(4).Width = 15; // 4. sütun
                ws.Column(5).Width = 15; // 5. sütun
                ws.Column(6).Width = 20; // 6. sütun
                ws.Column(7).Width = 15; // 7. sütun
                ws.Column(8).Width = 25; // 8. sütun
                ws.Column(9).Width = 25; // 8. sütun
                ws.Column(10).Width = 25; // 8. sütun
                ws.Column(11).Width = 25; // 8. sütun
                ws.Column(12).Width = 25; // 8. sütun
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Açık Siparişler " + " " + bugun + ".xlsx");
                }
            }
        }

        public ActionResult MusteriSiparisleriExcel()
        {
            var bugun = System.DateTime.Now.ToString("dd-MM-yyyy");
            var musteriID = Session["MusteriKullaniciAdi"] as string;
            var musteri_siparisler = db.GuncelSiparislers.First(x => x.sip_musteri_kod == musteriID);
            var musteri_siparisler_count = db.GuncelSiparislers.Where(x => x.sip_musteri_kod == musteriID).Count();
            using (var workbook = new XLWorkbook())
            {
                var musteri_siparisleri = db.GuncelSiparislers.Where(x => x.sip_musteri_kod == musteriID).ToList();
                var ws = workbook.Worksheets.Add("Siparişlerim");
                ws.Cell(1, 1).Value = "Seri - Sıra No";
                ws.Cell(1, 2).Value = "Sipariş No";
                ws.Cell(1, 3).Value = "Stok No";
                ws.Cell(1, 4).Value = "Ürün İsmi";
                ws.Cell(1, 5).Value = "Sipariş Miktarı";
                ws.Cell(1, 6).Value = "Teslim Edilen Miktar";
                ws.Cell(1, 7).Value = "Kalan Miktar";
                ws.Cell(1, 8).Value = "Planlanan Teslim Tarihi";
                int row = 2;
                foreach (var item in musteri_siparisleri)
                {
                    ws.Cell(row, 1).Value = item.sip_evrakno_seri + item.sip_evrakno_sira;
                    ws.Cell(row, 2).Value = item.sip_belge_no;
                    ws.Cell(row, 3).Value = item.sip_stok_kod;
                    ws.Cell(row, 4).Value = item.sto_isim;
                    ws.Cell(row, 5).Value = item.sip_miktar;
                    ws.Cell(row, 6).Value = item.sip_teslim_miktar;
                    ws.Cell(row, 7).Value = item.Teslimden_Kalan;
                    ws.Cell(row, 8).Value = item.sip_teslim_tarih;
                    row++;
                }
                ws.Cells("A1:H1").Style.Font.Bold = true;

                ws.Column(1).Width = 15; // 1. sütun
                ws.Column(2).Width = 15; // 2. sütun
                ws.Column(3).Width = 15; // 3. sütun
                ws.Column(4).Width = 70; // 4. sütun
                ws.Column(5).Width = 15; // 5. sütun
                ws.Column(6).Width = 20; // 6. sütun
                ws.Column(7).Width = 15; // 7. sütun
                ws.Column(8).Width = 25; // 8. sütun
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Siparişler " + musteri_siparisler.cari_unvan.ToUpper() + " " + bugun + ".xlsx");
                }
            }
        }

        public ActionResult MusteriSiparisleriExcelEng()
        {
            var bugun = System.DateTime.Now.ToString("dd-MM-yyyy");
            var musteriID = Session["MusteriKullaniciAdi"] as string;
            var musteri_siparisler = db.GuncelSiparislers.First(x => x.sip_musteri_kod == musteriID);
            var musteri_siparisler_count = db.GuncelSiparislers.Where(x => x.sip_musteri_kod == musteriID).Count();
            using (var workbook = new XLWorkbook())
            {
                var musteri_siparisleri = db.GuncelSiparislers.Where(x => x.sip_musteri_kod == musteriID).ToList();
                var ws = workbook.Worksheets.Add("My Orders");
                ws.Cell(1, 1).Value = "Document No";
                ws.Cell(1, 2).Value = "Order No";
                ws.Cell(1, 3).Value = "Product ID";
                ws.Cell(1, 4).Value = "Product Name";
                ws.Cell(1, 5).Value = "Order Quantity";
                ws.Cell(1, 6).Value = "Deliveried Quantity";
                ws.Cell(1, 7).Value = "Open Quantity";
                ws.Cell(1, 8).Value = "Scheduled Delivery Date";
                int row = 2;
                foreach (var item in musteri_siparisleri)
                {
                    ws.Cell(row, 1).Value = item.sip_evrakno_seri + item.sip_evrakno_sira;
                    ws.Cell(row, 2).Value = item.sip_belge_no;
                    ws.Cell(row, 3).Value = item.sip_stok_kod;
                    ws.Cell(row, 4).Value = item.sto_isim;
                    ws.Cell(row, 5).Value = item.sip_miktar;
                    ws.Cell(row, 6).Value = item.sip_teslim_miktar;
                    ws.Cell(row, 7).Value = item.Teslimden_Kalan;
                    ws.Cell(row, 8).Value = item.sip_teslim_tarih;
                    row++;
                }
                ws.Cells("A1:H1").Style.Font.Bold = true;

                ws.Column(1).Width = 15; // 1. sütun
                ws.Column(2).Width = 15; // 2. sütun
                ws.Column(3).Width = 15; // 3. sütun
                ws.Column(4).Width = 70; // 4. sütun
                ws.Column(5).Width = 15; // 5. sütun
                ws.Column(6).Width = 20; // 6. sütun
                ws.Column(7).Width = 15; // 7. sütun
                ws.Column(8).Width = 25; // 8. sütun
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Orders " + musteri_siparisler.cari_unvan.ToUpper() + " " + bugun + ".xlsx");
                }
            }
        }

        [Authorize(Roles = "7")]
        public ActionResult MusteriSiparisIndexEng(int sayfa = 1)
        {
            if (Session["MusteriKullaniciAdi"] != null)
            {
                //Güncel Siparişlerle ilgili verilerin döndürüldüğü kısım
                List<GuncelSiparisler> guncels = new List<GuncelSiparisler>();
                string sqlconnstr = ConfigurationManager.ConnectionStrings["Context"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(sqlconnstr);
                string gunceldb = "SELECT GuncelSiparislers.UID, GuncelSiparislers.sip_stok_kod, GuncelSiparislers.sip_musteri_kod, GuncelSiparislers.sto_isim, GuncelSiparislers.sip_teslim_tarih, GuncelSiparislers.sip_evrakno_seri," +
                                  "GuncelSiparislers.sip_evrakno_sira, GuncelSiparislers.sip_belge_no, GuncelSiparislers.sip_miktar, GuncelSiparislers.sip_teslim_miktar, GuncelSiparislers.Teslimden_Kalan, " +
                                  "GuncelSiparislers_NotDurum.Durum FROM GuncelSiparislers INNER JOIN GuncelSiparislers_NotDurum ON GuncelSiparislers.UID = GuncelSiparislers_NotDurum.UID";
                SqlCommand sqlcmd = new SqlCommand(gunceldb, sqlcon);
                SqlDataAdapter sda = new SqlDataAdapter(sqlcmd);  
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    GuncelSiparisler gs = new GuncelSiparisler();
                    gs.sip_musteri_kod = dr["sip_musteri_kod"].ToString();
                    gs.UID = dr["UID"].ToString();
                    gs.sip_teslim_tarih = Convert.ToDateTime(dr["sip_teslim_tarih"]);
                    gs.sip_stok_kod = dr["sip_stok_kod"].ToString();
                    gs.sto_isim = dr["sto_isim"].ToString();
                    gs.sip_miktar = Convert.ToInt16(dr["sip_miktar"]);
                    gs.sip_teslim_miktar = Convert.ToInt16(dr["sip_teslim_miktar"]);
                    gs.Teslimden_Kalan = Convert.ToInt16(dr["Teslimden_Kalan"]);
                    gs.sip_evrakno_seri = dr["sip_evrakno_seri"].ToString();
                    gs.sip_evrakno_sira = Convert.ToInt32(dr["sip_evrakno_sira"]);
                    gs.sip_belge_no = dr["sip_belge_no"].ToString();
                    gs.Durum = dr["Durum"].ToString();
                    if (gs.Durum != "")
                    {
                        gs.Durum = gs.Durum;
                    }
                    else
                    {
                        gs.Durum = "İşleme Alındı";
                    }
                    guncels.Add(gs);
                }

                var musteri = Session["MusteriUnvan"] as string;
                var musterikod = Session["MusteriKullaniciAdi"] as string;
                ViewBag.MusteriPageTitle = "Order Track System";
                ViewBag.Title = " My Orders";
                ViewBag.Musteri = musteri;
                ViewBag.Page = " My Orders";
                var musterisipariscount = guncels.Where(x => x.sip_musteri_kod == musterikod).Count();
                ViewBag.MusteriSiparisCount = musterisipariscount;
                var musterisiparis = guncels.Where(x => x.sip_musteri_kod == musterikod).OrderBy(x => x.sip_teslim_tarih).ToPagedList(sayfa, 20);
                return View(musterisiparis);
            }
            else
            {
                return RedirectToAction("KullaniciGirisi", "Giris");
            }
        }

        [Authorize(Roles = "1,2")]
        public ActionResult AskiyaAlinanSiparis(GuncelSiparisler gs, int sayfa = 1)
        {
            if (Session["KullaniciAdi"] as string != null)
            {

                var tbl_guncelserver = db.GuncelSiparislers.ToList();
                var tbl_guncelnot = db.GuncelSiparislersNotDurum.ToList();

                var serino = tbl_guncelserver.Select(x => x.sip_evrakno_seri);
                var sirano = tbl_guncelserver.Select(x => x.sip_evrakno_sira);

                var eksikUID = tbl_guncelserver.Select(x => x.UID).Except(tbl_guncelnot.Select(x => x.UID));
                var fazlaUID = tbl_guncelnot.Select(x => x.UID).Except(tbl_guncelserver.Select(x => x.UID));

                foreach (var uid in eksikUID)
                {
                    db.GuncelSiparislersNotDurum.Add(new GuncelSiparislers_NotDurum { UID = uid });
                    db.SaveChanges();
                }

                foreach (var uid in fazlaUID)
                {
                    var silinecek = db.GuncelSiparislersNotDurum.Find(uid);
                    db.GuncelSiparislersNotDurum.Remove(silinecek);
                    db.SaveChanges();
                }

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
                    gs = new GuncelSiparisler();
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
                Dondur();
                return View(guncels.Where(x => x.Durum == "Askıya Alındı").OrderBy(x => x.sip_teslim_tarih).ToPagedList(sayfa, 50));
            }
            else
            {
                return RedirectToAction("KullaniciGirisi", "Giris");
            }
        }

        public ActionResult CariAlfabetik(int sayfa = 1)
        {

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

            Dondur();
            return View("Index", guncels.OrderBy(x => x.cari_unvan).ToPagedList(sayfa, 50));
        }

        public ActionResult CariTersAlfabetik(int sayfa = 1)
        {

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

            Dondur();
            return View("Index", guncels.OrderByDescending(x => x.cari_unvan).ToPagedList(sayfa, 50));
        }

        public ActionResult UrunAlfabetik(int sayfa = 1)
        {

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

            Dondur();
            return View("Index", guncels.OrderBy(x => x.sto_isim).ToPagedList(sayfa, 50));
        }

        public ActionResult UrunTersAlfabetik(int sayfa = 1)
        {

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

            Dondur();
            return View("Index", guncels.OrderByDescending(x => x.sto_isim).ToPagedList(sayfa, 50));
        }

        static int order;

        public string FileVirtualPath { get; private set; }

        public ActionResult TersSiralama(int sayfa = 1)
        {
            Dondur();
            if (order == 0)
            {
                order = 1;

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
                return View("Index", guncels.Where(x => x.Durum != "Askıya Alındı").OrderByDescending(x => x.sip_teslim_tarih).ToPagedList(sayfa, 50));
            }
            else if (order == 1)
            {
                order = 0;
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
                return View("Index", guncels.Where(x => x.Durum != "Askıya Alındı").OrderBy(x => x.sip_teslim_tarih).ToPagedList(sayfa, 50));
            }
            else
            {
                return View("Index");
            }
        }

        [Authorize(Roles = "1,2")]
        public ActionResult SiparisGetir(string id)
        {
            if (Session["KullaniciAdi"] as string != null)
            {
                Dondur();
                var siparis = db.GuncelSiparislers.Find(id);
                var siparis2 = db.GuncelSiparislersNotDurum.Find(siparis.UID);
                ViewBag.DurumNow = siparis2.Durum;
                ViewBag.UrunIsim = siparis.sto_isim;
                siparis.Not = siparis2.Not;
                ViewBag.Kullanici = siparis2.Kullanici;
                return View("SiparisGetir", siparis);
            }
            else
            {
                return RedirectToAction("KullaniciGirisi", "Giris");
            }
        }

        [Authorize(Roles = "1,2")]
        public ActionResult SiparisDuzenle(GuncelSiparislers_NotDurum gsnd, FormCollection form, GuncelSiparisler gs, GecmisSiparisHareket gsh)
        {
            var admin = Session["Personel"] as string;
            var siparisddl = form["SiparisDurumDDL"].ToString();
            var siparis = db.GuncelSiparislers.Find(gs.UID);
            var siparis2 = db.GuncelSiparislersNotDurum.Find(siparis.UID);
            if (siparisddl == "Kısmi Paketlendi" && gsnd.Not != null)
            {
                siparis2.Durum = siparisddl;
                if (gsnd.Not != null)
                {
                    siparis2.Not = gsnd.Not.ToString();

                }
                else
                {
                    siparis2.Not = null;
                }
                siparis2.Tarih = System.DateTime.Now.ToString("dd/MM/yyyy:HH:mm");
                siparis2.Kullanici = admin;
                db.GecmisSiparisHarekets.Add(gsh);
                gsh.UID = gsnd.UID;
                gsh.Durum = siparisddl;
                if (gsnd.Not != null)
                {
                    gsh.Not = gsnd.Not.ToString();

                }
                else
                {
                    gsh.Not = null;
                }
                gsh.sip_stok_kod = siparis.sip_stok_kod;
                gsh.sto_isim = siparis.sto_isim;
                gsh.sip_teslim_tarih = siparis.sip_teslim_tarih;
                gsh.sip_evrakno_seri = siparis.sip_evrakno_seri;
                gsh.sip_evrakno_sira = siparis.sip_evrakno_sira;
                gsh.sip_belge_no = siparis.sip_belge_no;
                gsh.sip_musteri_kod = siparis.sip_musteri_kod;
                gsh.cari_unvan = siparis.cari_unvan;
                gsh.sip_miktar = siparis.sip_miktar;
                gsh.sip_teslim_miktar = siparis.sip_teslim_miktar;
                gsh.Teslimden_Kalan = siparis.Teslimden_Kalan;
                gsh.sip_cins = siparis.sip_cins;
                gsh.Tarih = System.DateTime.Now.ToString("dd/MM/yyyy:HH:mm");
                gsh.Kullanici = admin;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else if (siparisddl != "Kısmi Paketlendi")
            {
                if (siparisddl == "Seçiniz")
                {
                    siparis2.Durum = null;
                    if (gsnd.Not != null)
                    {
                        siparis2.Not = gsnd.Not.ToString();
                    }
                    else
                    {
                        siparis2.Not = null;
                    }
                    siparis2.Tarih = System.DateTime.Now.ToString("dd/MM/yyyy:HH:mm");
                    siparis2.Kullanici = admin;
                    db.GecmisSiparisHarekets.Add(gsh);
                    gsh.UID = gsnd.UID;
                    gsh.Durum = siparisddl;
                    gsh.Not = gsnd.Not;
                    gsh.sip_stok_kod = siparis.sip_stok_kod;
                    gsh.sto_isim = siparis.sto_isim;
                    gsh.sip_teslim_tarih = siparis.sip_teslim_tarih;
                    gsh.sip_evrakno_seri = siparis.sip_evrakno_seri;
                    gsh.sip_evrakno_sira = siparis.sip_evrakno_sira;
                    gsh.sip_belge_no = siparis.sip_belge_no;
                    gsh.sip_musteri_kod = siparis.sip_musteri_kod;
                    gsh.cari_unvan = siparis.cari_unvan;
                    gsh.sip_miktar = siparis.sip_miktar;
                    gsh.sip_teslim_miktar = siparis.sip_teslim_miktar;
                    gsh.Teslimden_Kalan = siparis.Teslimden_Kalan;
                    gsh.sip_cins = siparis.sip_cins;
                    gsh.Tarih = System.DateTime.Now.ToString("dd/MM/yyyy:HH:mm");
                    gsh.Kullanici = admin;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    siparis2.Durum = siparisddl;
                    if (gsnd.Not != null)
                    {
                        siparis2.Not = gsnd.Not.ToString();
                    }
                    else
                    {
                        siparis2.Not = null;
                    }
                    siparis2.Tarih = System.DateTime.Now.ToString("dd/MM/yyyy:HH:mm");
                    siparis2.Kullanici = admin;
                    db.GecmisSiparisHarekets.Add(gsh);
                    gsh.UID = gsnd.UID;
                    gsh.Durum = siparisddl;
                    gsh.Not = gsnd.Not;
                    gsh.sip_stok_kod = siparis.sip_stok_kod;
                    gsh.sto_isim = siparis.sto_isim;
                    gsh.sip_teslim_tarih = siparis.sip_teslim_tarih;
                    gsh.sip_evrakno_seri = siparis.sip_evrakno_seri;
                    gsh.sip_evrakno_sira = siparis.sip_evrakno_sira;
                    gsh.sip_belge_no = siparis.sip_belge_no;
                    gsh.sip_musteri_kod = siparis.sip_musteri_kod;
                    gsh.cari_unvan = siparis.cari_unvan;
                    gsh.sip_miktar = siparis.sip_miktar;
                    gsh.sip_teslim_miktar = siparis.sip_teslim_miktar;
                    gsh.Teslimden_Kalan = siparis.Teslimden_Kalan;
                    gsh.sip_cins = siparis.sip_cins;
                    gsh.Tarih = System.DateTime.Now.ToString("dd/MM/yyyy:HH:mm");
                    gsh.Kullanici = admin;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                Dondur();
                ViewBag.Msg = "Sipariş Durumu Kısmi Paketlendi ise Not eklemelisiniz";
                ViewBag.UrunIsim = siparis.sto_isim;
                return View("SiparisGetir", siparis);
            }
        }

        [Authorize(Roles = "1,2")]
        public ActionResult SiparisFiltrele(string siparisfiltre, int sayfa = 1)
        {
            if (Session["KullaniciAdi"] as string != null)
            {
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
                    if (gs.Durum == "")
                    {
                        gs.Durum = "İşleme Alındı";
                    }
                    else
                    {
                        gs.Durum = gs.Durum;
                    }
                    guncels.Add(gs);
                }
                Dondur();
                return View("Index", guncels.Where(x => x.Durum != "Askıya Alındı" && (x.cari_unvan.ToLower().Contains(siparisfiltre) || x.cari_unvan.ToUpper().Contains(siparisfiltre) ||
                                          x.Durum.ToLower().Contains(siparisfiltre) || x.Durum.ToUpper().Contains(siparisfiltre) ||
                                          x.UID.Equals(siparisfiltre) ||
                                          x.sip_evrakno_seri.Contains(siparisfiltre) ||
                                          x.sip_belge_no.Contains(siparisfiltre) || x.sip_stok_kod.Contains(siparisfiltre) ||
                                          x.sto_isim.ToLower().Contains(siparisfiltre) || x.sto_isim.ToUpper().Contains(siparisfiltre) ||
                                          x.Not.ToLower().Contains(siparisfiltre) || x.Not.ToUpper().Contains(siparisfiltre))).OrderBy(x => x.sip_teslim_tarih).ToPagedList(sayfa, 50));
            }
            else
            {
                return RedirectToAction("KullaniciGirisi", "Giris");
            }
        }

        [Authorize(Roles = "1,2")]
        public ActionResult SiparisFiltreleAskiyaAlinan(string siparisfiltre, int sayfa = 1)
        {
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

            Dondur();
            return View("AskiyaAlinanSiparis", guncels.Where(x => x.Durum == "Askıya Alındı" && (x.cari_unvan.ToLower().Contains(siparisfiltre) || x.cari_unvan.ToUpper().Contains(siparisfiltre) ||
                                      x.UID.Equals(siparisfiltre) ||
                                      x.sip_evrakno_seri.Contains(siparisfiltre) ||
                                      x.sip_belge_no.Contains(siparisfiltre) || x.sip_stok_kod.Contains(siparisfiltre) ||
                                      x.sto_isim.ToLower().Contains(siparisfiltre) || x.sto_isim.ToUpper().Contains(siparisfiltre) ||
                                      x.Not.ToLower().Contains(siparisfiltre) || x.Not.ToUpper().Contains(siparisfiltre))).OrderBy(x => x.sip_teslim_tarih).ToPagedList(sayfa, 50));
        }

        public ActionResult SiparisRecetesi(string id)
        {
            try
            {
                var siprec = db.Recetelers.First(x => x.rec_anakod == id);
                Dondur();
                var sipreclist = db.Recetelers.Where(x => x.rec_anakod == siprec.rec_anakod).ToList();
                ViewBag.SekmeBaslik = siprec.rec_anakod;
                ViewBag.ReceteBaslik = siprec.rec_anakod + " - " + siprec.sto_isim_mamul;
                ViewBag.RecAciklama = siprec.rec_aciklama;
                return View(sipreclist);
            }
            catch (Exception ex)
            {
                if (ex is System.InvalidOperationException || ex is System.NullReferenceException)
                {
                    Dondur();
                    return RedirectToAction("NullError");
                }
                else
                {
                    Dondur();
                    return RedirectToAction("NullError");
                }
            }
        }

        public ActionResult SiparisRecetesiTuketimKod(string id)
        {
            try
            {
                Dondur();
                ViewBag.KullaniciAdi = Session["KullaniciAdi"] as string;
                var siprectuk = db.Recetelers.First(x => x.rec_anakod == id);
                var siprectuklist = db.Recetelers.Where(x => x.rec_anakod == siprectuk.rec_anakod).ToList();
                ViewBag.SekmeBaslik = siprectuk.rec_anakod;
                ViewBag.ReceteBaslik = siprectuk.rec_anakod + " - " + siprectuk.sto_isim_mamul;
                ViewBag.RecAciklama = siprectuk.rec_aciklama;
                return View(siprectuklist);
            }
            catch (Exception ex)
            {
                if (ex is System.InvalidOperationException || ex is System.NullReferenceException)
                {
                    Dondur();
                    return RedirectToAction("NullError");
                }
                else
                {
                    Dondur();
                    return RedirectToAction("NullError");
                }
            }
        }

        [HttpGet]
        public ActionResult DosyaYukle(string id)
        {
            ViewBag.ID = id;
            return View();
        }

        [HttpPost]
        public ActionResult DosyaYukle(Dosyalar d, FormCollection form)
        {
            Dondur();
            var stokkodu = form["StokKodu"].ToString();
            if (Request.Files.Count > 0)
            {
                if (d.Dosya != null)
                {
                    db.Dosyalars.Add(d);
                    string filename = stokkodu + "_" + Path.GetFileName(Request.Files[0].FileName);
                    string extension = Path.GetExtension(Request.Files[0].FileName);
                    string path = "/Content/Dosyalar/" + filename + extension;
                    Request.Files[0].SaveAs(Server.MapPath(path));
                    d.sip_stok_kod = stokkodu;
                    d.Dosya = "/Content/Dosyalar/" + filename + extension;
                    d.EklenmeTarih = System.DateTime.Now.ToString("dd/MM/yyyy");
                    d.Ekleyen = Session["Personel"] as string;
                    db.SaveChanges();
                }
                else
                {
                    ViewBag.Msg = "Stok Kodu ve Dosya boş bırakılamaz";
                    return View();
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DosyaSec(string id)
        {
            try
            {
                ViewBag.ID = id;
                var dosya = db.Dosyalars.Where(x => x.sip_stok_kod == id).ToList();
                return View(dosya);
            }
            catch (Exception ex)
            {
                if (ex is InvalidOperationException || ex is NullReferenceException)
                {
                    return RedirectToAction("NullError");
                }
                else
                {
                    return RedirectToAction("NullError");
                }
            }
        }

        public ActionResult DosyaIndir(string id, string name)
        {
            try
            {
                var bul = db.Dosyalars.First(x => x.sip_stok_kod == id && x.Dosya == name);
                string path = Server.MapPath(bul.Dosya);
                string ext = Path.GetExtension(path);
                var contentType = MimeMapping.GetMimeMapping(path);
                if (ext == ".html")
                {
                    return File(path, contentType);
                }
                else
                {
                    return File(path, contentType, bul.Dosya.Substring(18));
                }
            }
            catch (Exception ex)
            {
                if (ex is InvalidOperationException || ex is NullReferenceException)
                {
                    return RedirectToAction("NullError");
                }
                else
                {
                    return RedirectToAction("NullError");
                }
            }
        }

        public ActionResult DosyaSil(int id)
        {
            var dosya = db.Dosyalars.Find(id);
            db.Dosyalars.Remove(dosya);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult PieChart()
        {
            Dondur();
            int tumkapaliisemri = db.IsEmirleri.Where(x => x.msg_S_0355 == "Kapanmış").Count();
            var zamanindanonce = db.IsEmirleri.Where(x => (x.msg_S_0355 == "Kapanmış") && (x.msg_S_0366 >= x.msg_S_0360)).ToList();
            var zamanindansonra = db.IsEmirleri.Where(x => (x.msg_S_0355 == "Kapanmış") && (x.msg_S_0366 < x.msg_S_0360)).ToList();

            float resultzamanindanonce = zamanindanonce.Count();
            float resultzamanindansonra = zamanindansonra.Count();

            float zamanindanonceperc = resultzamanindanonce / tumkapaliisemri * 100;
            float suresigecmisperc = resultzamanindansonra / tumkapaliisemri * 100;

            ViewBag.KapaliSuresiZamanindanOnce = zamanindanonceperc;
            ViewBag.KapaliSuresiGecmis = suresigecmisperc;

            var data = new List<float> { ViewBag.KapaliSuresiGecmis, ViewBag.KapaliSuresiZamanindanOnce };
            ViewBag.Data = data;

            return View(data);
        }

        public ActionResult PieChartFiltre(string piechartfiltre)
        {

            if (piechartfiltre == "Son 1 Ay")
            {
                ViewBag.PieChartSecim = piechartfiltre;
                Dondur();
                var tumkapaliisemri = db.IsEmirleri.Where(x => x.msg_S_0355 == "Kapanmış").ToList();
                var zamanindanonce = db.IsEmirleri.Where(x => (x.msg_S_0355 == "Kapanmış") && (x.msg_S_0366 >= x.msg_S_0360)).ToList();
                var zamanindansonra = db.IsEmirleri.Where(x => (x.msg_S_0355 == "Kapanmış") && (x.msg_S_0366 < x.msg_S_0360)).ToList();

                float tumkapaliisemri1ay = tumkapaliisemri.Where(x => x.msg_S_0360 <= System.DateTime.Now.AddMonths(-1)).Count();
                float resultzamanindanonce = zamanindanonce.Where(x => x.msg_S_0360 <= System.DateTime.Now.AddMonths(-1)).Count();
                float resultzamanindansonra = zamanindansonra.Where(x => x.msg_S_0360 <= System.DateTime.Now.AddMonths(-1)).Count();

                float zamanindanonceperc = resultzamanindanonce / tumkapaliisemri1ay * 100;
                float suresigecmisperc = resultzamanindansonra / tumkapaliisemri1ay * 100;

                ViewBag.KapaliSuresiZamanindanOnce = zamanindanonceperc;
                ViewBag.KapaliSuresiGecmis = suresigecmisperc;

                var data = new List<float> { ViewBag.KapaliSuresiGecmis, ViewBag.KapaliSuresiZamanindanOnce };
                ViewBag.Data = data;

                return View("PieChart", data);
            }
            else if (piechartfiltre == "Son 15 Gün")
            {
                ViewBag.PieChartSecim = piechartfiltre;
                Dondur();
                var tumkapaliisemri = db.IsEmirleri.Where(x => x.msg_S_0355 == "Kapanmış").ToList();
                var zamanindanonce = db.IsEmirleri.Where(x => (x.msg_S_0355 == "Kapanmış") && (x.msg_S_0366 >= x.msg_S_0360)).ToList();
                var zamanindansonra = db.IsEmirleri.Where(x => (x.msg_S_0355 == "Kapanmış") && (x.msg_S_0366 < x.msg_S_0360)).ToList();

                float tumkapaliisemri15gun = tumkapaliisemri.Where(x => x.msg_S_0360 <= System.DateTime.Now.AddDays(-15)).Count();
                float resultzamanindanonce = zamanindanonce.Where(x => x.msg_S_0360 <= System.DateTime.Now.AddDays(-15)).Count();
                float resultzamanindansonra = zamanindansonra.Where(x => x.msg_S_0360 <= System.DateTime.Now.AddDays(-15)).Count();

                float zamanindanonceperc = resultzamanindanonce / tumkapaliisemri15gun * 100;
                float suresigecmisperc = resultzamanindansonra / tumkapaliisemri15gun * 100;

                ViewBag.KapaliSuresiZamanindanOnce = zamanindanonceperc;
                ViewBag.KapaliSuresiGecmis = suresigecmisperc;

                var data = new List<float> { ViewBag.KapaliSuresiGecmis, ViewBag.KapaliSuresiZamanindanOnce };
                ViewBag.Data = data;

                return View("PieChart", data);
            }
            else
            {
                ViewBag.PieChartSecim = piechartfiltre;
                Dondur();
                int tumkapaliisemri = db.IsEmirleri.Where(x => x.msg_S_0355 == "Kapanmış").Count();
                var zamanindanonce = db.IsEmirleri.Where(x => (x.msg_S_0355 == "Kapanmış") && (x.msg_S_0366 >= x.msg_S_0360)).ToList();
                var zamanindansonra = db.IsEmirleri.Where(x => (x.msg_S_0355 == "Kapanmış") && (x.msg_S_0366 < x.msg_S_0360)).ToList();

                float resultzamanindanonce = zamanindanonce.Count();
                float resultzamanindansonra = zamanindansonra.Count();

                float zamanindanonceperc = resultzamanindanonce / tumkapaliisemri * 100;
                float suresigecmisperc = resultzamanindansonra / tumkapaliisemri * 100;

                ViewBag.KapaliSuresiZamanindanOnce = zamanindanonceperc;
                ViewBag.KapaliSuresiGecmis = suresigecmisperc;

                var data = new List<float> { ViewBag.KapaliSuresiGecmis, ViewBag.KapaliSuresiZamanindanOnce };
                ViewBag.Data = data;

                return View("PieChart", data);
            }
        }

        public ActionResult EquipmentNeedsList()
        {
            return View();
        }
    }
}