# StoreInventoryAPI

StoreInventoryAPI is a .NET 8-based web API designed to manage the administrative side of a store. It provides CRUD functionalities for managing users and products, along with robust authentication and authorization mechanisms.

## Features

- **User Management**: Create, retrieve, update, and delete users with role-based access control.
- **Product Management**: Perform CRUD operations on products, including search functionality.
- **Authentication & Authorization**: Implements JWT-based authentication and role-based authorization using ASP.NET Identity.
- **Database Integration**: Utilizes Entity Framework Core with MySQL for database interactions.
- **Swagger Integration**: API documentation and testing via Swagger UI.
- **Secure Password Policies**: Enforces strong password requirements and account lockout policies.

## Installation

1. Clone the repository: git clone https://github.com/chief-villager/StoreInventoryAPI.git

2. Navigate to the project directory:cd StoreInventoryAPI

3. Configure the database connection string in `appsettings.json`:"ConnectionStrings": { "DefaultConnection": "Your MySQL Connection String Here" }


4. Apply database migrations:dotnet ef database update

5. Run the application:dotnet run


## Usage

### Endpoints

- **User Management**:
  - `POST /api/User/create`: Create a new user.
  - `GET /api/User/{id}`: Retrieve user details.
  - `DELETE /api/User/{id}`: Delete a user.
	- `POST /api/User/login`: Login a user and retrieve a JWT token.
  - `PUT /api/User/{id}`: Update user details (Admin/Manager only).


- **Product Management**:
  - `POST /api/Product/create`: Add a new product (Admin/Manager only).
  - `PUT /api/Product/{id}`: Update product details (Admin/Manager only).
  - `DELETE /api/Product/{id}`: Delete a product (Admin/Manager only).
  - `GET /api/Product/search/{query}`: Search for products.

- **Authentication**:
  - `POST /api/Auth/login`: Authenticate a user and retrieve a JWT token.

### Swagger UI

Access the Swagger UI for API documentation and testing at:https://localhost:<port>/swagger


## Technologies Used

- **Framework**: .NET 8
- **Database**: MySQL with Entity Framework Core
- **Authentication**: ASP.NET Identity with JWT
- **Authorization**: ASP .NET Role based authorization
- **Mapping**: Mapster
- **Validation** Fluent Validation
- **API Documentation**: Swagger

## Contributing

Contributions are welcome! Please fork the repository and submit a pull request.

## License

This project is licensed under the MIT License.

This README provides a comprehensive overview of the project, including its features, installation steps, usage, and technologies used. Let me know if you need further adjustments!
   