# Reservation API

The API is designed to manage book reservations, providing an endpoint for all CRUD operations and additional endpoints
to manage reservations and reservation history. The API is built with .NET 8 (the fastest and greatest ASP.NET
core [yet](https://devblogs.microsoft.com/dotnet/announcing-asp-net-core-in-dotnet-8/#:~:text=the%20fastest%20release-,yet,-!%20Compared%20to%20.NET)),
Entity Framework and SQlite with Unit test with Xunit. Some of the keywords and terms associated with this design are
swagger UI, Moq, SQlite, Fluent Validation, Unit, Xunit, Docker, docker-compose, RESTful API, Testing, API
documentation, Container orchestration with kubernetes etc.

## Features

The API is documented with Swagger UI. It has the following endpoints

- GET    `api/v1/books`      - Get all books
- GET    `api/v1/books/{bookId}` - Get a book by id
- POST   `api/v1/books`      - Create a book
- PUT    `api/v1/books/{bookId}` - Update a book
- DELETE `api/v1/books/{bookId}` - Delete a book

- POST  `api/v1/books/{bookId}reserve/{comment}`   - Reserve a book
- POST  `api/v1/books/{bookId}/remove-reservation` - Remove a reservation
- GET   `api/v1/books/available-books`             - Get all available books

- GET   `api/v1/books/{bookId}/history`            - Get reservation history for a book

[## Prerequisites

- [.NET 8.0.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) and above
- [Docker](https://www.docker.com/), Kubernetes (Optional but recommended)

## Getting Started - Running the project

### 1. Locally with Docker - Recommended

<details open>
<summary>with Dockers</summary>

```bash
docker run -p 5099:5099 --name webapi  limov/reservationsapi:latest

# Access the API at: http://localhost:5099/swagger/index.html
```
</details>


### 2. Locally with Kubernetes — Recommended

<details >
<summary>with Kubernetes</summary>

```bash
# Clone the repository
git clone git@github.com:v-limo/reservations.git

# Navigate to project folder
cd reservations

# Apply kubernetes configuration files
kubectl apply -f kubernetes/deployment.yaml -f kubernetes/service.yaml

# Validate services, pods, deployments, and replica sets
kubectl get svc,pods,deployments,rs

# Get Minikube IP
minikube ip

# Expose the service - automatically open the app on the default
minikube service api-service

# IMPORTANT: keep the terminal open to maintain the service.
```
</details>

### 2. Running locally

<details >
<summary>Running locally</summary>

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
</details >

## Running the tests

```bash
dotnet test
```

## Authors

Vincent Limo - [Github](https://github.com/v-limo/reservations) - [LinkedIn](https://www.linkedin.com/in/vincentlimo/)

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details