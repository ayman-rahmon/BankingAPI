
# BankingAPI Project

## Overview

This project provides a simple API for managing clients and authentication in a banking system. It includes:
- Authentication (registration and login) with JWT token generation.
- Client management (CRUD operations) with filtering, sorting, and pagination features.

**Note:** This project has been tested on **Arch Linux**, **Windows**, and **Debian Linux**.

## Prerequisites

Ensure you have the following installed:

- **.NET SDK 8.0**
  - Download for Windows from the official .NET website.
  - For Arch Linux, use the command: sudo pacman -S dotnet-sdk
  - For Debian Linux, first run: sudo apt update, then install using: sudo apt install dotnet-sdk-8.0

- **SQL Server**
  - Download for Windows from the official SQL Server website.
  - For Debian Linux, follow the instructions on the Microsoft SQL Server Linux documentation page.
  - For Arch Linux, use a remote or containerized SQL Server.

- **Redis** (for caching)
  - Download for Windows from the Microsoft Redis GitHub repository.
  - For Arch Linux, use the command: sudo pacman -S redis
  - For Debian Linux, install it using the command: sudo apt install redis-server

- **Postman** (for testing the API)
  - Download for all platforms from the official Postman website.

## Setup Instructions

### 1. Clone the repository

```
Clone the project repository and navigate to the project directory:
git clone https://github.com/your-repo-name.git
cd BankingAPI
```

### 2. Setup the Database

Ensure that SQL Server is running locally or update the connection string in the `appsettings.json` file to point to your server:
Update "DefaultConnection" with your connection details (server, database, user ID, and password).

### 3. Setup Redis (For Caching)

#### Redis Installation on Arch Linux

Install Redis with the command: 

```
sudo pacman -S redis
```

#### Redis Installation on Debian Linux

Install Redis with the command: 

```
sudo apt install redis-server
```

#### Redis Installation on Windows

Download and extract the Redis server for Windows. Run the `redis-server.exe` file to start the Redis server.

For all environments, ensure Redis is running on `localhost:6379` as configured in `appsettings.json`.

### 4. Build the Project

To build the project, restore dependencies and build the solution:
Run: 

```
dotnet restore
```
 followed by 

```
dotnet build
```

### 5. Apply Migrations

Update the database schema by running migrations:
Run:

```
dotnet ef database update
```

### 6. Running the Project

Run the API with the following command:
Run: 

```
dotnet run --project BankingAPI.WebAPI
```

The API will be available at: http://localhost:5292 (the localhost url will change for every machine so pay attention to the terminal output).

## Testing the API

### Postman Collections

Two Postman collections are included for testing the API:

1. **AuthControllerTest**: Tests for user registration and login.
2. **ClientControllerTest**: Tests for client CRUD operations, filtering, sorting, and pagination.

### Importing Postman Collections

1. Open Postman.
2. Import the collections:
    - `AuthControllerTest.json`
    - `ClientControllerTest.json`
   
In Postman, click the **Import** button and select the JSON files.

### 1. Testing the Authentication Endpoints

- **Register User**: Registers a new user (email, password, role).
- **Login User**: Logs in the user and generates a JWT token.

After logging in, copy the token and use it in subsequent requests as a **Bearer Token** in the **Authorization** tab in Postman.

### 2. Testing the Client Management Endpoints

- **Add Client**: Adds a new client (Admin only).
- **Get Clients**: Retrieves a paginated list of clients with optional filtering and sorting.
- **Get Search Suggestions**: Retrieves the last three search filters used by Admin.
- **Save Search and Pagination**: Saves filtering and pagination parameters.

#### Sample Request: Get Clients with Filtering, Sorting, and Pagination

To retrieve clients filtered by the first name "John", sorted by last name, with 3 results per page (starting at page 1):
GET http://localhost:5292/api/client?filterBy=firstName&searchValue=John&sortBy=lastName&pageIndex=1&pageSize=3

## License

This project is licensed under the MIT License. See the LICENSE file for details.
