namespace AracKiralamaOtomasyonu.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DbAracKiralama : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AracDisDonanim",
                c => new
                    {
                        IDAracDisDonanim = c.Int(nullable: false, identity: true),
                        IDIlan = c.Int(nullable: false),
                        FarLed = c.Boolean(nullable: false),
                        FarSis = c.Boolean(nullable: false),
                        Sunroof = c.Boolean(nullable: false),
                        ElektrikliAyna = c.Boolean(nullable: false),
                        ParkSensoru = c.Boolean(nullable: false),
                        OtomatikKatlananAyna = c.Boolean(nullable: false),
                        RomorkDemiri = c.Boolean(nullable: false),
                        ParkAsistani = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IDAracDisDonanim);
            
            CreateTable(
                "dbo.Ilanlar",
                c => new
                    {
                        IDIlan = c.Int(nullable: false, identity: true),
                        IDAracMarka = c.Int(nullable: false),
                        IDAracModel = c.Int(nullable: false),
                        IDMusteri = c.Int(nullable: false),
                        IDAracIcDonanim = c.Int(nullable: false),
                        IDAracGuvenlik = c.Int(nullable: false),
                        IDAracDisDonanim = c.Int(nullable: false),
                        IDAracMultiMedya = c.Int(nullable: false),
                        IDKurumsal = c.Int(),
                        IDDosya = c.Int(nullable: false),
                        Durum = c.Byte(nullable: false),
                        Baslik = c.String(maxLength: 250),
                        Fiyat = c.Int(nullable: false),
                        Kilometre = c.String(maxLength: 50),
                        Yil = c.DateTime(nullable: false, storeType: "date"),
                        Yakit = c.String(maxLength: 50),
                        Vites = c.String(maxLength: 50),
                        Kasa = c.String(maxLength: 50),
                        MotorGucu = c.String(maxLength: 50),
                        MotorHacmi = c.String(maxLength: 50),
                        Cekis = c.String(maxLength: 30),
                        IlanTarihi = c.DateTime(nullable: false, storeType: "date"),
                        Aciklama = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.IDIlan)
                .ForeignKey("dbo.AracDisDonanim", t => t.IDAracDisDonanim, cascadeDelete: true)
                .ForeignKey("dbo.AracGuvenlik", t => t.IDAracGuvenlik, cascadeDelete: true)
                .ForeignKey("dbo.AracIcDonanim", t => t.IDAracIcDonanim, cascadeDelete: true)
                .ForeignKey("dbo.AracMarka", t => t.IDAracMarka, cascadeDelete: true)
                .ForeignKey("dbo.AracModel", t => t.IDAracModel, cascadeDelete: true)
                .ForeignKey("dbo.AracMultiMedya", t => t.IDAracMultiMedya, cascadeDelete: true)
                .ForeignKey("dbo.Dosyalar", t => t.IDDosya, cascadeDelete: true)
                .ForeignKey("dbo.Kullanici", t => t.IDMusteri, cascadeDelete: true)
                .Index(t => t.IDAracMarka)
                .Index(t => t.IDAracModel)
                .Index(t => t.IDMusteri)
                .Index(t => t.IDAracIcDonanim)
                .Index(t => t.IDAracGuvenlik)
                .Index(t => t.IDAracDisDonanim)
                .Index(t => t.IDAracMultiMedya)
                .Index(t => t.IDDosya);
            
            CreateTable(
                "dbo.AracGuvenlik",
                c => new
                    {
                        IDAracGuvenlik = c.Int(nullable: false, identity: true),
                        IDIlan = c.Int(nullable: false),
                        ABS = c.Boolean(nullable: false),
                        YokusDestegi = c.Boolean(nullable: false),
                        HavaYastigi = c.Boolean(nullable: false),
                        CocukKilidi = c.Boolean(nullable: false),
                        LastikAriza = c.Boolean(nullable: false),
                        MerkeziKilit = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IDAracGuvenlik);
            
            CreateTable(
                "dbo.AracIcDonanim",
                c => new
                    {
                        IDAracIcDonanim = c.Int(nullable: false, identity: true),
                        IDIlan = c.Int(nullable: false),
                        DeriKoltuk = c.Boolean(nullable: false),
                        SogutmaliKoltuk = c.Boolean(nullable: false),
                        YolBilgisayarı = c.Boolean(nullable: false),
                        KumasKoltuk = c.Boolean(nullable: false),
                        Klima = c.Boolean(nullable: false),
                        HidrolikDireksiyon = c.Boolean(nullable: false),
                        HizSabitleyici = c.Boolean(nullable: false),
                        GeriGorusKamera = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IDAracIcDonanim);
            
            CreateTable(
                "dbo.AracMarka",
                c => new
                    {
                        IDAracMarka = c.Int(nullable: false, identity: true),
                        Ad = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.IDAracMarka);
            
            CreateTable(
                "dbo.AracModel",
                c => new
                    {
                        IDAracModel = c.Int(nullable: false, identity: true),
                        ModelAd = c.String(maxLength: 30),
                        IDAracMarka = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IDAracModel)
                .ForeignKey("dbo.AracMarka", t => t.IDAracMarka, cascadeDelete: true)
                .Index(t => t.IDAracMarka);
            
            CreateTable(
                "dbo.AracMultiMedya",
                c => new
                    {
                        IDAracMultiMedya = c.Int(nullable: false, identity: true),
                        IDIlan = c.Int(nullable: false),
                        Radyo = c.Boolean(nullable: false),
                        TV = c.Boolean(nullable: false),
                        Bluetooth = c.Boolean(nullable: false),
                        USB = c.Boolean(nullable: false),
                        AUX = c.Boolean(nullable: false),
                        Navigasyon = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IDAracMultiMedya);
            
            CreateTable(
                "dbo.Dosyalar",
                c => new
                    {
                        IDDosya = c.Int(nullable: false, identity: true),
                        IDKullanici = c.Int(nullable: false),
                        IDIlan = c.Int(),
                        Url = c.String(maxLength: 250),
                        tip = c.String(maxLength: 20),
                        IlkFoto = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IDDosya);
            
            CreateTable(
                "dbo.Kullanici",
                c => new
                    {
                        IDKullanici = c.Int(nullable: false, identity: true),
                        ProfilFotoUrl = c.String(maxLength: 100),
                        Ad = c.String(maxLength: 30),
                        Soyad = c.String(maxLength: 30),
                        KullaniciAd = c.String(maxLength: 30),
                        Telefon = c.String(maxLength: 15),
                        TCKimlik = c.String(maxLength: 11),
                        Adres = c.String(maxLength: 250),
                        DogumTarihi = c.DateTime(nullable: false, storeType: "date"),
                        Cinsiyet = c.Boolean(nullable: false),
                        Eposta = c.String(maxLength: 50),
                        SifreHash = c.Binary(maxLength: 500),
                        SifreSalt = c.Binary(maxLength: 500),
                        Dogrulama = c.Boolean(nullable: false),
                        KayitTarihi = c.DateTime(nullable: false, storeType: "date"),
                        SonGirisTarihi = c.DateTime(nullable: false, storeType: "date"),
                    })
                .PrimaryKey(t => t.IDKullanici);
            
            CreateTable(
                "dbo.SifreSifirlama",
                c => new
                    {
                        IDSifreSifirlama = c.Int(nullable: false, identity: true),
                        KullaniciID = c.Int(nullable: false),
                        Eposta = c.String(maxLength: 50),
                        Dogrulama = c.Boolean(nullable: false),
                        DogrulamaLinki = c.String(maxLength: 50),
                        KayitTarihi = c.DateTime(nullable: false, storeType: "date"),
                    })
                .PrimaryKey(t => t.IDSifreSifirlama);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ilanlar", "IDMusteri", "dbo.Kullanici");
            DropForeignKey("dbo.Ilanlar", "IDDosya", "dbo.Dosyalar");
            DropForeignKey("dbo.Ilanlar", "IDAracMultiMedya", "dbo.AracMultiMedya");
            DropForeignKey("dbo.Ilanlar", "IDAracModel", "dbo.AracModel");
            DropForeignKey("dbo.Ilanlar", "IDAracMarka", "dbo.AracMarka");
            DropForeignKey("dbo.AracModel", "IDAracMarka", "dbo.AracMarka");
            DropForeignKey("dbo.Ilanlar", "IDAracIcDonanim", "dbo.AracIcDonanim");
            DropForeignKey("dbo.Ilanlar", "IDAracGuvenlik", "dbo.AracGuvenlik");
            DropForeignKey("dbo.Ilanlar", "IDAracDisDonanim", "dbo.AracDisDonanim");
            DropIndex("dbo.AracModel", new[] { "IDAracMarka" });
            DropIndex("dbo.Ilanlar", new[] { "IDDosya" });
            DropIndex("dbo.Ilanlar", new[] { "IDAracMultiMedya" });
            DropIndex("dbo.Ilanlar", new[] { "IDAracDisDonanim" });
            DropIndex("dbo.Ilanlar", new[] { "IDAracGuvenlik" });
            DropIndex("dbo.Ilanlar", new[] { "IDAracIcDonanim" });
            DropIndex("dbo.Ilanlar", new[] { "IDMusteri" });
            DropIndex("dbo.Ilanlar", new[] { "IDAracModel" });
            DropIndex("dbo.Ilanlar", new[] { "IDAracMarka" });
            DropTable("dbo.SifreSifirlama");
            DropTable("dbo.Kullanici");
            DropTable("dbo.Dosyalar");
            DropTable("dbo.AracMultiMedya");
            DropTable("dbo.AracModel");
            DropTable("dbo.AracMarka");
            DropTable("dbo.AracIcDonanim");
            DropTable("dbo.AracGuvenlik");
            DropTable("dbo.Ilanlar");
            DropTable("dbo.AracDisDonanim");
        }
    }
}
