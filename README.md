# LeadForge

AI-powered LinkedIn post generator built with **.NET 8 and Next.js**.

LeadForge helps founders and developers generate high-quality LinkedIn
posts designed to drive engagement and inbound leads.

------------------------------------------------------------------------

## Features

-   AI-powered LinkedIn post generation
-   Authentication with JWT + Refresh Tokens
-   Credits system
-   Generation history with pagination
-   Rate limiting for AI requests
-   FluentValidation request validation
-   Global exception handling middleware
-   Modern React frontend with React Query
-   Responsive SaaS dashboard UI

------------------------------------------------------------------------

## Tech Stack

### Backend

-   .NET 8
-   ASP.NET Core Web API
-   Entity Framework Core
-   PostgreSQL
-   FluentValidation
-   Serilog
-   JWT Authentication
-   OpenAI API

### Frontend

-   Next.js
-   React
-   React Query
-   TailwindCSS
-   shadcn/ui

------------------------------------------------------------------------

## Architecture

The backend follows a **Clean Architecture inspired structure**.

    API
    │
    Application
    │
    Domain
    │
    Infrastructure

Responsibilities:

-   **API** → controllers and middleware\
-   **Application** → business logic\
-   **Domain** → entities and domain rules\
-   **Infrastructure** → database and external services

------------------------------------------------------------------------

## Screenshots

### Login

![Login](docs/login.png)

### Dashboard

![Dashboard](docs/dashboard.png)

### Generate Post

![Generate](docs/generate.png)

------------------------------------------------------------------------

## Demo Account

You can use the demo account:

email: demo@leadforge.ai\
password: demo123

------------------------------------------------------------------------

## Running locally

Clone repository:

git clone https://github.com/yourname/leadforge

### Backend

dotnet run

### Frontend

npm install\
npm run dev

------------------------------------------------------------------------

## Project Structure

    /LeadForge
      /docs
        login.png
        dashboard.png
        generate.png
      /LeadForge.Api
      /LeadForge.Application
      /LeadForge.Domain
      /LeadForge.Infrastructure
      /frontend

------------------------------------------------------------------------

## Future Improvements

-   Payments with Stripe
-   Subscription plans
-   Redis caching
-   Analytics dashboard
