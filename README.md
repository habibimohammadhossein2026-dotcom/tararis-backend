# \# Support Ticket Management System

# 

# سیستم مدیریت درخواست‌های پشتیبانی با استفاده از \*\*ASP.NET Core Web API، Angular و SQL Server\*\*.

# 

# این پروژه امکان ثبت، مشاهده، جستجو، فیلتر و تغییر وضعیت درخواست‌های پشتیبانی را فراهم می‌کند. همچنین درخواست‌های با اولویت بالا که بیش از زمان مجاز در وضعیت Open باقی مانده‌اند، به‌صورت Overdue مشخص می‌شوند.

# 

# \---

# 

# \## تکنولوژی‌های استفاده‌شده

# 

# \### Backend

# 

# \- ASP.NET Core Web API

# \- Entity Framework Core

# \- SQL Server

# \- Clean Architecture

# \- Swagger / OpenAPI

# \- EF Core Migrations

# 

# \### Frontend

# 

# \- Angular

# \- TypeScript

# \- Standalone Components

# \- Reactive Forms

# \- HttpClient

# \- RTL / Persian UI

# 

# \### Database

# 

# \- Microsoft SQL Server

# 

# \---

# 

# \# معماری پروژه

# 

# Backend با استفاده از \*\*Clean Architecture\*\* پیاده‌سازی شده است تا Business Logic از جزئیات زیرساختی مانند Database و API مستقل باشد.

# 

# ساختار کلی Backend:

# 

# ```text

# SupportTicket

# │

# ├── SupportTicket.Domain

# │   ├── Entities

# │   └── Enums

# │

# ├── SupportTicket.Application

# │   ├── DTOs

# │   ├── Interfaces

# │   └── Services

# │

# ├── SupportTicket.Infrastructure

# │   ├── Persistence

# │   ├── Configurations

# │   └── Repositories

# │

# └── SupportTicket.Api

# &#x20;   ├── Controllers

# &#x20;   └── Program.cs

# ```

# 

# \### Domain

# 

# این لایه شامل مدل‌های اصلی سیستم و قوانین پایه Domain است و به سایر لایه‌ها وابستگی ندارد.

# 

# موجودیت‌های اصلی:

# 

# \- Ticket

# \- TicketStatusHistory

# 

# و Enumهای اصلی:

# 

# \- TicketPriority

# \- TicketStatus

# 

# \### Application

# 

# این لایه شامل Use Caseها و منطق کاربردی سیستم است.

# 

# مسئولیت‌هایی مانند:

# 

# \- ایجاد Ticket

# \- دریافت Ticketها

# \- جستجو

# \- فیلتر

# \- تغییر وضعیت

# \- محاسبه Overdue

# 

# در این لایه قرار می‌گیرند.

# 

# \### Infrastructure

# 

# مسئول ارتباط با سرویس‌های خارجی و زیرساخت است.

# 

# در این پروژه عمدتاً شامل:

# 

# \- Entity Framework Core

# \- SQL Server

# \- DbContext

# \- Repositoryها

# \- Database Configuration

# 

# است.

# 

# \### API

# 

# نقطه ورود Backend است و HTTP Requestها را دریافت می‌کند.

# 

# Controllerها درخواست را دریافت کرده و عملیات مربوطه را از Application Layer فراخوانی می‌کنند.

# 

# این ساختار باعث می‌شود Business Logic مستقیماً به Controller یا SQL Server وابسته نباشد و توسعه و تست پروژه ساده‌تر باشد.

# 

# \---

# 

# \# ساختار Database

# 

# پروژه از SQL Server و Entity Framework Core Code First استفاده می‌کند.

# 

# دو جدول اصلی سیستم عبارت‌اند از:

# 

# ```text

# Tickets

# TicketStatusHistories

# ```

# 

# \## جدول Tickets

# 

# اطلاعات اصلی هر درخواست در این جدول ذخیره می‌شود.

# 

# | ستون | توضیح |

# |---|---|

# | Id | شناسه درخواست |

# | Title | عنوان درخواست |

# | RequesterName | نام درخواست‌کننده |

# | Description | توضیحات |

# | Priority | اولویت درخواست |

# | Status | وضعیت فعلی |

# | CreatedAt | تاریخ ایجاد |

# 

# Priority شامل مقادیر زیر است:

# 

# ```text

# Low

# Medium

# High

# ```

# 

# Status شامل مقادیر زیر است:

