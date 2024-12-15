**Project Documentation**

**1. Introduction**

This document details the changes, improvements, and practices implemented in the project to meet specific requirements and update the technology stack. The work followed **DDD (Domain-Driven Design)** principles and **S.O.L.I.D**, focusing on scalability, maintainability, and performance.

-----
**2. Updates Implemented**

**2.1 .NET Core Update**

- The project was updated from **.NET Core 3.1** to **.NET 9.0**, ensuring support for the latest features and better performance.

**2.2 C# Update**

- Migrated from **C# 8** to **C# 13**, enabling the use of modern features such as:
  - **Advanced Pattern Matching**
  - **Raw String Literals**
  - **List Patterns**
  - **Static Abstract Members in Interfaces**

**2.3 Nullable Reference Types**

- **Nullable reference types** were enabled to ensure better safety against **NullReferenceException**.
- All classes were reviewed and adjusted to utilize this feature.

**2.4 NuGet Package Updates**

- All NuGet packages were updated to their latest versions.
- Vulnerabilities in outdated packages were addressed:
  - A security audit was performed using tools such as **dotnet outdated** and **Dependabot**.

**3. Code Changes**

**3.1 Test Refactoring**

- **NUnit Framework** was updated to support the new .NET and C# versions.
- Changes made to test methods:
  - Replaced **Assert.AreEqual()** with **Assert.Equals()**, as required.
  - Adopted **Assert.Throws** for exception validation.
- Tests were created or updated for the following areas:
  - **DishManager**
  - **DishRepository**
  - **Server**
  - Error cases, such as invalid periods or dishes.

**3.2 Use of String Interpolation**

- Replaced **string.Format** with **string interpolation** ($"Text {variable}") for better readability and performance.

**3.3 Project Structure**

- Organized the project into logical folders, following a **DDD (Domain-Driven Design)** layer pattern:
  - **Domain**:
    - Domain entities and interfaces.
  - **Application**:
    - Business logic and services.
  - **Infrastructure**:
    - Repository implementations and external dependencies.
  - **Tests**:
    - Unit and integration tests.

**3.4 Implementation of IoC and DI**

- **Inversion of Control (IoC)** and **Dependency Injection (DI)** were implemented using **Microsoft.Extensions.DependencyInjection**.
- Configurations made in **Program.cs**:
  - Registered interfaces and their implementations:
    - IDishManager, DishManager
    - IDishRepository, DishRepository
    - IServer, Server
  - Managed instance of **ServiceProvider** for dependency injection.

**4. Project Structure**

**4.1 Layers**

1. **Presentation**:
   1. Entry point for the application (e.g., CLI).
   1. Handles user input and passes it to the services.
   1. Example: **Program.cs**.
1. **Services**:
   1. Contains the application’s business logic.
   1. Coordinates actions between the **Domain** and **Infrastructure** layers.
   1. Example:
      1. DishManager (implements IDishManager)
      1. Server (implements IServer).
1. **Domain**:
   1. Represents the core business logic and rules.
   1. Contains entities and interfaces defining the domain.
   1. Example:
      1. Dish, DishData, Order
      1. IDishRepository.
1. **Infrastructure**:
   1. Handles communication with external systems or data sources.
   1. Example:
      1. DishRepository:
         1. Centralizes data for dishes.
         1. Implements IDishRepository.
1. **Tests**:
   1. Contains unit and integration tests.
   1. Coverage for:
      1. **DishManagerTests**
      1. **DishRepositoryTests**
      1. **ServerTests**

**5. Best Practices Adopted**

**5.1 Design Patterns**

- **DDD (Domain-Driven Design)**:
  - Separation of responsibilities between **Domain**, **Application**, and **Infrastructure**.
- **S.O.L.I.D**:
  - **Single Responsibility Principle**: Each class or method has a well-defined function.
  - **Dependency Inversion Principle**: Interfaces define contracts, with implementations injected dynamically.

**5.2 Clean Code**

- Refactored code for improved readability and maintainability:
  - Short and focused methods.
  - Descriptive variable and method names.

**6. System Features**

- Accept CLI input for processing orders.
- Validate periods and dishes.
- Return results in a clear and ordered format:
  - Example: "eggs,toast,coffee(x2)".
- Support for multiple instances of dishes when allowed (e.g., coffee and potato).
- Friendly error messages for invalid inputs:
  - Example: "error: Invalid period 'afternoon'.".

**7. Tests Implemented**

**7.1 Coverage**

- **DishManager**:
  - Validation of periods.
  - Allowed and multiple dishes.
- **DishRepository**:
  - Correct data retrieval for dishes and periods.
  - Error cases for nonexistent dishes or invalid periods.
- **Server**:
  - Full order processing and output formatting.

**7.2 Tools**

- Test Framework: **NUnit**
- CI/CD: Integrated with **Azure DevOps** for automated test execution.

**8. Getting Started**

**Setup Steps**

1. Clone the repository:

   git clone https://github.com/lkolodziey/Grosvenor.git

1. Restore NuGet packages:

   dotnet restore

1. Run the tests:

   dotnet test

1. Run the application:

   dotnet run --project GrosvenorDeveloperPracticum

-----
**9. Final Considerations**

The changes significantly improved the security, maintainability, and performance of the project. By adopting patterns like **DDD**, **S.O.L.I.D**, and modern C# practices, the system is now prepared for future scalability and enhancements.

