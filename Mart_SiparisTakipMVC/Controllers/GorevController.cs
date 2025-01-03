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

namespace Mart_SiparisTakipMVC.Controllers
{
    public class GorevController : Controller
    {
        // GET: Gorev

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

        // bildirim@martelektronik.com maili ile istenen maile konu ve açıklama yazılarak gönderilecek mail fonksiyonu
        public void MailGonder(string to, string subject, string body)
        {
            MailMessage newMail = new MailMessage("bildirim@martelektronik.com", to)
            {
                From = new MailAddress("bildirim@martelektronik.com", "Mart Elektronik"),
                Subject = subject,
                Body = body,
            };

            SmtpClient smtp = new SmtpClient()
            {
                Host = "mail.martelektronik.com",
                Port = 587,
                EnableSsl = false,
            };

            NetworkCredential credential = new NetworkCredential("bildirim@martelektronik.com", "MartBildir24!!");
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = credential;
            smtp.Send(newMail);
        }

        [Authorize(Roles = "1")]
        public ActionResult Index(int sayfa = 1)
        {
            if (Session["KullaniciAdi"] as string != null)
            {
                Dondur();
                var gorevler = db.Gorevler.Where(x => x.Durum == "Aktif").OrderBy(x => x.ID).ToPagedList(sayfa, 20);
                return View(gorevler);
            }
            else
            {
                return RedirectToAction("KullaniciGirisi", "Giris");
            }
        }

        [Authorize(Roles = "1,2,3,4,9")]
        [HttpGet]
        public ActionResult YeniGorev()
        {
            Dondur();
            List<SelectListItem> personel = (from p in db.Kullanici.Where(x => x.Gorev != "Müşteri" && x.Gorev != "SMD Viewer" && x.Gorev != "THT Viewer").ToList()
                                             select new SelectListItem
                                             {
                                                 Text = p.Personel,
                                                 Value = p.KullaniciAdi,
                                             }).ToList();
            ViewBag.PersonelDDl = personel;
            return View();
        }

