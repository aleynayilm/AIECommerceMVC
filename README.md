# AIECommerceWeb

Bu proje, kullanıcıların yapay zeka destekli bir sistem üzerinden e-ticaret ürünlerini kendi fotoğrafları üzerinde deneyimlemelerini sağlayan bir ASP.NET Core MVC uygulamasıdır.

## 📦 Proje Özellikleri

- Ürün listeleme ve kategori filtreleme
- Sepete ürün ekleme ve sipariş oluşturma
- Admin paneli üzerinden ürün/kategori yönetimi
- Yapay zeka entegrasyonu ile kullanıcı fotoğrafı üzerine ürün "Try On" sistemi
- Entity Framework Core ile veritabanı işlemleri
- Yapay zeka servisi: Replicate AI API kullanımı

## 🛠️ Kurulum Adımları

### 1. **Projeyi klonlayın:**

```bash
git clone https://github.com/aleynayilm/AIECommerceMVC.git
```

### 2. Gerekli NuGet paketlerini yükleyin:

Visual Studio üzerinde Tools > NuGet Package Manager > Manage NuGet Packages menüsünden eksik bağımlılıkları yükleyin.

### 3. appsettings.json dosyasını düzenleyin:

```csharp
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=AIECommerceDb;Trusted_Connection=True;"
  },
  "ReplicateAPI": {
    "BaseUrl": "https://api.replicate.com/v1/",
    "ApiKey": "your_api_key_here"
  }
}
```
🔐 Not: ApiKey değeri size özel olmalıdır. Replicate.com üzerinden alınmalıdır.

### 4. Veritabanını oluşturun:

EF Core kullanılarak veritabanı oluşturulmuştur.

### 5. Projeyi çalıştırın:

Visual Studio üzerinden IIS Express ile projeyi başlatabilirsiniz.

## 👤 Kullanıcı Rolleri

Admin: Ürün, kategori ve siparişleri yönetebilir.

Kullanıcı: Ürünleri görüntüleyebilir, sepete ekleyebilir ve kendi fotoğrafıyla ürünü deneyebilir.

## 🤖 Yapay Zeka Kullanımı

Kullanıcılar ürün detay sayfasında "Try On" butonuna tıklayarak kendi fotoğraflarını yükler. Görsel, Replicate AI API'ye gönderilir ve sonuç uygulamada kullanıcıya gösterilir.

## 📂 Proje Yapısı

Controllers/: MVC controller sınıfları

Models/: Veritabanı modelleri

Views/: Razor view dosyaları

Services/ReplicateAIService.cs: Yapay zeka API işlemleri
