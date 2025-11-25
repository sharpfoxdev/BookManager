# BookManager

Example full stack ASP.NET Core and Blazor Server application for borrowing and returning books. Uses Clean Architecture, EF COre, SQLite, Swagger for REST API. 

## How to run

- Either in development mode using Microsoft Aspire for local orchestration
	- Can run both API and frontend simultaneously - Set AppHost as starting project
- App halso has dockerfiles and docker-compose.yml, so quick `docker compose up --build` will get the app running (both apiservice (backend) and frontend)
- SQLite database has volume mapped in docker compose, the SQLite files are stored in the apiservice (backend) container in `/app/data`

## Project structure

### Aspire
- BookManager.AppHost - orchestration from aspire
- BookManager.ServiceDefaults - extension methods for Aspire 

### Backend
- Follows [clean architecture principles](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures#clean-architecture)
- BookManager.Core - domain entities and interfaces (repositories, services)
- BookManager.Infrastructure - implementation of interfaces from Core, DbContext setup, migrations
- BookManager.ApiService - serves REST API (there is Swagger available), handles dependency injection, automapper mappings

### Frontend 
- BookManager.Web - Blazor Server frontend, uses REST API to get data from backend
- BookManager.Shared - contains DTOs for REST API, that are shared by both frontend and backend