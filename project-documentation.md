## **Project Documentation**

### **1. Introduction**

This document details the changes, improvements, and practices implemented in the project to meet specific requirements and update the technology stack. The work followed **DDD (Domain-Driven Design)** principles and **S.O.L.I.D**, focusing on scalability, maintainability, and performance.

A **Web API** was implemented as an additional interface to process meal orders, leveraging **minimal APIs** and **Swagger/OpenAPI** for ease of integration and documentation.

---

### **2. Updates Implemented**

#### **2.1 Web API Implementation**

- Introduced a Web API as an additional interface to process meal orders.
- The API was built with **minimal APIs** in ASP.NET Core 9.0.
- **Swagger/OpenAPI** was integrated for detailed endpoint documentation and interactive testing.

---

### **3. Project Structure**

#### **3.1 Layers** *(Updated)*

1. **Presentation**:
    - Handles both CLI and Web API input.
    - Entry points:
        - **CLI**: Processes user input via `Program.cs`.
        - **Web API**: Accepts HTTP requests via minimal APIs.
    - Example: **Program.cs** for CLI, **Web API setup in Program.cs** for HTTP endpoints.

2. **Services**:
    - Contains the business logic of the application.
    - Coordinates between the **Domain** and **Infrastructure** layers.
    - Example:
        - `DishManager` (implements `IDishManager`).
        - `Server` (implements `IServer`).

3. **Domain**:
    - Represents the core business logic and rules.
    - Contains entities and interfaces defining the domain.
    - Example:
        - `Dish`, `DishData`, `Order`.
        - `IDishRepository`.

4. **Infrastructure**:
    - Manages data access and communication with external systems.
    - Example:
        - `DishRepository`: Implements `IDishRepository`.

5. **Tests**:
    - Contains unit and integration tests for all layers.
    - Examples:
        - `DishManagerTests`.
        - `DishRepositoryTests`.
        - `ServerTests`.
        - `API Tests`.

---

### **4. Web API Details**

#### **4.1 Overview**

- The Web API provides an interface to process meal orders using HTTP requests.
- Built with **ASP.NET Core 9.0**.
- Utilizes **Swagger/OpenAPI** for self-documenting endpoints.

#### **4.2 Endpoints**

**Endpoint**: `/orders`

- **Method**: `GET`
- **Description**: Processes a meal order and returns the result.
- **Parameters**:
    - `unparsedOrder`: The input string representing the meal period and dish IDs (e.g., `morning, 1, 2, 3`).
- **Responses**:
    - `200 OK`: The order was successfully processed.
        - Example:
          ```json
          {
            "data": "eggs,toast,coffee"
          }
          ```
    - `400 Bad Request`: The input was invalid.
        - Example:
          ```json
          {
            "error": "Invalid period. Must be 'morning' or 'evening'."
          }
          ```

#### **4.3 Features**

1. **Swagger Integration**:
    - Accessible at `/swagger` in development environments.
    - Includes parameter details, example inputs, and response formats.

2. **Error Handling**:
    - Returns descriptive error messages for invalid inputs.

3. **Minimal API**:
    - Streamlined API setup using minimal APIs in ASP.NET Core 9.0.

#### **4.4 How to Use**

1. **Start the API**:
   ```bash
   dotnet run
   ```

2. **Access Swagger**:
    - Navigate to `https://localhost:<port>/swagger` to explore and test the API.

3. **Example Request**:
    - Input: `GET /order?unparsedOrder=morning,1,2,3`
    - Output:
      ```json
      {
        "data": "eggs,toast,coffee"
      }
      ```

---

### **5. Final Considerations**

The inclusion of the Web API enhances the project's extensibility and usability. By supporting both CLI and HTTP interfaces, the system accommodates a wider range of client applications. The use of Swagger ensures that the API is self-documenting and easy to integrate.

