# Mikroservis Tabanlı E-Ticaret ve Yetkilendirme API Projesi

Bu projeyi, mikroservis mimarisini derinlemesine öğrenmek, katmanlı bir yapıda sistemlerin birbiriyle nasıl haberleşeceğini denemek ve güncel deployment süreçlerini (Docker, CI/CD) uygulamak için geliştirdim. İçerisinde yetkilendirme, ürün yönetimi ve sipariş süreçlerini barındıran, hepsi tek bir kapıdan (Gateway) asenkron olarak yönetilen kapsamlı bir yapı bulunuyor.

## 🚀 Neler Kullandım?
* **Backend:** .NET 8.0 (ASP.NET Core)
* **Veritabanı:** MS SQL Server & Entity Framework Core
* **Mesaj Kuyruğu (Event-Driven):** RabbitMQ & MassTransit (SAGA Pattern ve asenkron iletişim için)
* **Güvenlik:** JWT (JSON Web Token), Role-Based ve Policy-Based Authorization
* **Merkezi Geçit (Gateway):** Ocelot
* **Loglama:** Serilog (Hataları ve işlemleri logs klasörü altındaki txt dosyasına yazıyor)
* **DevOps & Dağıtım:** Docker ve GitHub Actions (CI/CD Pipeline)

## 🏗️ Projeyi Nasıl Kurdum ve Mimari Nasıl Çalışıyor?
Projeyi geliştirirken monolitik (tek parça) bir yapı yerine, sorumlulukları 4 ana bölüme ayırdım:

1. **Giriş ve Kayıt İşlemleri (AuthService):** Kullanıcıların güvenli bir şekilde kayıt olup giriş yapabileceği servisi hallettim. Başarılı girişlerde kullanıcının yetkilerini barındıran bir JWT token üretiyor.
2. **Ürün Yönetimi (ProductService):** Ürünlerin listelenmesi ve yeni ürün eklenmesi için ayrı bir servis kurup kendi veritabanı bağlantılarını yaptım.
3. **Sipariş ve Asenkron İşlemler (OrderService):** Kullanıcıların sipariş verebildiği en kritik servis. 
   * **Güvenlik:** Uçları korumak için Role-Based (sadece Admin yetkililer listelemeyi görebilir) ve Policy-Based (sadece IT departmanı sipariş girebilir) kısıtlamalar ekledim.
   * **Tasarım Kararı:** Sipariş oluştuğunda sistemi bekletmemek için *Event-Driven Mimari* kullandım. Sipariş veritabanına yazıldığı an, MassTransit aracılığıyla RabbitMQ kuyruğuna bir `OrderCreatedEvent` fırlatılıyor. Bu sayede SAGA pattern için dağıtık transaction altyapısını kurmuş oldum.
4. **Merkezi Yönetim (API Gateway):** Tüm servisleri ayrı ayrı adreslerden çağırmak yerine, her şeyi tek bir port (Port 7000) üzerinden yöneten bir Gateway (Ocelot) ekledim. İstemci sadece burayı tanıyor.

## 🛠️ Çalıştırma Notları
Projeyi bilgisayarınıza indirdiğinizde sorunsuz çalıştırmak için şu adımları izleyebilirsiniz:

1. Veritabanı bağlantısı için servislerin içindeki `appsettings.json` dosyalarında bulunan SQL bağlantı cümlesini (`ConnectionString`) kendi bilgisayarınıza göre düzenlemeniz gerekiyor.
2. Veritabanı tablolarının oluşması için Visual Studio'da Paket Yöneticisi Konsolu üzerinden ilgili projeyi (örn: OrderService.API) seçip `Update-Database` komutunu çalıştırmalısınız.
3. Sipariş servisinin mesaj fırlatabilmesi için bilgisayarınızda RabbitMQ'nun çalışıyor olması gerekmektedir.
4. Visual Studio'da Çözüm (Solution) özelliklerine girip "Çoklu Başlatma" (Multiple Startup Projects) ayarını aktif edip projeyi çalıştırdığınızda tüm sistem ayağa kalkacaktır.

## 🐳 Docker ve CI/CD Entegrasyonu
* **Docker:** Projeyi farklı ortamlarda sorunsuz ayağa kaldırmak için servislere `Dockerfile` ekleyerek containerize ettim. İmajları terminalden `docker build` komutlarıyla oluşturabilirsiniz.
* **CI/CD:** Projeyi GitHub'a entegre ettim. `.github/workflows/pipeline.yml` dosyası sayesinde `test` veya `prod` branch'lerine kod gönderildiğinde GitHub Actions devreye girip projeyi otomatik olarak derliyor ve test ediyor.