        [Authorize(Roles = "1,2,3,4,9")]
        [HttpPost]
        public ActionResult YeniGorev(Gorevler g, FormCollection form)
        {
            var today = System.DateTime.Today;
            var logged_user = Session["Personel"] as string;
            var logged_userid = Session["KullaniciAdi"] as string;

            var atanan1 = form["AtananPersonel1"].ToString();
            if (atanan1 != "")
            {
                var personel1 = db.Kullanici.First(x => x.KullaniciAdi == atanan1);
                if (personel1.Mail != null)
                {
                    db.Gorevler.Add(g);
                    g.Durum = "Aktif";
                    g.EklenmeTarih = today.ToString("yyyy-MM-dd");
                    g.Atayan = logged_user;
                    g.AtayanID = logged_userid;
                    g.Atanan1ID = personel1.KullaniciAdi;
                    g.Atanan1 = personel1.Personel;
                    db.SaveChanges();
                    g.GorevDosyaID = Convert.ToString(g.ID);
                    db.SaveChanges();

                    var to1 = personel1.Mail;
                    var mail_subject1 = "Yeni Göreviniz Var!";
                    var mail_body1 = "Sayın " + g.Atanan1 + ", " + "\r\n" + "' " + g.Atayan + " '" + " tarafından atanan " + "\r\n" + " Görev: " + g.Gorev + ",  Başlama Tarihi: " + Convert.ToDateTime(g.Baslama_Tarihi).ToString("dd-MM-yyyy") +
                                               ",  Bitiş Tarihi: " + Convert.ToDateTime(g.Bitis_Tarihi).ToString("dd-MM-yyyy") + "\r\n" + "görevinizin detaylarına Görev Portalı'ndan erişebilirsiniz.";
                    MailGonder(to1, mail_subject1, mail_body1);

                    var atanan2 = form["AtananPersonel2"].ToString();
                    if (atanan2 != "")
                    {
                        var personel2 = db.Kullanici.First(x => x.KullaniciAdi == atanan2);
                        if (personel2.Mail != null)
                        {
                            g.Atanan2ID = personel2.KullaniciAdi;
                            g.Atanan2 = personel2.Personel;
                            db.SaveChanges();
                            var to2 = personel2.Mail;
                            var mail_subject2 = "Yeni Göreviniz Var!";
                            var mail_body2 = "Sayın " + g.Atanan2 + ", " + "\r\n" + "' " + g.Atayan + " '" + " tarafından atanan " + "\r\n" + " Görev: " + g.Gorev + ",  Başlama Tarihi: " + Convert.ToDateTime(g.Baslama_Tarihi).ToString("dd-MM-yyyy") +
                                               ",  Bitiş Tarihi: " + Convert.ToDateTime(g.Bitis_Tarihi).ToString("dd-MM-yyyy") + "\r\n" + "görevinizin detaylarına Görev Portalı'ndan erişebilirsiniz.";
                            MailGonder(to2, mail_subject2, mail_body2);

                            var atanan3 = form["AtananPersonel3"].ToString();
                            if (atanan3 != "")
                            {
                                var personel3 = db.Kullanici.First(x => x.KullaniciAdi == atanan3);
                                if (personel3.Mail != null)
                                {
                                    g.Atanan3ID = personel3.KullaniciAdi;
                                    g.Atanan3 = personel3.Personel;
                                    db.SaveChanges();
                                    var to3 = personel2.Mail;
                                    var mail_subject3 = "Yeni Göreviniz Var!";
                                    var mail_body3 = "Sayın " + g.Atanan3 + ", " + "\r\n" + "' " + g.Atayan + " '" + " tarafından atanan " + "\r\n" + " Görev: " + g.Gorev + ",  Başlama Tarihi: " + Convert.ToDateTime(g.Baslama_Tarihi).ToString("dd-MM-yyyy") +
                                               ",  Bitiş Tarihi: " + Convert.ToDateTime(g.Bitis_Tarihi).ToString("dd-MM-yyyy") + "\r\n" + "görevinizin detaylarına Görev Portalı'ndan erişebilirsiniz.";

                                    MailGonder(to3, mail_subject3, mail_body3);
                                    return RedirectToAction("Gorevlerim");
                                }
                                else
                                {
                                    ViewBag.NullMailMsg = "Atanan personel 3 ün mail bilgisi boş olamaz";
                                    List<SelectListItem> personel = (from p in db.Kullanici.Where(x => x.Gorev != "Müşteri" && x.Gorev != "SMD Viewer" && x.Gorev != "THT Viewer").ToList()
                                                                     select new SelectListItem
                                                                     {
                                                                         Text = p.Personel,
                                                                         Value = p.KullaniciAdi,
                                                                     }).ToList();
                                    ViewBag.PersonelDDl = personel;
                                    return View("YeniGorev");
                                }
                            }
                            else
                            {
                                return RedirectToAction("Gorevlerim");
                            }
                        }
                        else
                        {
                            ViewBag.NullMailMsg = "Atanan personel 2 nin mail bilgisi boş olamaz";
                            List<SelectListItem> personel = (from p in db.Kullanici.Where(x => x.Gorev != "Müşteri" && x.Gorev != "SMD Viewer" && x.Gorev != "THT Viewer").ToList()
                                                             select new SelectListItem
                                                             {
                                                                 Text = p.Personel,
                                                                 Value = p.KullaniciAdi,
                                                             }).ToList();
                            ViewBag.PersonelDDl = personel;
                            return View("YeniGorev");
                        }
                    }
                    else
                    {
                        return RedirectToAction("Gorevlerim");
                    }
                }
                else
                {
                    ViewBag.NullMailMsg = "Atanan personel 1 in mail bilgisi boş olamaz";
                    List<SelectListItem> personel = (from p in db.Kullanici.Where(x => x.Gorev != "Müşteri" && x.Gorev != "SMD Viewer" && x.Gorev != "THT Viewer").ToList()
                                                     select new SelectListItem
                                                     {
                                                         Text = p.Personel,
                                                         Value = p.KullaniciAdi,
                                                     }).ToList();
                    ViewBag.PersonelDDl = personel;
                    return View("YeniGorev");
                }
            }
            else
            {
                ViewBag.Msg = "En az bir personel atamanız gerekmektedir";
                return View();
            }
        }

