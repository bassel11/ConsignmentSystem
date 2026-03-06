#  Enterprise Consignment Management System API

> A robust, scalable, and secure RESTful API built with **.NET 10** for managing consignment inventory, logistics, and financial settlements. 

##  Project Overview & Business Value
This system is designed to digitalize the workflow of a consignment business. It tracks the entire lifecycle of goods: from the moment a vendor's vehicle arrives at the warehouse, through inventory intake and retail sales, down to the final financial settlement and automated invoice generation.

###  Architectural Decisions (The "Why")
As a software engineer, I focus on building systems that are not just functional, but maintainable and secure. Here are the core architectural patterns applied:

* **Clean Architecture & DDD (Domain-Driven Design):** The core business logic is encapsulated within Domain Entities. We avoided "anemic domains" by encapsulating rules (e.g., `NetPayable` calculation, `QuantitySold` deduction) directly inside the entities.
* **CQRS with MediatR:** Separated Read (Queries) and Write (Commands) operations. Commands use EF Core's Change Tracker, while Queries use `AsNoTracking()` and `Mapster`'s `ProjectToType` for extreme performance and direct SQL translation.
* **Strict Encapsulation & Aggregate Roots:** Entities reference each other via IDs (e.g., `VehicleId`) rather than object references, preventing EF Core from pulling unnecessary large object graphs (N+1 problem) and respecting DDD aggregate boundaries.
* **Zero Trust Security & RBAC:** Endpoints don't just check if a user is authenticated; they check if the user has the right role (`Admin`, `Storekeeper`, `Accountant`) and if the requested resource logically belongs to the specified context (e.g., ensuring a sold item actually belongs to the vehicle making the sale).
* **Audit Trail & Soft Delete:** Financial systems cannot lose data. All entities inherit from `AuditableEntity`. Every `INSERT/UPDATE` is stamped with the user's email via an injected `ICurrentUserService` at the `DbContext` level. Deletions are logical (`IsDeleted = true`) with Global Query Filters applied.

---

##  Entity Relationship Diagram (ERD)

The database schema is designed to enforce relational integrity and separate master data from transactional data.

```mermaid
erDiagram
    ApplicationUser ||--o{ Vendor : manages
    Vendor ||--o{ Vehicle : owns
    Vehicle ||--o{ ConsignmentReceipt : delivers
    ConsignmentReceipt ||--o{ ConsignmentItem : contains
    Vehicle ||--o{ SaleTransaction : performs
    Vehicle ||--o{ Invoice : billed_via
    ConsignmentItem ||--o{ SaleTransaction : sold_in

    Vendor {
        Guid Id PK
        string Name
        string ContactEmail
        decimal DefaultCommissionPercentage
        bool IsDeleted
    }
    Vehicle {
        Guid Id PK
        Guid VendorId FK
        string LicensePlate
        string DriverName
    }
    ConsignmentReceipt {
        Guid Id PK
        Guid VehicleId FK
        string ReceiptNumber
        DateTime ReceiptDate
    }
    ConsignmentItem {
        Guid Id PK
        Guid ConsignmentReceiptId FK
        string ProductName
        int QuantityReceived
        int QuantitySold
        decimal UnitPrice
    }
    SaleTransaction {
        Guid Id PK
        Guid VehicleId FK
        Guid ConsignmentItemId FK
        int Quantity
        decimal SalePrice
        bool IsInvoiced
    }
    Invoice {
        Guid Id PK
        Guid VehicleId FK
        string InvoiceNumber
        decimal TotalSalesAmount
        decimal CommissionAmount
        decimal ExpensesAmount
        decimal NetPayable
    }