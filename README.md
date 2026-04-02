# SimpleAuth 

![Animation](https://github.com/user-attachments/assets/bd374223-0e7a-4dd5-978e-3888d1160d8e)



SimpleAuth is a production-ready ASP.NET Core 6 Web API that demonstrates secure Google OAuth 2.0 authentication integrated with cookie-based session handling and SPA (Single Page Application) workflows.

This project is designed with enterprise-grade principles including security, scalability, and clean separation between backend and frontend responsibilities.

---

# 🧭 Overview

SimpleAuth provides a robust authentication flow using Google as an identity provider and integrates seamlessly with frontend applications (e.g., Angular).

It implements:

- OAuth 2.0 Authorization Code Flow (via Google)
- Cookie-based session handling on backend
- Token handoff to SPA (JWT placeholder)
- Redirect-based authentication flow
- Secure logout handling

---

# 🏗️ Architecture

- Backend: ASP.NET Core 6 Web API (C# 10)
- Authentication:
  - Google OAuth 2.0
  - Cookie Authentication (server-side session)
- Frontend (optional): Angular (http://localhost:4200)
- Communication:
  - Browser-based redirects (OAuth flow)
  - Query string token exchange (SPA handoff)

---

# 🔐 Authentication Flow (Detailed)

1. User clicks "Login with Google" in frontend
2. Frontend redirects browser to:
   /auth/google/login
3. Backend triggers OAuth challenge
4. User authenticates on Google
5. Google redirects to:
   /signin-google (internal ASP.NET middleware endpoint)
6. Middleware validates identity and creates ClaimsPrincipal
7. Backend redirects to:
   /auth/google/callback-success
8. Backend:
   - Extracts claims (email, name)
   - Generates application token (JWT placeholder)
   - Redirects to frontend with token and user info

---

# 📌 Endpoints

## GET /auth/google/login
Initiates Google OAuth authentication flow.

## GET /auth/google/callback-success
Handles post-authentication logic:
- Validates authenticated user
- Extracts claims
- Generates token (placeholder)
- Redirects to frontend with query parameters

## GET /auth/login
Returns current authenticated user session info.

## GET /auth/logout
Signs out user from application:
- Clears authentication cookies
- Redirects to frontend

---

# 📁 Project Structure

/Controllers
  AuthController.cs

/Properties
  launchSettings.json

Program.cs
appsettings.json

---

# ⚙️ Prerequisites

- .NET 6 SDK
- Google OAuth 2.0 credentials
  - https://console.cloud.google.com/apis/credentials?hl=pt-br
    - Configure: Origens JavaScript:
      http://localhost:4200 (etc...)
    - Redirect URI:
      https://localhost:5001/signin-google (etc)
- Frontend (optional): Angular

---

# 🛠️ Setup

## 1. Clone repository

git clone https://github.com/yourusername/SimpleAuth.git
cd SimpleAuth

## 2. Configure Google OAuth

Go to:
https://console.cloud.google.com/

Create credentials:

Authorized JavaScript Origins:
http://localhost:4200

Authorized Redirect URIs:
https://localhost:5001/signin-google

## 3. Configure appsettings.json

{
  "Authentication": {
    "Google": {
      "ClientId": "YOUR_CLIENT_ID",
      "ClientSecret": "YOUR_CLIENT_SECRET"
    }
  }
}

## 4. Restore dependencies

dotnet restore

## 5. Run

dotnet run

---

# 🧪 Usage

Frontend login trigger:

window.location.href = 'https://localhost:5001/auth/google/login';

---

# 🔁 Callback Handling

Redirect flow:

Backend → Google → Backend → Frontend

Frontend receives:

/auth/social-callback?token=...&email=...&name=...

---

# 🚪 Logout

GET /auth/logout

---

# 🔄 Account Switching

Google session reuse is default behavior.

To force account selection:

prompt=select_account

Implemented via redirect event in .NET 6.

---

# 🔐 Security Considerations

- Validate returnUrl to prevent open redirect attacks
- Store secrets securely (Environment Variables / Key Vault)
- Enforce HTTPS
- Do not expose tokens in logs
- Implement proper JWT validation in production

---

# 🧩 Production Enhancements

Recommended improvements:

- JWT with expiration + refresh tokens
- Role-based authorization
- User persistence (database)
- Logging (Serilog / Application Insights)
- Rate limiting
- Health checks
- Docker support

---

# 📊 Observability (Recommended)

- Structured logging
- Distributed tracing
- Metrics (Prometheus / Azure Monitor)

---

# 🚀 Deployment Strategy

- Use environment-based configuration:
  - Development
  - Staging
  - Production
- Store secrets outside codebase
- Use CI/CD pipelines (Azure DevOps / GitHub Actions)

---

# 📄 License

MIT

---

# ✅ Summary

SimpleAuth provides a clean and extensible foundation for:

- Google authentication
- SPA integration
- Secure backend session handling
- Enterprise-ready architecture patterns

