# Egyption_League Web-API

## Description 
- A RESTful Web API using ASP.NET Core to service and manipulate an egyption league system.
- The system API provides a two models [Team, Player].

## Features
- CRUD operations for Team and Player
- Authentication & Authorization with JWT
- Entity Framework Core with SQL Server
- Use Dependency Injection and generic repository patterns
- AutoMapper for DTO Mapping and logging for testing

## Technologies Used
- ASP.NET Core Web API (.NET 8.0)
- Entity Framework Core
- SQL Server
- AutoMapper
- Swagger (API Documentation)
- EFC Logger
- Dependency Injection & Generic repository patterns
- Git & GitHub

## API Endpoints

### **Team API**
| Method | Endpoint                            | Description                       |
|--------|------------------------------------|------------------------------------|
| POST   | /api/teams/AddTeam                 | Add a new team                     |
| POST   | /api/teams/AddTeams                | Add multiple teams                 |
| GET    | /api/teams/GetTeams                | Get all teams                      |
| GET    | /api/teams/GetArchivedTeams        | Get all archived teams             |
| GET    | /api/teams/GetTeam                 | Get a specific team                |
| GET    | /api/teams/GetTeamWithPlayers      | Get a team along with its players  |
| PUT    | /api/teams/UpdateTeam              | Update a team                      |
| PATCH  | /api/teams/ArchiveTeam             | Archive a team                     |
| PATCH  | /api/teams/UnArchiveTeam           | Unarchive a team                   |
| DELETE | /api/teams/DeleteTeam              | Delete a team                      |

### **Player API**
| Method | Endpoint                            | Description                       |
|--------|------------------------------------|------------------------------------|
| POST   | /api/players/AddPlayer             | Add a new player                   |
| POST   | /api/players/AddPlayers            | Add multiple players               |
| GET    | /api/players/GetPlayer             | Get a specific player              |
| GET    | /api/players/GetPlayers            | Get all players                    |
| GET    | /api/players/GetPlayerInDetails    | Get player details                 |
| GET    | /api/players/GetArchivedPlayers    | Get all archived players           |
| PUT    | /api/players/UpdatePlayer          | Update a player                    |
| PATCH  | /api/players/ArchivePlayer         | Archive a player                   |
| PATCH  | /api/players/UnArchivePlayer       | Unarchive a player                 |
| DELETE | /api/players/DeletePlayer          | Delete a player                    |


