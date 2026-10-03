# Support Ticket Management System

سیستم مدیریت درخواست‌های پشتیبانی با استفاده از **ASP.NET Core Web API،
Angular و SQL Server**.

این پروژه امکان ثبت، مشاهده، جستجو، فیلتر و تغییر وضعیت درخواست‌های
پشتیبانی را فراهم می‌کند. همچنین درخواست‌های با اولویت بالا که بیش از زمان
مجاز در وضعیت Open باقی مانده‌اند، به‌صورت Overdue مشخص می‌شوند.

## تکنولوژی‌های استفاده‌شده

### Backend

-   ASP.NET Core Web API
-   Entity Framework Core
-   SQL Server
-   Clean Architecture
-   Swagger / OpenAPI
-   EF Core Migrations

### Frontend

-   Angular
-   TypeScript
-   Standalone Components
-   Reactive Forms
-   HttpClient
-   RTL / Persian UI

### Database

-   Microsoft SQL Server

## معماری پروژه

Backend با استفاده از **Clean Architecture** پیاده‌سازی شده است تا
Business Logic از جزئیات زیرساختی مانند Database و API مستقل باشد.

``` text
SupportTicket
│
├── SupportTicket.Domain
│   ├── Entities
│   └── Enums
│
├── SupportTicket.Application
│   ├── DTOs
│   ├── Interfaces
│   └── Services
│
├── SupportTicket.Infrastructure
│   ├── Persistence
│   ├── Configurations
│   └── Repositories
│
└── SupportTicket.Api
    ├── Controllers
    └── Program.cs
```

### Domain

این لایه شامل مدل‌های اصلی سیستم و قوانین پایه Domain است و به سایر
لایه‌ها وابستگی ندارد.

موجودیت‌های اصلی: - Ticket - TicketStatusHistory

Enumهای اصلی: - TicketPriority - TicketStatus

### Application

این لایه شامل Use Caseها و منطق کاربردی سیستم است، از جمله: - ایجاد
Ticket - دریافت Ticketها - جستجو و فیلتر - تغییر وضعیت - محاسبه Overdue

### Infrastructure

مسئول ارتباط با زیرساخت است و شامل Entity Framework Core، SQL Server،
DbContext، Repositoryها و تنظیمات Database می‌شود.

### API

نقطه ورود Backend است. Controllerها HTTP Requestها را دریافت کرده و
عملیات مربوطه را از Application Layer فراخوانی می‌کنند.

## ساختار Database

پروژه از SQL Server و Entity Framework Core Code First استفاده می‌کند.

دو جدول اصلی:

``` text
Tickets
TicketStatusHistories
```

### جدول Tickets

  ستون            توضیح
  --------------- ------------------
  Id              شناسه درخواست
  Title           عنوان درخواست
  RequesterName   نام درخواست‌کننده
  Description     توضیحات
  Priority        اولویت درخواست
  Status          وضعیت فعلی
  CreatedAt       تاریخ ایجاد

Priority: - Low - Medium - High

Status: - Open - InProgress - Done

### جدول TicketStatusHistories

  ستون        توضیح
  ----------- ------------------
  Id          شناسه
  TicketId    شناسه Ticket
  Status      وضعیت
  StartedAt   زمان شروع وضعیت
  EndedAt     زمان پایان وضعیت

هر Ticket می‌تواند چندین Status History داشته باشد:

``` text
Tickets (1) -------- (*) TicketStatusHistories
```

نمونه:

``` text
Ticket #1

Open
08:00 → 18:00

InProgress
18:00 → 02:00

Open
02:00 → 17:00
```

## منطق Overdue

طبق Requirement پروژه، یک Ticket زمانی Overdue محسوب می‌شود که:

1.  Priority آن `High` باشد.
2.  مجموع زمانی که در وضعیت `Open` قرار داشته بیشتر از ۲۴ ساعت باشد.
3.  مدت حضور Ticket در `InProgress` در این محاسبه لحاظ نشود.

به همین دلیل صرفاً اختلاف بین `CreatedAt` و زمان فعلی محاسبه نمی‌شود.
سیستم تاریخچه Statusها را در `TicketStatusHistories` ذخیره می‌کند.

هنگام تغییر Status، بازه وضعیت قبلی بسته شده و یک History جدید ایجاد
می‌شود.

