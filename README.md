# TinyURL

TinyURL is a straightforward URL shortening service developed using **.NET 8** and adhering to principles of **Clean Architecture**, **Domain Driven Design (DDD)**, and **Test Driven Development (TDD)**. This project enables users to create shortened URLs, retrieve original URLs, delete shortened URLs, and monitor usage statistics.

## Prerequisites

- **.NET 8**
- **Visual Studio 2022** (Optional)

## Getting Started

### Running the Application from Visual Studio 2022

1. **Open the Solution File**:
    - Open the solution file `TinyURL.sln` in Visual Studio 2022.

2. **Set the Startup Project**:
    - In the Solution Explorer, right-click on the `TinyURL.Presentation` project and select `Set as Startup Project`.

3. **Run the Project**:
    - Press `F5` or click the `Run` button to start the application.

### Using .NET CLI
1. **Open the CLI**
2. **Go to directory:**  `...\TinyURL\src\TinyURL.Presentation`
3. **Run command:**   `dotnet run`

### Running xUnit Tests Using Visual Studio

The unit tests for this project are located in the `TinyURL\tests\TinyURL.Presentation.UnitTests` directory. Follow the steps below to run these tests using **Visual Studio 2022**:

### 1. Open the Solution File

- Open the solution file `TinyURL.sln` in **Visual Studio 2022**.

### 2. Open Test Explorer

- Go to **`Test > Test Explorer`** from the menu to open the **Test Explorer** window.

### 4. Run the Tests

- In the **Test Explorer**, you should see a list of all the tests available in the solution, including those under **`TinyURL.Presentation.UnitTests`**.
- To run all the tests, click the **`Run All`** button at the top of the **Test Explorer** window.
- Alternatively, you can run specific tests by selecting them and clicking **`Run Selected Tests`**.

## Project Structure

- **TinyURL.Presentation**: The console application that provides the CLI interface for interacting with the URL shortening service.
- **TinyURL.Application**: Contains the **use cases** of the application.
- **TinyURL.Domain**: Contains the **core logic** and **business rules** of the application.
- **TinyURL.Infrastructure**: Provides implementations for data storage and other infrastructure-related concerns.