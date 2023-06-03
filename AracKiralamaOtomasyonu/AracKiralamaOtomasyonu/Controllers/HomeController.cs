using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AracKiralamaOtomasyonu.Models;
using AracKiralamaOtomasyonu.Models.Class;
using AracKiralamaOtomasyonu.PasswordHash;

namespace AracKiralamaOtomasyonu.Controllers
{

    //-DEVELOPER BY EMİR YASİN SÜT
    //  -https://github.com/emirst20
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (AracKiralamaContext db = new AracKiralamaContext())
            {
                IlanListeViewModel model = new IlanListeViewModel();

                var sorgu = db.Ilanlar.Include("AracMarka").Include("AracModel").Include("AracGuvenlik").Include("AracDisDonanim").Include("AracIcDonanim").Include("AracMultiMedya").Include("Kullanici").Include("Dosyalar").Where(x=>x.Durum == 3).ToList().OrderBy(s=>s.IlanTarihi).Take(10);

                model.Ilanlars = sorgu.ToList();
                model.Dosyalar = db.Dosyalar.Where(x => x.tip == "ilan" && x.IlkFoto == true).FirstOrDefault();

                ViewBag.user = db.Kullanici.Count();
                return View(model);
            }

        }

        public ActionResult Giris(int? eror)
        {
            if (eror != null)
            {
                ViewBag.eror = eror;
            }
            else
            {
                ViewBag.eror = 3;
            }

            return  View();
        }


        //Giriş yap butonuna basılınca bu metoda düşer burada parametre olarak eposta ve sifre gönderilir
        public ActionResult GirisMethod(string eposta, string sifre)
        {
            using ( AracKiralamaContext db = new AracKiralamaContext())
            {
                //Sistemde girilen E-posta kullanıcıya ait mi diye kontrol edilir
                var Kullanıcıkontrol = db.Kullanici.Where(x => x.Eposta == eposta).FirstOrDefault();
                //Eğer kullanıcıya ait değilse bu kodun içine girer
                if (Kullanıcıkontrol == null)
                {
                    int eror = 1;
                    return RedirectToAction("Giris", new { eror });
                }
                //Kullanıcının şifresi yanlış ise hatalı şifre şeklinde mesaj gönderilir
                else if (!HashingHelper.VerifyPasswordHash(sifre, Kullanıcıkontrol.SifreHash, Kullanıcıkontrol.SifreSalt))
                {
                    int eror = 1;
                    return RedirectToAction("Giris", new { eror });
                }
                else
                {
                    //E-mail ve şifre doğru ise id,ad ve e posta session'a kaydedilri.
                    Session.Add("KullaniciId", Kullanıcıkontrol.IDKullanici);
                    Session.Add("KullaniciAd", Kullanıcıkontrol.Ad + " " + Kullanıcıkontrol.Soyad);

                    Session.Add("Email", Kullanıcıkontrol.Eposta);
                    //profil fotoğrafı sistemde kayıtlı ise kullanıcıc profil fotoğrafı session'a eklenir
                    if (Kullanıcıkontrol.ProfilFotoUrl != null)
                    {
                        Session.Add("ProfilFoto", Kullanıcıkontrol.ProfilFotoUrl);
                    }
                    else
                    {
                        //profil fotoğrafı sistemde kayıtlı değil ise kullanıcının profil fotoğrafı default olarak verilir ve session'a eklenir
                        Session.Add("ProfilFoto","default-profile.jpg" );
                    }

                    //İşemler sonucunda Kullanici paneline yönlendirilir
                    return RedirectToAction("Panel", "Kullanici");
                }

            }
        }

        public ActionResult Kayit()
        {
            return View();
        }
        public ActionResult KayıtMethod(string eposta, string ad, string soyad, string sifre, string telefon)
        {
            using (AracKiralamaContext db = new AracKiralamaContext())
            {
                Kullanici kullanici = new Kullanici();

                string Password = sifre;

                //Girilen Şifreyi Hashlemek için passwordHash kullanıyorum
                byte[] passwordHashnew, passwordSaltnew;
                HashingHelper.CreatePasswordHash(sifre, out passwordHashnew, out passwordSaltnew);

                //Burada kullanıcının girdiği bilgileri kullanici tablosuna kayıt ediyorum
                kullanici.SifreHash = passwordHashnew;
                kullanici.SifreSalt = passwordSaltnew;

                kullanici.Ad = ad;
                kullanici.Soyad = soyad;
                kullanici.KayitTarihi = DateTime.Now;
                kullanici.Eposta = eposta;
                kullanici.Telefon = telefon;
                kullanici.Dogrulama = false;
                db.Kullanici.Add(kullanici);
                db.SaveChanges();
                
                TempData["mailyok"] = "mailvar";
                TempData["mail"] = eposta;
            }


            return RedirectToAction("Giris");
        }

        public ActionResult EpostaDogrulamaBasarili()
        {
            return View();
        }

        public ActionResult SifremiUnuttum()
        {
            return View();
        }
        public ActionResult SifremiUnuttumMethod(string Eposta )
        {
            using (AracKiralamaContext db = new AracKiralamaContext())
            {
                //Girilen E-posta sistemde kayıtlı mı diye kontrol ediliyor
                var kullanici = db.Kullanici.Where(x => x.Eposta == Eposta).FirstOrDefault();

                string guid = Guid.NewGuid().ToString();
                guid = guid.Replace("-", string.Empty);
                guid = guid.Substring(0, 30);
                if (kullanici != null)
                {
                    //Şifre sıfırlayabilmesi için sifreSifirlama tablosuna kayıt ekliyorum ki buradan URL'e ulaşabilsin
                    SifreSifirlama sifreSifirlama = new SifreSifirlama();
                    sifreSifirlama.KayitTarihi = DateTime.Now;
                    sifreSifirlama.Eposta = Eposta;
                    sifreSifirlama.KullaniciID = kullanici.IDKullanici;
                    sifreSifirlama.Dogrulama = true;
                    sifreSifirlama.DogrulamaLinki = guid;
                    db.SifreSifirlama.Add(sifreSifirlama);
                    db.SaveChanges();
                }

                //Şifre sıfırlama için gönderdiğim E-posta içeriğini mail'in bodysine ekliyorum
                string filename = Server.MapPath("~/Views/Mail/SifreSifirlama.cshtml");
                string mailbody = System.IO.File.ReadAllText(filename);

                //Mail'de yer alan butona bağlantı ekleyebilmek için guid'yi maile içeriğine ekliyorum
                mailbody = mailbody.Replace("##guid##", guid);
                SmtpClient client = new SmtpClient();

                //Alttaki kodlar ise mail'in gönderilmesi için gerekli olan kodlar
                client.Port = 587;
                client.Host = "smtp-mail.outlook.com";
                client.EnableSsl = true;

                client.Credentials = new NetworkCredential("FastCar2023@outlook.com.tr", "gobeller67bb2411.ue");

                MailMessage mail = new MailMessage();

                mail.From = new MailAddress("FastCar2023@outlook.com.tr", "FastCar");

                mail.To.Add(Eposta);
                mail.Subject = "Şifre Sıfırlama";
                mail.IsBodyHtml = true;
                mail.Body = mailbody;
                mail.BodyEncoding = UTF8Encoding.UTF8;
                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                client.Send(mail);
                TempData["mailyok"] = "mailvar";
                TempData["mail"] = Eposta;
                //Eğer bu girilen mail sistemde kayıtlı değil ise SifremiUnuttum ekranına yönlendiriyorum

                    return RedirectToAction("SifremiUnuttum");


            }
          
        }

        [Route("SifreSifirla/{title}")]
        public ActionResult SifreSifirla(string title)
        {
            using (AracKiralamaContext db = new AracKiralamaContext())
            {
                var basvuru = db.SifreSifirlama.Where(x => x.DogrulamaLinki == title && x.Dogrulama == true).FirstOrDefault();
                if (basvuru != null)
                {
                    DateTime bTarih = Convert.ToDateTime(DateTime.Now);
                    DateTime kTarih = Convert.ToDateTime(basvuru.KayitTarihi);
                    TimeSpan Sonuc = bTarih - kTarih;
                    var sonucgun = Sonuc.TotalDays;
                    int sonucsayi = Convert.ToInt32(sonucgun);
                    if (sonucsayi <= 1)
                    {
                      var kullanici =  db.Kullanici.Where(x => x.Eposta == basvuru.Eposta).FirstOrDefault();
                        if (kullanici != null)
                        {
                            ViewBag.kullaniciid = kullanici.IDKullanici;
                            ViewBag.Eposta = kullanici.Eposta;
                        }

                        ViewBag.guid = title;
                        ViewBag.hata = 0;
                    }
                    else
                    {
                        ViewBag.hata = 1;
                    }
                }
                else
                {
                    return RedirectToAction("Giris");
                }

            }
            return View();
        }

        public ActionResult SifreSifirlaMethod(string yenisifre, string ID, string guid, string Eposta)
        {
            using (AracKiralamaContext db = new AracKiralamaContext())
            {
                int kullaniciid1 = Convert.ToInt32(ID);

                var kullanici = db.Kullanici.Where(x => x.IDKullanici == kullaniciid1 && x.Eposta == Eposta).FirstOrDefault();
                string sifreyeni = yenisifre;
                byte[] passwordHashnew, passwordSaltnew;
                HashingHelper.CreatePasswordHash(sifreyeni, out passwordHashnew, out passwordSaltnew);
                if (kullanici != null)
                {
                    kullanici.SifreHash = passwordHashnew;
                    kullanici.SifreSalt = passwordSaltnew;
                    db.SaveChanges();


                    var talep = db.SifreSifirlama.Where(x => x.DogrulamaLinki == guid).FirstOrDefault();
                    talep.Dogrulama = false;
                    db.SaveChanges();


                }
                return RedirectToAction("Giris");
            }

        }

        public ActionResult Logout()
        {
            Session["userId"] = null;
            Session["FirmaID"] = null;
            Session.Abandon();
            return RedirectToAction("Giris");
        }

        public String EpostaKontrol(string eposta)
        {
            using (AracKiralamaContext db = new AracKiralamaContext())
            {
                var kullanici = db.Kullanici.Where(x => x.Eposta == eposta).FirstOrDefault();
                if (kullanici != null)
                {
                    var data = "basarili";
                    return data;
                }
                else
                {
                    var data = "basarisiz";
                    return data;
                }
                
            }
        }


    }
}