        public ActionResult KendiGorevimiHatirlat()
        {
            var today = System.DateTime.Today;
            var tasks = db.Gorevler.ToList();
            var mytasks_expired = tasks.Where(x => x.AtayanID == x.Atanan1ID && (Convert.ToDateTime(x.Bitis_Tarihi) < today) && (x.Tamamlanma_Tarihi == null)).ToList();
            var mytasks_today = tasks.Where(x => x.AtayanID == x.Atanan1ID && (Convert.ToDateTime(x.Bitis_Tarihi) == today) && (x.Tamamlanma_Tarihi == null)).ToList();
            //var mytasks_last3days = tasks.Where(x => x.AtayanID == x.Atanan1ID && ((Convert.ToDateTime(x.Bitis_Tarihi) - today).Days < 4 && (Convert.ToDateTime(x.Bitis_Tarihi) - today).Days > 0) && (x.Tamamlanma_Tarihi == null)).ToList();

            var mytasksexpired_count = mytasks_expired.Count();
            var mytaskstoday_count = mytasks_today.Count();
            //var mytaskslast3days_count = mytasks_last3days.Count();

            try
            {
                foreach (var task in mytasks_expired)
                {
                    var from = "bildirim@martelektronik.com";
                    var kullaniciMail = db.Kullanici.First(x => x.KullaniciAdi == task.AtayanID);
                    var gunfarki = (Convert.ToDateTime(task.Bitis_Tarihi) - Convert.ToDateTime(System.DateTime.Today)).Days;

                    MailMessage gorevmail_toappointee1 = new MailMessage(from, kullaniciMail.Mail);
                    gorevmail_toappointee1.From = new MailAddress(from, "Mart Elektronik");
                    gorevmail_toappointee1.Subject = "Kendi Görevim Hatırlatma";
                    gorevmail_toappointee1.Body = "Sayın " + kullaniciMail.Personel + ", " + "\r\n" + "\r\n" + " Kendinize eklediğiniz '" + task.Gorev + "' görevinizin Bitiş Tarihi üzerinden " + gunfarki + " gün geçmiştir.";

                    SmtpClient smtp1 = new SmtpClient();
                    smtp1.Host = "mail.martelektronik.com";
                    smtp1.Port = 587;
                    smtp1.EnableSsl = false;

                    NetworkCredential credential1 = new NetworkCredential("bildirim@martelektronik.com", "MartBildir24!!");
                    smtp1.UseDefaultCredentials = true;
                    smtp1.Credentials = credential1;
                    smtp1.Send(gorevmail_toappointee1);
                }

                foreach (var task in mytasks_today)
                {
                    var from = "bildirim@martelektronik.com";
                    var kullaniciMail = db.Kullanici.First(x => x.KullaniciAdi == task.AtayanID);

                    MailMessage gorevmail_toappointee1 = new MailMessage(from, kullaniciMail.Mail);
                    gorevmail_toappointee1.From = new MailAddress(from, "Mart Elektronik");
                    gorevmail_toappointee1.Subject = "Kendi Görevim Hatırlatma";
                    gorevmail_toappointee1.Body = "Sayın " + kullaniciMail.Personel + ", " + "\r\n" + "\r\n" + " Kendinize eklediğiniz '" + task.Gorev + "' görevinizin Bitiş Tarihi bugündür.";

                    SmtpClient smtp1 = new SmtpClient();
                    smtp1.Host = "mail.martelektronik.com";
                    smtp1.Port = 587;
                    smtp1.EnableSsl = false;

                    NetworkCredential credential1 = new NetworkCredential("bildirim@martelektronik.com", "MartBildir24!!");
                    smtp1.UseDefaultCredentials = true;
                    smtp1.Credentials = credential1;
                    smtp1.Send(gorevmail_toappointee1);
                }

                //foreach (var task in mytasks_last3days)
                //{
                //    var from = "bildirim@martelektronik.com";
                //    var kullaniciMail = db.Kullanici.First(x => x.KullaniciAdi == task.AtayanID);
                //    var gunfarki = (Convert.ToDateTime(task.Bitis_Tarihi) - Convert.ToDateTime(System.DateTime.Today)).Days;

                //    MailMessage gorevmail_toappointee1 = new MailMessage(from, kullaniciMail.Mail);
                //    gorevmail_toappointee1.From = new MailAddress(from, "Mart Elektronik");
                //    gorevmail_toappointee1.Subject = "Kendi Görevim Hatırlatma";
                //    gorevmail_toappointee1.Body = "Sayın " + kullaniciMail.Personel + ", " + "\r\n" + "\r\n" + " Kendinize eklediğiniz '" + task.Gorev + "' görevinizin Bitiş Tarihi'ne son " + gunfarki + " gün kalmıştır.";

                //    SmtpClient smtp1 = new SmtpClient();
                //    smtp1.Host = "mail.martelektronik.com";
                //    smtp1.Port = 587;
                //    smtp1.EnableSsl = false;

                //    NetworkCredential credential1 = new NetworkCredential("bildirim@martelektronik.com", "MartBildir24!!");
                //    smtp1.UseDefaultCredentials = true;
                //    smtp1.Credentials = credential1;
                //    smtp1.Send(gorevmail_toappointee1);
                //}
            }
            catch
            {
                ViewBag.Msg = "Mail Gönderilemedi";
            }
            return View();
        }


