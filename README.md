# Reservation API

The API is designed to manage book reservations, providing an endpoint for all CRUD operations and additional endpoints to manage reservations and reservation history. The API is built with .NET 8 (the fastest and greatest ASP.NET core [yet](https://devblogs.microsoft.com/dotnet/announcing-asp-net-core-in-dotnet-8/#:~:text=the%20fastest%20release-,yet,-!%20Compared%20to%20.NET)), Entity Framework and Postgress SQL with Unit test with Xunit. Some of the keywords and terms associated with this design are swagger UI, Moq, Fluent Validation, Unit, Xunit, Docker, docker-compose, RESTful API, Testing, API documentation, etc

## Features

The API is documented wth swagger UI. it has the following endpoints

```bash

# CRUD
- GET api/v1/books - Get all books
- GET api/v1/books/{id} -Get a book by id
- POST api/v1/books - Create a book
- PUT api/v1/books/{id} -Update a book
- DELETE api/v1/books/{id} -Delete a book

# Others
- POST api/v1/books/reserve - Reserve a book
- POST api/v1/books/remove-reservation - Remove a reservation
- GET api/v1/books/available - Get all available books

- GET api/v1/books/{id}/history - Get reservation history for a book
```

## Prerequisites

- [.NET 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) and above
- [Docker](https://www.docker.com/) (Optional)
- [Postgres](https://www.postgresql.org/) SQl (Optional as it can be run with Docker)

## Getting Started

### 1. Locally with Docker - Recommended

- Clone the repository

```bash
git clone git@github.com:v-limo/reservations.git
```

- Navigate to the project directory

```bash
cd reservations
```

- Run the application with docker compose

```bash
docker compose -f "docker-compose.yml" up -d --build
```

The above command will build the application and run it in a docker container. It wll also run a postgres SQL database in a docker container and connect the application to it.The application will be running on port 5165. You can change the port in the [docker-compose.yml](/docker-compose.yml) file

- API: http://localhost:5165
  -Swagger UI: http://localhost:5165/swagger/index.html
  -Postgres SQL on port 5432 - Username: postgres - Password: MyVeryStrongPassword123!

- Navigate to the swagger UI to test the endpoints

```bash
http://localhost:5165/swagger/index.html
```

### 2. Exclusively as a container

To be able to run the application as a container, you need to have docker installed and **running** on your machine. You can download and install docker from [here](https://www.docker.com/)

- Pull the [image](https://hub.docker.com/r/limov/reservationsapi) from docker hub

```bash
docker pull limov/reservations
```

- Run the image

```bash
docker run -d -p 5165:80 --name reservations vlimo/reservations:latest
```

- The application will be running on port 5165.You can change the port in the "/docker-compose.yml" file
  - API: http://localhost:5165
  - Swagger UI: http://localhost:5165/swagger/index.html

### 3. Running without Docker

- Clone the repository

```bash
git clone git@github.com:v-limo/reservations.git
```

- Navigate to the project directory

```bash
cd reservations
```

- Run the application

```bash
dotnet build

dotnet run --project Reservations.API/Reservations.API.csproj
```
- Navigate to the swagger UI to test the endpoints

  - API: http://localhost:5165
  - Swagger UI: http://localhost:5165/swagger/index.html



```bash
http://localhost:5165/swagger/index.html
```

## Running the tests

```bash
dotnet test
```

## Authors

Vincent Limo - [Github](https://github.com/v-limo/reservations) - [LinkedIn](https://www.linkedin.com/in/vincentlimo/)

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details
