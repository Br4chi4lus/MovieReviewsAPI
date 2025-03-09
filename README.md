## Table of contents
* [About the project](#About-the-project)
* [Technologies](#Technologies)
* [Setup](#Setup)
* [Database](#Database)

## About the project
Simple REST API to manage movie reviews.
## Technologies
* ASP .NET Core 8.0
* Entity Framework
* Docker
## Setup
Clone this repository:
```
git clone https://github.com/Br4chi4lus/MovieReviewsAPI.git
```
Run docker containers:
```
cd MovieReviewsAPI
docker compose up
```
Install dotnet ef CLI and update database:
```
dotnet tool install --global dotnet-ef
dotnet ef database update
```
## Database
Currently changing role of the user is possible using database only. PgAdmin email and password can be found/changed in docker-compose.yml.
Connecting to database with pgAdmin in given docker-compose.yml:
* Host - db
* user - postgres
* password - password
## API Documentation
The full API documentation is available via Swagger.  
After running the project, open:
 **[http://localhost:8080/api/docs](http://localhost:8080/api/docs)**