        public ActionResult Gorevlerim()
        {
            Dondur();
            ViewBag.Personel = Session["Personel"] as string;
            var personel = Session["KullaniciAdi"] as string;
            var gorevler = db.Gorevler.ToList();
            var gorevlerim = gorevler.Where(x => (x.AtayanID == personel || x.Atanan1ID == personel) && x.Durum == "Aktif").OrderBy(x => Convert.ToDateTime(x.Bitis_Tarihi)).ToList();
            return View(gorevlerim);
        }

        public ActionResult GorevTamamla(int id)
        {
            var gorevim = db.Gorevler.Find(id);
            gorevim.Durum = "Pasif";
            gorevim.Tamamlanma_Tarihi =System.DateTime.Today.ToString();
            db.SaveChanges();
            return RedirectToAction("Gorevlerim");
        }

        [Authorize(Roles = "1,2,3,4,9")]
        public ActionResult GorevGetir(int id)
        {
            if (Session["KullaniciAdi"] as string != null)
            {
                Dondur();
                var gorev = db.Gorevler.Find(id);
                List<SelectListItem> personel = (from p in db.Kullanici.Where(x => x.Gorev != "Müşteri" && x.Gorev != "SMD Viewer" && x.Gorev != "THT Viewer").ToList()
                                                 select new SelectListItem
                                                 {
                                                     Text = p.Personel,
                                                     Value = p.KullaniciAdi,
                                                 }).ToList();
                ViewBag.PersonelDDl = personel;
                ViewBag.PersonelID = Session["KullaniciAdi"] as string;
                ViewBag.Atayan = gorev.Atayan;
                ViewBag.AtayanID = gorev.AtayanID;
                ViewBag.Atanan1ID = gorev.Atanan1ID;
                ViewBag.Atanan2ID = gorev.Atanan2ID;
                ViewBag.Atanan3ID = gorev.Atanan3ID;
                TempData["AtayanID"] = gorev.AtayanID;
                TempData["Atanan1ID"] = gorev.Atanan1ID;
                TempData["Atanan2ID"] = gorev.Atanan2ID;
                TempData["Atanan3ID"] = gorev.Atanan3ID;
                ViewBag.Atanan1 = gorev.Atanan1;
                ViewBag.Atanan2 = gorev.Atanan2;
                ViewBag.Atanan3 = gorev.Atanan3;
                ViewBag.Id = id;
                TempData["GorevGetirID"] = id;
                ViewBag.GorevID = gorev.GorevDosyaID;
                TempData["GorevID"] = ViewBag.GorevID;
                ViewBag.GorevDosyaCount = db.GorevDosyalaris.Where(x => x.GorevID == gorev.GorevDosyaID).Count();
                ViewBag.FileSizeErrMsg = TempData["FileSizeErrMsg"];
                ViewBag.MaxFileCountErr = TempData["MaxFileCountErr"];

                return View("GorevGetir", gorev);
            }
            else
            {
                return RedirectToAction("KullaniciGirisi", "Giris");
            }
        }

