# Reservation API

The API is designed to manage book reservations, providing an endpoint for all CRUD operations and additional endpoints to manage reservations and reservation history. The API is built with .NET 8 (the fastest and greatest ASP.NET core [yet](https://devblogs.microsoft.com/dotnet/announcing-asp-net-core-in-dotnet-8/#:~:text=the%20fastest%20release-,yet,-!%20Compared%20to%20.NET)), Entity Framework and  SQlite with Unit test with Xunit. Some of the keywords and terms associated with this design are swagger UI, Moq, SQlite, Fluent Validation, Unit, Xunit, Docker, docker-compose, RESTful API, Testing, API documentation, etc

## Features

The API is documented wth Swagger UI. it has the following endpoints


- GET    `api/v1/books`      - Get all books
- GET    `api/v1/books/{bookId}` - Get a book by id
- POST   `api/v1/books`      - Create a book
- PUT    `api/v1/books/{bookId}` - Update a book
- DELETE `api/v1/books/{bookId}` - Delete a book

- POST  `api/v1/books/{bookId}reserve/{comment}`   - Reserve a book
- POST  `api/v1/books/{bookId}/remove-reservation` - Remove a reservation
- GET   `api/v1/books/available-books`             - Get all available books

- GET   `api/v1/books/{bookId}/history`            - Get reservation history for a book

## Prerequisites

- [.NET 8.0.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) and above
- [Docker](https://www.docker.com/) (Optional but reccommended)

## Getting Started

### 1. Locally with Docker - Recommended

```bash
docker run -p 5165:5165 --name webapi  limov/reservationsapi
```

The above command will pull the [image](https://hub.docker.com/r/limov/reservationsapi) from docker hub and run it on port 5165.
  - API: http://localhost:5165
  - Swagger UI: http://localhost:5165/swagger/index.html


### 2. Running without Docker

```bash
# Clone
git clone git@github.com:v-limo/reservations.git

# - Navigate to project
cd reservations

# - Build projects
dotnet build && dotnet restore && dotnet test

# Run project
dotnet run --project Reservations.API/Reservations.API.csproj

# Navigate to the port running the project:
http://localhost:NNNN/swagger/index.html
```

## Running the tests

```bash
dotnet test
```

## Authors

Vincent Limo - [Github](https://github.com/v-limo/reservations) - [LinkedIn](https://www.linkedin.com/in/vincentlimo/)

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details
