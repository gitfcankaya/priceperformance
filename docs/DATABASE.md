# Database Design Document
## PricePerformance Database Schema

### Overview
The PricePerformance database uses a relational model with MSSQL Server. The schema is designed to efficiently store and query product information, prices across multiple platforms and countries, and related metadata.

### Database Tables

#### 1. Countries
Stores information about supported countries.

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | int | PK, Identity | Primary key |
| Name | nvarchar(100) | NOT NULL | Country name (e.g., "United States") |
| Code | nvarchar(3) | NOT NULL, UNIQUE | ISO country code (e.g., "US") |
| Currency | nvarchar(3) | NOT NULL | Currency code (e.g., "USD") |
| CurrencySymbol | nvarchar(10) | NOT NULL | Currency symbol (e.g., "$") |
| IsActive | bit | NOT NULL | Whether country is active |
| CreatedAt | datetime2 | NOT NULL | Record creation timestamp |
| UpdatedAt | datetime2 | NULL | Record update timestamp |

**Indexes:**
- PK_Countries on Id
- IX_Countries_Code (Unique) on Code

#### 2. Platforms
Stores information about e-commerce platforms.

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | int | PK, Identity | Primary key |
| Name | nvarchar(100) | NOT NULL, UNIQUE | Platform name (e.g., "Amazon") |
| BaseUrl | nvarchar(500) | NOT NULL | Platform base URL |
| LogoUrl | nvarchar(500) | NULL | Logo image URL |
| IsActive | bit | NOT NULL | Whether platform is active |
| CreatedAt | datetime2 | NOT NULL | Record creation timestamp |
| UpdatedAt | datetime2 | NULL | Record update timestamp |

**Indexes:**
- PK_Platforms on Id
- IX_Platforms_Name (Unique) on Name

#### 3. Categories
Hierarchical product categories.

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | int | PK, Identity | Primary key |
| Name | nvarchar(200) | NOT NULL | Category name |
| Slug | nvarchar(200) | NOT NULL, UNIQUE | URL-friendly identifier |
| Description | nvarchar(max) | NULL | Category description |
| ParentCategoryId | int | NULL, FK | Parent category reference |
| CreatedAt | datetime2 | NOT NULL | Record creation timestamp |
| UpdatedAt | datetime2 | NULL | Record update timestamp |

**Relationships:**
- Self-referencing: ParentCategoryId → Categories.Id (Restrict)

**Indexes:**
- PK_Categories on Id
- IX_Categories_Slug (Unique) on Slug
- IX_Categories_ParentCategoryId on ParentCategoryId

#### 4. Products
Product information and details.

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | int | PK, Identity | Primary key |
| Name | nvarchar(500) | NOT NULL | Product name |
| Slug | nvarchar(500) | NOT NULL | URL-friendly identifier |
| Description | nvarchar(5000) | NULL | Product description |
| OriginalDescription | nvarchar(max) | NULL | Original scraped description |
| Brand | nvarchar(200) | NULL | Product brand |
| Model | nvarchar(200) | NULL | Product model |
| ImageUrl | nvarchar(500) | NULL | Main product image URL |
| CategoryId | int | NOT NULL, FK | Category reference |
| IsActive | bit | NOT NULL | Whether product is active |
| CreatedAt | datetime2 | NOT NULL | Record creation timestamp |
| UpdatedAt | datetime2 | NULL | Record update timestamp |

**Relationships:**
- CategoryId → Categories.Id (Restrict)

**Indexes:**
- PK_Products on Id
- IX_Products_Slug on Slug
- IX_Products_CategoryId on CategoryId

#### 5. ProductPrices
Price information for products across platforms and countries.

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | int | PK, Identity | Primary key |
| ProductId | int | NOT NULL, FK | Product reference |
| PlatformId | int | NOT NULL, FK | Platform reference |
| CountryId | int | NOT NULL, FK | Country reference |
| Price | decimal(18,2) | NOT NULL | Current/recorded price |
| OriginalPrice | decimal(18,2) | NULL | Original price (before discount) |
| IsInStock | bit | NOT NULL | Stock availability |
| ProductUrl | nvarchar(1000) | NOT NULL | Direct product URL |
| PriceDate | datetime2 | NOT NULL | Price record timestamp |
| CreatedAt | datetime2 | NOT NULL | Record creation timestamp |
| UpdatedAt | datetime2 | NULL | Record update timestamp |