        static int gorevidstatic;
        public ActionResult GoreveEklenenDosyalariGetir(string id)
        {
            int gorevid = gorevidstatic;
            ViewBag.ID = TempData["GorevID"];
            ViewBag.AtayanID = TempData["AtayanID"];
            ViewBag.Atanan1ID = TempData["Atanan1ID"];
            ViewBag.Atanan2ID = TempData["Atanan2ID"];
            ViewBag.Atanan3ID = TempData["Atanan3ID"];
            ViewBag.GorevGetirID = TempData["GorevGetirID"];
            ViewBag.ID = gorevidstatic;
            if (id != null)
            {
                Session["GorevID"] = id;
                var goreveeklenendosyalar = db.GorevDosyalaris.Where(x => x.GorevID == id).ToList();
                var gorevler = db.Gorevler.First(x => x.GorevDosyaID == id);
                ViewBag.Gorev = gorevler.Gorev;
                return View(goreveeklenendosyalar);
            }
            else
            {
                ViewBag.DosyaYokHataMsg = "Bu göreve eklenen dosya bulunmamaktadır";
                TempData["DosyaYokHataMsg"] = ViewBag.DosyaYokHataMsg;
                return RedirectToAction("BanaAtanmisGorevGetir/" + gorevid);
            }
        }

        [Authorize(Roles = "1,2,3,4,9")]
        public ActionResult GorevDuzenle(Gorevler g, GorevDosyalari gd, FormCollection form, HttpPostedFileBase DosyaYolu)
        {
            var logged_personelid = Session["KullaniciAdi"] as string;
            var logged_personel = Session["Personel"] as string;
            var gorev = db.Gorevler.Find(g.ID);
            var gorevdosyalari = db.GorevDosyalaris.ToList();
            var loggedpersonel_eklenengorevdosyacount = gorevdosyalari.Where(x => x.GorevID == Convert.ToString(gorev.ID) && x.EkleyenPersonelID == logged_personelid).Count();

            if (DosyaYolu != null)
            {
                if (loggedpersonel_eklenengorevdosyacount < 3)
                {
                    var filesize = Request.Files[0].ContentLength;
                    if (filesize <= 10000000)
                    {
                        db.GorevDosyalaris.Add(gd);
                        string filename = Path.GetFileName(Request.Files[0].FileName);
                        string ext = Path.GetExtension(Request.Files[0].FileName);
                        string path = "/Content/GorevDosyalari/" + filename + ext;
                        Request.Files[0].SaveAs(Server.MapPath(path));
                        gd.EkleyenPersonelID = logged_personelid;
                        gd.EkleyenPersonel = logged_personel;
                        gd.DosyaYolu = path;
                        gd.GorevID = gorev.GorevDosyaID;
                        db.SaveChanges();
                    }
                    else
                    {
                        ViewBag.FileSizeErrMsg = "Dosya boyutu 10 MB üzerinde olamaz";
                        TempData["FileSizeErrMsg"] = ViewBag.FileSizeErrMsg;
                        return RedirectToAction("GorevGetir/" + g.ID);
                    }
                }
                else
                {
                    ViewBag.MaxFileCountErr = "Bir göreve en fazla 3 dosya ekleyebilirsiniz";
                    TempData["MaxFileCountErr"] = ViewBag.MaxFileCountErr;
                    return RedirectToAction("GorevGetir/" + g.ID);
                }
            }


            var atanan1 = form["AtananPersonel1"].ToString();
            if (atanan1 != "")
            {
                var personel1 = db.Kullanici.First(x => x.KullaniciAdi == atanan1);
                gorev.Gorev = g.Gorev;
                gorev.Baslama_Tarihi = g.Baslama_Tarihi;
                gorev.Bitis_Tarihi = g.Bitis_Tarihi;
                gorev.Atanan1ID = personel1.KullaniciAdi;
                gorev.Atanan1 = personel1.Personel;
                gorev.Not_Atayan = g.Not_Atayan;
                db.SaveChanges();

                var atanan2 = form["AtananPersonel2"].ToString();
                if (atanan2 != "" && atanan2 != atanan1)
                {
                    var personel2 = db.Kullanici.First(x => x.KullaniciAdi == atanan2);
                    gorev.Atanan2ID = personel2.KullaniciAdi;
                    gorev.Atanan2 = personel2.Personel;
                    db.SaveChanges();

                    var atanan3 = form["AtananPersonel3"].ToString();
                    if (atanan3 != "" && atanan3 != atanan2 && atanan3 != atanan1)
                    {
                        var personel3 = db.Kullanici.First(x => x.KullaniciAdi == atanan3);
                        gorev.Atanan3ID = personel3.KullaniciAdi;
                        gorev.Atanan3 = personel3.Personel;
                        db.SaveChanges();
                    }
                    else
                    {
                        return RedirectToAction("Gorevlerim");
                    }
                }
                else
                {
                    return RedirectToAction("Gorevlerim");
                }
            }
            else
            {
            }
            return View("GorevGetir");
        }

