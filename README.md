# Ambev Developer Evaluation Project

## Overview
This project is a backend solution for managing e-commerce operations, including user management, product catalog, checkout, and order processing. It is built using .NET 8 and follows a modular architecture.

## Prerequisites
- .NET 8 SDK
- Docker installed on your operating system ([Download Docker Desktop](https://www.docker.com/products/docker-desktop/))
- A terminal or command-line interface

## Setup Instructions

1. Clone the repository:git clone <https://github.com/brunocfigueira/developer-evaluation-project.git>
cd developer-evaluation-project/template/backend
2. Configure the database connection string:
   - Navigate to the `appsettings.json` file in the `Ambev.DeveloperEvaluation.WebApi` project.
   - Update the `ConnectionStrings` section with your PostgreSQL database credentials.

3. Start the required containers using Docker Compose:
   - **run**: 'docker-compose up -d' (This command will start the following services as defined in `docker-compose.yml`):
   - **ambev.developerevaluation.webapi**: The main Web API application (ports 8080, 8081)
   - **ambev.developerevaluation.database**: PostgreSQL database (port 5432)
   - **ambev.developerevaluation.nosql**: MongoDB NoSQL database (port 27017)
   - **ambev.developerevaluation.cache**: Redis cache (port 6379)

4. Apply database migrations:
   - **run**: 'dotnet ef database update --project src/Ambev.DeveloperEvaluation.ORM/Ambev.DeveloperEvaluation.ORM.csproj --startup-project src/Ambev.DeveloperEvaluation.WebApi/Ambev.DeveloperEvaluation.WebApi.csproj'
## Running the Project

1. Navigate to the Web API project directory:cd src/Ambev.DeveloperEvaluation.WebApi
2. Run the application:dotnet run
3. The API will be available at `http://localhost:7181` by default.

## API Documentation
- The Swagger UI for testing the API is available at: [https://localhost:7181/swagger/index.html](https://localhost:7181/swagger/index.html)

## Main Sales Flow
To execute a complete sale, follow these steps using the API endpoints (see Swagger for details):

1. **Create a User**
   - Endpoint: `POST /api/Users`
   - Register a new user with the required information.
2. **Create Products**
   - Endpoint: `POST /api/Products`
   - Add one or more products to the catalog.
3. **Create a Shopping Cart**
   - Endpoint: `POST /api/Carts`
   - Create a cart and add products to it.
4. **Send Cart to Checkout**
   - Endpoint: `POST /api/Carts/{id}/StartCheckout`
   - Initiate the checkout process for the cart.
5. **Confirm the Order**
   - Endpoint: `POST /api/Checkout/ConfirmOrder`
   - Confirm the order and trigger payment and fulfillment flows.
6. **Search Consolidated Sales**
   - Endpoint: `GET /api/Sales` (or similar, see Swagger for the exact route)
   - Retrieve a list of completed sales/orders.

## Testing the Project

### Unit Tests
1. To run unit tests, navigate to the `tests/Ambev.DeveloperEvaluation.Unit` directory and execute:
   - 'dotnet test' 
2. To run integration tests, navigate to the `tests/Ambev.DeveloperEvaluation.Integration` directory and execute:
   - 'dotnet test' 
3. To run functional tests, navigate to the `tests/Ambev.DeveloperEvaluation.Functional` directory and execute:
   - 'dotnet test'
## Key Features
- User management with roles and permissions.
- Product catalog with CRUD operations.
- Checkout process with order confirmation.
- Modular architecture for scalability.

## Additional Notes
- Ensure Docker is running and all containers are healthy before applying migrations or starting the application.
- Use tools like Postman or Swagger to test the API endpoints interactively.
- Log records are in the `src\Ambev.DeveloperEvaluation.WebApi\logs` directory

## Contribution
Feel free to fork the repository and submit pull requests for any improvements or bug fixes.