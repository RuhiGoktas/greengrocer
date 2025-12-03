
# 🍏 Greengrocer — Modern Sipariş Yönetimi Uygulaması

**Greengrocer**, meyve–sebze odaklı bir sipariş yönetim platformudur.
.NET Core Razor Pages ile hazırlanmış, JWT ile güvenli API erişimi sunan, DevExtreme DataGrid ile gelişmiş tablo özelliklerine sahip modern bir uygulamadır.

Bu proje, hem frontend hem backend becerilerini sergileyen temiz bir örnek uygulamadır.

---

## 🚀 Özellikler

### ✔ Dinamik Sipariş Oluşturma

* Bir sipariş içinde çoklu ürün (item) ekleme
* Ürün başlığı + miktar alanları
* Dinamik satır ekleme/silme (JavaScript + jQuery)
* Ürün kartlarından **“Sepete Ekle”** ile otomatik item ekleme

### ✔ Meyve–Sebze Kartları (Carrefour Tarzı UI)

* Responsive, modern ürün kartları
* Fiyat, indirim, badge, resim desteği
* Tıklanınca otomatik sipariş satırı oluşturur

### ✔ DevExtreme DataGrid Entegrasyonu

* Otomatik API’den veri çekme
* Arama, filtreleme, kolon sıralama
* **JWT Token ile korunan API üzerinden veri alma**
* İçerik kolonunda çoklu ürünlerin gösterimi

### ✔ JWT Authentication

* `/api/auth/token` ile kullanıcı adı / şifre doğrulama
* `/api/orders` endpoint’i **Bearer Token** olmadan çalışmaz
* Win7 + eski browser uyumluluğu için custom `onBeforeSend` header enjeksiyonu

### ✔ MSSQL + EF Core 3.1

* `Orders` ve `OrderItems` tabloları
* Migration desteği
* Uygulama startında `EnsureCreated()` fallback’i

---

## 📸 Ekran Görüntüleri

### 🛒 Sipariş Yönetimi Sayfası


```
![grocer1](https://github.com/user-attachments/assets/8fb38148-bd9d-486e-a8ab-54457f88ed96)


```

### 📊 DevExtreme Grid — Sepetteki Siparişler

```
![grocer2](https://github.com/user-attachments/assets/efa9fd42-7377-4297-95d5-0ec64b8e81da)

```


---

## 🧱 Kullanılan Teknolojiler

| Katman             | Teknoloji                                                        |
| ------------------ | ---------------------------------------------------------------- |
| **Frontend**       | Razor Pages, HTML, CSS, Bootstrap 4, jQuery, DevExtreme DataGrid |
| **Backend**        | ASP.NET Core 3.1, JWT Authentication, Repository Pattern         |
| **Database**       | Microsoft SQL Server, EF Core 3.1                                |
| **Authentication** | Bearer Token (JWT)                                               |
| **Tooling**        | Visual Studio 2019, Git, GitHub                                  |

---

## 🗂 Proje Yapısı

```plaintext
/Pages
    /Order.cshtml      → Sipariş yönetimi UI
    /Login.cshtml      → Basit giriş ekranı
/Models
    Order.cs
    OrderItem.cs
/Data
    AppDbContext.cs    → EF Core DB Context
/Api
    OrdersController.cs → JWT korumalı API
    AuthController.cs   → Token üretici
/wwwroot
    /images            → Ürün resimleri
```

---

## 🔐 JWT Token Kullanımı

**Token alma:**

```http
POST /api/auth/token
Content-Type: application/json

{
  "username": "admin",
  "password": "123456"
}
```

**Response:**

```json
{
  "token": "eyJhbGciOiJIUzI1NiIs..."
}
```

**Korumalı endpoint kullanımı:**

```http
GET /api/orders
Authorization: Bearer eyJh..."
```

DevExtreme grid içinde token şu şekilde eklenir:

```javascript
onBeforeSend: function (method, ajaxOptions) {
    ajaxOptions.headers = ajaxOptions.headers || {};
    ajaxOptions.headers["Authorization"] = "Bearer " + token;
}
```

---

## 📦 Kurulum

### 1. Bağımlılıkları yükle

Proje .NET Core 3.1 ile çalışır.

```bash
dotnet restore
```

### 2. Database oluşturma (otomatik)

Uygulama ilk çalıştığında EF Core tabloyu otomatik oluşturur.

### 3. Çalıştır

```bash
dotnet run
```

---

## 🤝 Katkıda Bulunanlar

**Furkan Ruhi Goktaş** — Full Stack Geliştirici

GitHub: [https://github.com/RuhiGoktas](https://github.com/RuhiGoktas)

---

## ⭐ Destek Olmak İstersen

Projeyi beğendiysen **Star 🌟** atabilirsin!