# 

# ```text

# Open

# InProgress

# Done

# ```

# 

# \## جدول TicketStatusHistories

# 

# این جدول برای نگهداری تاریخچه تغییر وضعیت Ticket استفاده می‌شود.

# 

# ساختار منطقی آن:

# 

# | ستون | توضیح |

# |---|---|

# | Id | شناسه |

# | TicketId | شناسه Ticket |

# | Status | وضعیت |

# | StartedAt | زمان شروع وضعیت |

# | EndedAt | زمان پایان وضعیت |

# 

# ارتباط بین جداول:

# 

# ```text

# Tickets

# &#x20;  │

# &#x20;  │ 1

# &#x20;  │

# &#x20;  └────────── \*

# &#x20;       TicketStatusHistories

# ```

# 

# یعنی هر Ticket می‌تواند چندین Status History داشته باشد.

# 

# برای مثال:

# 

# ```text

# Ticket #1

# 

# Open

# 08:00 → 18:00

# 

# InProgress

# 18:00 → 02:00

# 

# Open

# 02:00 → 17:00

# ```

# 

# این ساختار برای محاسبه دقیق مدت حضور Ticket در هر وضعیت استفاده می‌شود.

# 

# \---

# 

# \# منطق Overdue

# 

# طبق Requirement پروژه، یک Ticket زمانی Overdue محسوب می‌شود که:

# 

# 1. Priority آن `High` باشد.

# 2. مجموع زمانی که در وضعیت `Open` قرار داشته بیشتر از ۲۴ ساعت باشد.

# 3. مدت حضور Ticket در `InProgress` در این محاسبه لحاظ نشود.

# 

# به همین دلیل صرفاً اختلاف بین `CreatedAt` و زمان فعلی محاسبه نمی‌شود.

# 

# سیستم تاریخچه Statusها را در `TicketStatusHistories` ذخیره می‌کند.

# 

# هنگام تغییر Status، بازه وضعیت قبلی بسته شده و یک History جدید ایجاد می‌شود.

# 

# برای مثال:

# 

# ```text

# 08:00

# Ticket Created

# Status = Open

# 

# &#x20;       ↓ 10 hours

# 

# 18:00

# Status = InProgress

# 

# &#x20;       ↓ 8 hours

# 

# 02:00

# Status = Open

# 

# &#x20;       ↓ 15 hours

# 

# 17:00

# ```

# 

# محاسبه زمان:

# 

# ```text

# Open          10h

# InProgress     8h   ← محاسبه نمی‌شود

# Open          15h

# \-------------------

# Total Open     25h

# ```

# 

# در نتیجه اگر Priority برابر `High` باشد:

# 

# ```text

# 25h > 24h

# ```

# 

# و Ticket به‌عنوان:

# 

# ```text

# Overdue = true

# ```

# 

# برگردانده می‌شود.

# 

# به این ترتیب توقف Ticket در وضعیت `InProgress` باعث مصرف SLA مربوط به Open نمی‌شود.

# 

# \---

# 

# \# API Endpoints

# 

# APIهای اصلی سیستم:

# 

# \### دریافت لیست درخواست‌ها

# 

# ```http

# GET /api/tickets

# ```

# 

# \### جستجو

# 

# ```http

# GET /api/tickets?search=payment

# ```

# 

# جستجو روی:

# 

# \- Title

# \- RequesterName

# 

# انجام می‌شود.

# 

# \### فیلتر بر اساس Status

# 

# ```http

# GET /api/tickets?status=Open

# ```

# 

# \### فیلتر بر اساس Priority

# 

# ```http

# GET /api/tickets?priority=High

# ```

# 

# فیلترها و Search می‌توانند همزمان نیز استفاده شوند:

# 

# ```http

# GET /api/tickets?search=ali\&status=Open\&priority=High

# ```

# 

# \### ایجاد درخواست

# 

# ```http

# POST /api/tickets

# ```

# 

# نمونه Request:

# 

# ```json

# {

# &#x20; "title": "مشکل ورود به سیستم",

# &#x20; "requesterName": "علی احمدی",

# &#x20; "description": "امکان ورود به حساب کاربری وجود ندارد.",

# &#x20; "priority": "High"

# }

# ```

# 

# \### تغییر وضعیت

# 

# ```http

# PATCH /api/tickets/{id}/status

# ```

# 

# نمونه:

# 

# ```json

# {

