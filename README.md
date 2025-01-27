# API Blog

A .NET Core Web API application built for managing a simple Blog Api.

## API Endpoints

The following are the main endpoints of the Todo API:

* GET /api/post - Retrieve all Post items.
* GET /api/post/{id} - Retrieve a specific Post item by ID.
* POST /api/post - Create a new Post item
* PUT /api/post/{id} - Update an existing Post item that is in editable state.
* POST /api/post/publish - Publish a new Post item
* DELETE /api/post/{id} - Delete a Post item

## Prerequisites

* .NET 8.0 or later
* Docker

## Running Locally

### 1. Build and Run with Docker Compose

To build and run the application using Docker compose, run the following command:

```sh
docker compose -f compose.yml up --build -d
```

This will start up the application and a SQL Server database instance. The API will be accesible at http://localhost:5000

### 2. Debugging

To run the project in Debug mode:

1. Open the solution in Visual Studio or Rider.
2. Set ApiBlog.WebApi as the startup project.
3. Run MongoDB with docker compose using compose-debug.yml file
```sh
docker compose -f compose-debug.yml up -d
```
4. Press F5 to Start the application in debug mode.

This will start up the application and open the browser at https://localhost:7280/swagger/index.html

## Dependencies

* .NET 8
* MongoDB
* Docker
* Docker Compose

