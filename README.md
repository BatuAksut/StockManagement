# Stock Management Application

This project is a **Stock Management System** built with **ASP.NET Core MVC** and leverages caching, CRUD operations, and a modular area-based architecture for **Admin** and **User** functionalities.

## Features

### Admin Panel
- **Product Management**:  
  - Create, Read, Update, and Delete (CRUD) products.  
  - Product list caching for optimized performance.  
  - Cache automatically refreshed after product updates (create, edit, delete).  
- **Stock Chart Visualization**:  
  - Visual representation of product stock and category-based stock levels.  


### Areas
- **Admin**: Manages product data, stock insights, and low-stock warnings.  
- **User**: Extendable for user-specific views or operations.

### Caching
- Implemented caching for **product listing** to improve performance.  
- Cache is invalidated and refreshed after any CRUD operation.  

### Logging
- Integrated **Serilog** for structured logging.  
- Logs are stored in files for better traceability and debugging.

---

## Technologies Used

- **Backend**: ASP.NET Core MVC  
- **Database**: SQL Server  
- **Caching**: IMemoryCache  
- **Logging**: Serilog  
- **ORM**: Entity Framework Core  

---

## Installation

### Prerequisites
- .NET 8 SDK installed  
- SQL Server running  

 **Clone the repository**:  
   ```bash
   git clone https://github.com/batuaksut/stock-management.git
   cd stock-management