# &#x20; "status": "InProgress"

# }

# ```

# 

# \---

# 

# \# نحوه اجرای Backend

# 

# \## پیش‌نیازها

# 

# برای اجرای Backend موارد زیر موردنیاز است:

# 

# \- .NET SDK

# \- SQL Server

# 

# ابتدا Connection String را در فایل:

# 

# ```text

# appsettings.json

# ```

# 

# تنظیم کنید.

# 

# نمونه:

# 

# ```json

# {

# &#x20; "ConnectionStrings": {

# &#x20;   "DefaultConnection": "Server=localhost;Database=SupportTicketDb;Trusted\_Connection=True;TrustServerCertificate=True;"

# &#x20; }

# }

# ```

# 

# در صورت استفاده از SQL Server Authentication:

# 

# ```text

# Server=localhost;Database=SupportTicketDb;User Id=sa;Password=YOUR\_PASSWORD;TrustServerCertificate=True;

# ```

# 

# سپس Migrationها را روی Database اعمال کنید:

# 

# ```bash

# dotnet ef database update

# ```

# 

# در صورتی که Startup Project و Infrastructure جدا باشند، دستور متناسب با ساختار Solution اجرا می‌شود، برای مثال:

# 

# ```bash

# dotnet ef database update --project SupportTicket.Infrastructure --startup-project SupportTicket.Api

# ```

# 

# سپس API را اجرا کنید:

# 

# ```bash

# dotnet run --project SupportTicket.Api

# ```

# 

# پس از اجرا، Swagger از آدرس نمایش‌داده‌شده توسط ASP.NET Core قابل دسترسی خواهد بود.

# 

# \---

# 

# \# نحوه اجرای Frontend

# 

# ابتدا وارد پوشه Angular شوید:

# 

# ```bash

# cd support-ticket-ui

# ```

# 

# Dependencyها را نصب کنید:

# 

# ```bash

# npm install

# ```

# 

# آدرس Backend را در Environment پروژه بررسی کنید.

# 

# برای مثال:

# 

# ```typescript

# export const environment = {

# &#x20; apiUrl: 'https://localhost:7162/api'

# };

# ```

# 

# سپس Angular را اجرا کنید:

# 

# ```bash

# ng serve

# ```

# 

# در Windows PowerShell، در صورتی که اجرای `ng.ps1` توسط Execution Policy محدود شده باشد، می‌توان از دستور زیر استفاده کرد:

# 

# ```bash

# ng.cmd serve

# ```

# 

# Frontend به‌صورت پیش‌فرض در آدرس زیر اجرا می‌شود:

# 

# ```text

# http://localhost:4200

# ```

# 

# \---

# 

# \# قابلیت‌های پیاده‌سازی‌شده

# 

# \- نمایش لیست درخواست‌های پشتیبانی

# \- ثبت درخواست جدید

# \- ذخیره اطلاعات در SQL Server

# \- تغییر وضعیت درخواست

# \- جستجو بر اساس عنوان

# \- جستجو بر اساس نام درخواست‌کننده

# \- فیلتر بر اساس Priority

# \- فیلتر بر اساس Status

# \- ثبت تاریخچه تغییر وضعیت

# \- محاسبه مدت واقعی Open بودن درخواست

# \- تشخیص درخواست‌های Overdue

# \- رابط کاربری فارسی و RTL

# \- نمایش تاریخ به‌صورت شمسی

# \- Responsive UI

# \- Swagger برای بررسی APIها

# 

# \---

# 

# \# تصمیمات طراحی

# 

# برای تشخیص Overdue از فیلد ساده‌ای مانند `IsOverdue` در Database استفاده نشده است؛ زیرا Overdue یک مقدار محاسباتی و وابسته به زمان است و ممکن است بدون هیچ تغییری در رکورد Ticket از `false` به `true` تبدیل شود.

# 

# همچنین برای محاسبه مدت Open بودن، تنها از `CreatedAt` استفاده نشده است؛ زیرا طبق Requirement، مدت حضور در `InProgress` نباید محاسبه شود.

# 

# به همین دلیل تاریخچه Statusها نگهداری شده و مجموع بازه‌های `Open` محاسبه می‌شود.

# 

# این طراحی امکان توسعه قوانین SLA در آینده را نیز فراهم می‌کند؛ برای مثال می‌توان زمان مجاز را براساس Priority یا نوع درخواست متفاوت کرد.

