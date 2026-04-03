# 🍔 QuickBite – Online Food Ordering System
### .NET 9 · Clean Microservices · Ocelot API Gateway · EF Core · SQL Server

---

## 🏗 Architecture

```
OnlineFoodOrder/
├── src/
│   ├── Shared/FoodOrder.Shared          → Integration events, EventBus, ApiResponse
│   ├── Services/
│   │   ├── Identity.API    :5001        → JWT auth, BCrypt, user management
│   │   ├── Restaurant.API  :5002        → Restaurants, menus, categories
│   │   ├── Order.API       :5003        → Order lifecycle, status history
│   │   ├── Payment.API     :5004        → Payment processing, UPI/Card/COD
│   │   ├── Delivery.API    :5005        → Driver assignment, GPS tracking
│   │   └── Notification.API             → BackgroundService, event consumer
│   ├── Gateway             :5000        → Ocelot reverse proxy + JWT
│   └── FoodOrder.Web       :5010        → ASP.NET Core MVC client
└── OnlineFoodOrder.sln
```

<img width="1536" height="1024" alt="image" src="https://github.com/user-attachments/assets/6d721394-3bc8-4882-be26-174bef66dd7e" />


---

## 🗄 Databases  (one per service — true isolation)

| Database                    | Tables                                              |
|-----------------------------|-----------------------------------------------------|
| FoodOrder_IdentityDB        | Users                                               |
| FoodOrder_RestaurantDB      | Restaurants, Categories, MenuItems                  |
| FoodOrder_OrderDB           | Orders, OrderItems, OrderStatusHistories             |
| FoodOrder_PaymentDB         | Payments                                            |
| FoodOrder_DeliveryDB        | Deliveries                                          |
| FoodOrder_NotificationDB    | NotificationLogs                                    |

> **No migrations needed.** Each service calls `db.Database.EnsureCreated()` on startup,
> which creates the schema and applies all `HasData()` seed automatically.

---

## 👥 Demo Accounts  (password: `Food@1234`)

| Role            | Email               |
|-----------------|---------------------|
| Admin           | admin@food.com      |
| Customer        | rahul@food.com      |
| Customer        | priya@food.com      |
| RestaurantOwner | owner@food.com      |
| Driver          | driver@food.com     |

---

## 🚀 Quick Start

### Prerequisites
- .NET 9 SDK — https://dotnet.microsoft.com/download
- SQL Server (LocalDB ships with Visual Studio, or Express/full)

### 1. Open solution
```
OnlineFoodOrder.sln
```

### 2. Run services (7 terminals or use VS multiple startup)

```bash
# Terminal 1
cd src/Services/Identity.API    && dotnet run   # https://localhost:5001

# Terminal 2
cd src/Services/Restaurant.API  && dotnet run   # https://localhost:5002

# Terminal 3
cd src/Services/Order.API       && dotnet run   # https://localhost:5003

# Terminal 4
cd src/Services/Payment.API     && dotnet run   # https://localhost:5004

# Terminal 5
cd src/Services/Delivery.API    && dotnet run   # https://localhost:5005

# Terminal 6 (background worker)
cd src/Services/Notification.API && dotnet run

# Terminal 7 — Gateway
cd src/Gateway                  && dotnet run   # https://localhost:5000

# Terminal 8 — MVC Client
cd src/FoodOrder.Web            && dotnet run   # https://localhost:5010
```

### 3. Browse
Open **https://localhost:5010** and sign in with any demo account above.

### Swagger UIs
| Service       | URL                                |
|---------------|------------------------------------|
| Identity      | https://localhost:5001/swagger     |
| Restaurant    | https://localhost:5002/swagger     |
| Order         | https://localhost:5003/swagger     |
| Payment       | https://localhost:5004/swagger     |
| Delivery      | https://localhost:5005/swagger     |

---

## 📡 Gateway Routes

| Gateway path                      | Downstream                |
|-----------------------------------|---------------------------|
| /gateway/auth/**                  | Identity :5001            |
| /gateway/restaurants/**           | Restaurant :5002          |
| /gateway/orders/**                | Order :5003               |
| /gateway/payments/**              | Payment :5004             |
| /gateway/deliveries/**            | Delivery :5005            |

---

## 🌊 Order Flow

```
Customer → Web → Gateway → Restaurant API  (browse menu)
                         → Order API        (place order)  → [OrderPlacedEvent]
                         → Payment API      (pay)          → [PaymentSucceededEvent]
                         → Order API        (confirm)      → [OrderStatusChangedEvent]
                         → Delivery API     (assign driver)→ [DeliveryAssignedEvent]
                         → Delivery API     (delivered)    → [DeliveryCompletedEvent]

All events → Notification.API (logs to NotificationDB)
```

---

## 📦 Key NuGet Packages

| Package                                | Purpose                  |
|----------------------------------------|--------------------------|
| Microsoft.EntityFrameworkCore.SqlServer 9.0 | ORM + SQL Server    |
| Microsoft.AspNetCore.Authentication.JwtBearer 9.0 | JWT Bearer auth |
| BCrypt.Net-Next 4.0.3                  | Password hashing         |
| Ocelot 23.4.2                          | API Gateway routing      |
| Swashbuckle.AspNetCore 7.2.0           | Swagger per service      |
| System.IdentityModel.Tokens.Jwt 8.3.0  | JWT token generation     |

---

## 🔑 Seed Data Highlights

- **3 restaurants** — Spice Garden (South Indian), Pizza Hub (Italian), Biryani Palace (Mughlai)
- **6 categories** — Starters, Main Course, Pizzas, Pasta, Biryani, Curries
- **15 menu items** — with prices, veg/non-veg, tags, popular flags
- **2 seeded orders** — one Delivered, one OutForDelivery
- **2 seeded payments** — UPI + Card both Completed
- **2 seeded deliveries** — one Delivered, one InTransit

---

## ✅ Key Design Decisions

| Decision | Rationale |
|---|---|
| `EnsureCreated()` instead of migrations | Zero setup — no PM Console, no migration files needed |
| One DB per service | True data isolation — no cross-service joins |
| `InMemoryEventBus` | Swap for RabbitMQ/Azure Service Bus by changing one DI line |
| Session-based cart | No DB writes; cleared on successful order |
| Shared JWT key | All services validate the same token issued by Identity API |
| No `MigrationsAssembly()` | Eliminates the `Delivery.Service.dll not found` error |
