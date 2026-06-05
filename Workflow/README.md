# Workflow

This document describes how to set up and run the `Workflow.Web` application in a local development environment.

## Requirements

- .NET 10 SDK
- A running MySQL server

## Configuration

## Configuration

Copy the example configuration file for your environment:

```pwsh
cp appsettings.example.json appsettings.Development.json
```

## Running the Application

```pwsh
dotnet restore Workflow.sln
dotnet run --project ./Workflow.Web/Workflow.Web.csproj
```