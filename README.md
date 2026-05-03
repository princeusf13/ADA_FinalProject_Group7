# AI Tutor Platform 🎓🤖

A modern, full-stack Learning Management System (LMS) built with **ASP.NET Core 10** and deployed on **Azure**. This platform bridges the gap between structured course content and interactive student engagement through a context-aware tutoring interface and gamified analytics.

---

## 🌟 Key Features

### 👨‍🏫 Curriculum Management (CRUD)
- **Course & Topic Hierarchy:** Professors can create and manage academic structures.
- **Material Repository:** Upload and organize assignments and quizzes linked to specific topics.
- **Role-Based Access Control (RBAC):** Granular permissions for Admins, Professors, and Students using **ASP.NET Core Identity**.

### 🎮 Gamified Learning
- **Computer Science Trivia:** Real-time integration with the **OpenTrivia Database API**.
- **Global Leaderboard:** Track and visualize top student performers across the platform.

### 📊 Behavioral Analytics
- **Engagement Tracking:** Visualizes AI interaction frequency over a 7-day rolling window.
- **Curriculum Insights:** Bar charts showing topic distribution across various courses using **Chart.js**.

### 🎨 Modern UI/UX
- **Glassmorphism Theme:** A clean, translucent interface design with custom CSS backdrop filters.
- **Dynamic Wizard:** A multi-step jQuery/AJAX-powered onboarding flow for the AI learning module.

---

## 🛠️ Technical Stack

- **Framework:** ASP.NET Core 10 (MVC & Razor Pages)
- **Database:** Azure SQL Database
- **ORM:** Entity Framework Core (Code-First)
- **Frontend:** Bootstrap 5, jQuery, Chart.js
- **Cloud/DevOps:** Azure App Service, GitHub Actions (CI/CD)

---

## 🔌 External API Integration

The platform consumes the following third-party service:
- **API:** [Open Trivia Database](https://opentdb.com/api_config.php)
- **Implementation:** Uses `IHttpClientFactory` for asynchronous, non-blocking requests to fetch Computer Science questions (Category 18).

---

## 📐 Project Architecture

The application follows an **N-Tier Architecture**:
1. **Presentation Layer:** Razor Views and custom CSS/JS for the Glassmorphism theme.
2. **Logic Layer:** Controllers and Services handling Business Logic and Identity Security.
3. **Data Layer:** EF Core managing the relational schema and migrations in Azure SQL.

---

## 🚀 Deployment & CI/CD

This project is fully automated using **GitHub Actions**. 
1. **Build:** Triggered on every push to the `main` branch.
2. **Security:** Production connection strings are injected via **Azure Environment Variables**.
3. **Deploy:** Automated delivery to **Azure App Service**, ensuring the live site is always up to date.

---

## 👥 The Team (USF Muma College of Business)

- **Prince Praveen:**
- **Sushma Swaraj Padala:** 
- **Harshitha Neerudu:** 
- **Koushik Devalapalli:** 

---

## 📝 Setup & Installation

1. **Clone the repo:** `git clone https://github.com/your-username/ai-tutor.git`
2. **Update Database:** Run `dotnet ef database update` in the Package Manager Console.
3. **Run Application:** `dotnet watch run`

---
*Developed as a Graduate Project at the University of South Florida.*
