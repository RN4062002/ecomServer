# ⚙️ ecomServer

The robust backend RESTful API powering an advanced E-Commerce application. This server handles business logic, relational data storage, secure user authentication, order processing pathways, and inventory status tracking.

---

## 🚀 Features

* **Secure Authentication:** Identity management utilizing JWT (JSON Web Tokens) with secure password hashing and role-based access control (Admin vs. Customer).
* **Product & Catalog Management:** Full CRUD operations for managing product inventories, variants, categories, and stock updates.
* **Shopping Cart & Checkout Logic:** Server-side validation for processing orders, stock availability verification, and transactional workflows.
* **Order Tracking:** Persistent storage and management of user orders, invoice generation data, and payment status updates.
* **Data Validation & Security:** Strict request payload validation layers and optimization routines for handling concurrent database sessions.

---

## 🛠️ Tech Stack

* **Core Framework:** .NET Core Web API (C#)
* **Database Engine:** Microsoft SQL Server
* **ORM:** Entity Framework Core (EF Core)
* **Authentication:** JWT (JSON Web Tokens)

---

## 📦 Installation & Setup

Follow these steps to configure and run the backend development server on your local environment.

### Prerequisites
Make sure you have the following installed on your machine:
* [.NET 8.0 SDK](https://dotnet.microsoft.com/download) (or your specific runtime environment)
* [SQL Server Management Studio (SSMS)](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms) or local SQL Express instance
* Git

### Step-by-Step Guide

1. **Clone the Repository**
   ```bash
   git clone [https://github.com/RN4062002/ecomServer.git](https://github.com/RN4062002/ecomServer.git)
   cd ecomServer
