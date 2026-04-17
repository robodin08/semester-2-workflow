# Workflow

This document describes how to set up and run the `Workflow.Web` application in a local development environment.

## Requirements

- .NET 10 SDK
- A running MySQL server

## Configurations

The application requires the following configuration values to be provided in your environment:

| Key | Description | Example |
|-----|------------|---------|
| `ConnectionStrings:DefaultConnection` | MySQL connection string used by the application | `server=localhost;port=3306;database=workflow;user=your_user;password=your_password;` |
| `Turnstile:SecretKey` | Secret key used for server-side Turnstile validation | `your_secret_key` |
| `Turnstile:SiteKey` | Public site key used on the client side | `your_site_key` |

## Running the Application

```pwsh
dotnet restore Workflow.sln
dotnet run --project ./Workflow.Web/Workflow.Web.csproj
```