مثال:

``` text
08:00  Ticket Created / Open
        ↓ 10 hours
18:00  InProgress
        ↓ 8 hours
02:00  Open
        ↓ 15 hours
17:00
```

محاسبه:

``` text
Open          10h
InProgress     8h   ← محاسبه نمی‌شود
Open          15h
-------------------
Total Open     25h
```

اگر Priority برابر `High` باشد، چون `25h > 24h` است، درخواست
`Overdue = true` خواهد بود.

## API Endpoints

### دریافت لیست درخواست‌ها

``` http
GET /api/tickets
```

### جستجو

``` http
GET /api/tickets?search=payment
```

جستجو روی Title و RequesterName انجام می‌شود.

### فیلتر بر اساس Status

``` http
GET /api/tickets?status=Open
```

### فیلتر بر اساس Priority

``` http
GET /api/tickets?priority=High
```

### ترکیب جستجو و فیلتر

``` http
GET /api/tickets?search=ali&status=Open&priority=High
```

### ایجاد درخواست

``` http
POST /api/tickets
```

نمونه Request:

``` json
{
  "title": "مشکل ورود به سیستم",
  "requesterName": "علی احمدی",
  "description": "امکان ورود به حساب کاربری وجود ندارد.",
  "priority": "High"
}
```

### تغییر وضعیت

``` http
PATCH /api/tickets/{id}/status
```

نمونه:

``` json
{
  "status": "InProgress"
}
```

## نحوه اجرای Backend

### پیش‌نیازها

-   .NET SDK
-   SQL Server

Connection String را در `appsettings.json` تنظیم کنید.

نمونه Windows Authentication:

``` json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=SupportTicketDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

نمونه SQL Server Authentication:

``` text
Server=localhost;Database=SupportTicketDb;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True;
```

Migrationها را اعمال کنید:

``` bash
dotnet ef database update
```

در صورت جدا بودن Startup Project و Infrastructure:

``` bash
dotnet ef database update --project SupportTicket.Infrastructure --startup-project SupportTicket.Api
```

سپس API را اجرا کنید:

``` bash
dotnet run --project SupportTicket.Api
```

## نحوه اجرای Frontend

وارد پوشه Angular شوید:

``` bash
cd support-ticket-ui
```

Dependencyها را نصب کنید:

``` bash
npm install
```

آدرس Backend را در Environment بررسی کنید:

``` typescript
export const environment = {
  apiUrl: 'https://localhost:7162/api'
};
```

سپس Angular را اجرا کنید:

``` bash
ng serve
```

در Windows PowerShell، اگر اجرای `ng.ps1` محدود باشد:

``` bash
ng.cmd serve
```

Frontend به‌صورت پیش‌فرض در این آدرس اجرا می‌شود:

``` text
http://localhost:4200
```

## قابلیت‌های پیاده‌سازی‌شده

-   نمایش لیست درخواست‌های پشتیبانی
-   ثبت درخواست جدید
-   ذخیره اطلاعات در SQL Server
-   تغییر وضعیت درخواست
-   جستجو بر اساس عنوان و نام درخواست‌کننده
-   فیلتر بر اساس Priority و Status
-   ثبت تاریخچه تغییر وضعیت
-   محاسبه مدت واقعی Open بودن درخواست
-   تشخیص درخواست‌های Overdue
-   رابط کاربری فارسی و RTL
-   نمایش تاریخ به‌صورت شمسی
-   Responsive UI
-   Swagger برای بررسی APIها

## تصمیمات طراحی

برای تشخیص Overdue از فیلد ساده‌ای مانند `IsOverdue` در Database استفاده
نشده است؛ زیرا Overdue یک مقدار محاسباتی و وابسته به زمان است و ممکن است
بدون هیچ تغییری در رکورد Ticket از `false` به `true` تبدیل شود.

همچنین برای محاسبه مدت Open بودن، تنها از `CreatedAt` استفاده نشده است؛
زیرا طبق Requirement، مدت حضور در `InProgress` نباید محاسبه شود.

به همین دلیل تاریخچه Statusها نگهداری شده و مجموع بازه‌های `Open` محاسبه
می‌شود. این طراحی علاوه بر حفظ Audit Trail، امکان توسعه قوانین SLA در
آینده را نیز فراهم می‌کند.