**Relationships:**
- ProductId → Products.Id (Cascade)
- PlatformId → Platforms.Id (Restrict)
- CountryId → Countries.Id (Restrict)

**Indexes:**
- PK_ProductPrices on Id
- IX_ProductPrices_Composite on (ProductId, PlatformId, CountryId, PriceDate)

#### 6. ProductReviews
User reviews for products.

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | int | PK, Identity | Primary key |
| ProductId | int | NOT NULL, FK | Product reference |
| ReviewerName | nvarchar(200) | NOT NULL | Reviewer name |
| ReviewText | nvarchar(5000) | NOT NULL | Review content |
| OriginalReviewText | nvarchar(max) | NULL | Original scraped review |
| Rating | int | NOT NULL | Rating (1-5) |
| ReviewDate | datetime2 | NOT NULL | Review date |
| Platform | nvarchar(100) | NULL | Source platform |
| IsVerified | bit | NOT NULL | Verified purchase status |
| CreatedAt | datetime2 | NOT NULL | Record creation timestamp |
| UpdatedAt | datetime2 | NULL | Record update timestamp |

**Relationships:**
- ProductId → Products.Id (Cascade)

**Indexes:**
- PK_ProductReviews on Id

#### 7. ProductSpecifications
Technical specifications for products.

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | int | PK, Identity | Primary key |
| ProductId | int | NOT NULL, FK | Product reference |
| SpecificationKey | nvarchar(200) | NOT NULL | Specification name (e.g., "Screen Size") |
| SpecificationValue | nvarchar(500) | NOT NULL | Specification value (e.g., "6.1 inches") |
| CreatedAt | datetime2 | NOT NULL | Record creation timestamp |
| UpdatedAt | datetime2 | NULL | Record update timestamp |

**Relationships:**
- ProductId → Products.Id (Cascade)

**Indexes:**
- PK_ProductSpecifications on Id

### Entity Relationship Diagram

```
Countries (1) ----< (N) ProductPrices
Platforms (1) ----< (N) ProductPrices
Products (1) ----< (N) ProductPrices
Products (1) ----< (N) ProductReviews
Products (1) ----< (N) ProductSpecifications
Categories (1) ----< (N) Products
Categories (1) ----< (N) Categories (self-reference)
```

### Seed Data

The database includes seed data for:
- 10 countries (US, UK, DE, TR, FR, ES, IT, JP, CA, AU)
- 9 e-commerce platforms (Amazon, eBay, AliExpress, Walmart, Target, Best Buy, Trendyol, Hepsiburada, N11)
- 10 categories with hierarchical structure
- 5 sample products
- Sample price data for demonstration

### Performance Considerations

1. **Indexing Strategy**
   - Primary keys on all tables
   - Unique indexes on natural keys (Country.Code, Platform.Name, Category.Slug)
   - Composite index on ProductPrices for efficient price queries
   - Foreign key indexes for join optimization

2. **Data Types**
   - Decimal(18,2) for prices to ensure precision
   - nvarchar for Unicode support (international products)
   - datetime2 for better precision and range

3. **Relationships**
   - Cascade delete for dependent entities (prices, reviews, specs)
   - Restrict delete for reference data (countries, platforms, categories)

4. **Query Optimization**
   - Eager loading of related entities in API queries
   - Composite index on (ProductId, PlatformId, CountryId, PriceDate) for price history queries
   - Filtering inactive records at query time

### Migration Commands

```bash
# Create initial migration
dotnet ef migrations add InitialCreate --startup-project ../PricePerformance.API --project ../PricePerformance.Infrastructure

# Update database
dotnet ef database update --startup-project ../PricePerformance.API --project ../PricePerformance.Infrastructure

# Create new migration after schema changes
dotnet ef migrations add <MigrationName> --startup-project ../PricePerformance.API --project ../PricePerformance.Infrastructure
```

### Backup Strategy

1. **Automated Backups**: Daily full backups
2. **Transaction Log Backups**: Every 15 minutes
3. **Retention**: 30 days for daily, 7 days for transaction logs
4. **Testing**: Monthly restore test to verify backup integrity

### Future Enhancements

1. Add UserAccounts table for authentication
2. Add Wishlists table for user favorites
3. Add PriceAlerts table for notifications
4. Implement data archival for old price records
5. Add audit logging tables
6. Implement read replicas for scaling