        [Authorize(Roles = "1,2,9")]
        public ActionResult GorevSil(int id)
        {
            var gorev = db.Gorevler.Find(id);
            db.Gorevler.Remove(gorev);
            db.SaveChanges();
            return RedirectToAction("Gorevlerim");
        }

        [Authorize(Roles = "1,2,3,4,9")]
        public ActionResult GecmisGorevler()
        {
            if (Session["KullaniciAdi"] as string != null)
            {
                Dondur();
                var personelid = Session["KullaniciAdi"] as string;
                ViewBag.PersonelID = personelid;
                ViewBag.GecmisGorevCount = db.Gorevler.Where(x => (x.AtayanID == personelid || x.Atanan1ID == personelid || x.Atanan2ID == personelid || x.Atanan3ID == personelid) && (x.Durum == "Pasif")).Count();
                var gecmisgorev = db.Gorevler.Where(x => (x.AtayanID == personelid || x.Atanan1ID == personelid || x.Atanan2ID == personelid || x.Atanan3ID == personelid) && (x.Durum == "Pasif")).OrderByDescending(x => x.ID).ToList();
                return View(gecmisgorev);
            }
            else
            {
                return RedirectToAction("KullaniciGirisi", "Giris");
            }
        }

        [Authorize(Roles = "1,2,3,4,9")]
        public ActionResult GecmisGorevDetay(int id)
        {
            var gorev = db.Gorevler.Find(id);
            ViewBag.Atanan1 = gorev.Atanan1;
            ViewBag.Atanan2 = gorev.Atanan2;
            ViewBag.Atanan3 = gorev.Atanan3;
            ViewBag.Atayan = gorev.Atayan;
            ViewBag.TamamlanmaTarihi = Convert.ToDateTime(gorev.Tamamlanma_Tarihi).ToString("dd/MM/yyyy");
            return View(gorev);
        }

        public ActionResult DosyaIndir(int id, string gorevid)
        {
            try
            {
                var bul = db.GorevDosyalaris.First(x => x.GorevID == gorevid && x.ID == id);
                string path = Server.MapPath(bul.DosyaYolu);
                string ext = Path.GetExtension(path);
                var contentType = MimeMapping.GetMimeMapping(path);
                return File(path, contentType, Path.GetFileName(bul.DosyaYolu));
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
            var gorevdosyasi = db.GorevDosyalaris.Find(id);
            db.GorevDosyalaris.Remove(gorevdosyasi);
            db.SaveChanges();
            return RedirectToAction("GoreveEklenenDosyalariGetir/" + Convert.ToString(gorevdosyasi.GorevID));
        }
    }
}