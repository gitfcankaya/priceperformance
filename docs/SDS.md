# Software Design Specification (SDS)
## PricePerformance Platform Architecture

### 1. System Architecture

#### 1.1 Architecture Pattern
The system follows a **3-tier architecture** with clean separation of concerns:
- **Presentation Layer**: React SPA
- **Business Logic Layer**: .NET Core API
- **Data Layer**: Entity Framework Core + MSSQL

#### 1.2 Design Principles
- SOLID principles
- Repository pattern
- Dependency Injection
- Asynchronous programming
- RESTful API design

### 2. Component Design

#### 2.1 Backend Components

##### 2.1.1 API Layer (PricePerformance.API)
- **Controllers**: Handle HTTP requests and responses
  - ProductsController
  - CategoriesController
  - PriceComparisonController
  - PlatformsController
  - CountriesController
- **Middleware**: CORS, error handling, logging
- **Startup Configuration**: DI container setup, database configuration

##### 2.1.2 Core Layer (PricePerformance.Core)
- **Entities**: Domain models
  - BaseEntity (abstract base)
  - Product
  - ProductPrice
  - Category
  - Platform
  - Country
  - ProductReview
  - ProductSpecification
- **Interfaces**: Service contracts
  - IRepository<T>
  - ICrawlerService
  - IPriceComparisonService
  - IAIContentService

##### 2.1.3 Infrastructure Layer (PricePerformance.Infrastructure)
- **Data Access**
  - ApplicationDbContext
  - Repository<T> implementation
  - DbSeeder for sample data
- **Services**
  - PriceComparisonService
  - CrawlerService
  - AIContentService

#### 2.2 Frontend Components

##### 2.2.1 Pages
- HomePage: Landing page with featured deals
- ProductDetailPage: Detailed product view with price comparison
- CategoryPage: Category browse with products
- SearchPage: Search results display

##### 2.2.2 Components
- Navbar: Navigation and search
- ProductCard: Product summary card
- (Future): PriceChart, FilterPanel, etc.

##### 2.2.3 Services
- api.js: Axios-based API client
- API modules: productsApi, categoriesApi, priceComparisonApi, etc.

### 3. Database Design

#### 3.1 Entity Relationship Diagram
```
Countries (1) ----< (N) ProductPrices
Platforms (1) ----< (N) ProductPrices
Products (1) ----< (N) ProductPrices
Products (1) ----< (N) ProductReviews
Products (1) ----< (N) ProductSpecifications
Categories (1) ----< (N) Products
Categories (1) ----< (N) Categories (self-referencing)
```

#### 3.2 Key Tables

**Products**
- Id, Name, Slug, Description, Brand, Model, ImageUrl, CategoryId, IsActive
- Indexes: Slug, CategoryId

**ProductPrices**
- Id, ProductId, PlatformId, CountryId, Price, OriginalPrice, IsInStock, ProductUrl, PriceDate
- Indexes: (ProductId, PlatformId, CountryId, PriceDate)

**Countries**
- Id, Name, Code, Currency, CurrencySymbol, IsActive
- Indexes: Code (unique)

**Platforms**
- Id, Name, BaseUrl, LogoUrl, IsActive
- Indexes: Name (unique)

**Categories**
- Id, Name, Slug, Description, ParentCategoryId
- Indexes: Slug (unique), ParentCategoryId

### 4. API Design

#### 4.1 Endpoints

**Products**
- GET /api/products - List all products (with optional categoryId filter)
- GET /api/products/{id} - Get product details
- GET /api/products/search?query={q} - Search products
- POST /api/products - Create product (admin)
- PUT /api/products/{id} - Update product (admin)
- DELETE /api/products/{id} - Delete product (admin)

**Categories**
- GET /api/categories - List all categories
- GET /api/categories/{id} - Get category details
- GET /api/categories/root - Get root categories

**Price Comparison**
- GET /api/pricecomparison/best-price/{productId} - Get best price for product
- GET /api/pricecomparison/best-prices-category/{categoryId} - Get best prices for category
- GET /api/pricecomparison/price-history/{productId}?startDate={date} - Get price history

**Platforms**
- GET /api/platforms - List all platforms
- GET /api/platforms/{id} - Get platform details

**Countries**
- GET /api/countries - List all countries
- GET /api/countries/{id} - Get country details

#### 4.2 Response Format
```json
{
  "data": {},
  "message": "Success",
  "errors": []
}
```

### 5. Security Design

#### 5.1 Authentication (Future)
- JWT token-based authentication
- Refresh token mechanism
- Role-based authorization (User, Admin)

#### 5.2 Data Protection
- SQL injection prevention via parameterized queries
- XSS protection via input sanitization
- CORS configuration
- HTTPS enforcement

### 6. Performance Optimization

#### 6.1 Database
- Proper indexing on frequently queried columns
- Eager loading for related entities
- Query result caching (future)

#### 6.2 API
- Asynchronous operations
- Response compression
- API response caching

#### 6.3 Frontend
- Code splitting
- Lazy loading of components
- Image optimization
- CDN for static assets (future)

### 7. Monitoring and Logging

#### 7.1 Logging
- Structured logging with Serilog (future)
- Log levels: Debug, Info, Warning, Error, Critical
- Log aggregation service (future)

#### 7.2 Monitoring
- Application Insights (future)
- Health check endpoints
- Performance metrics

### 8. Deployment Architecture

#### 8.1 Development Environment
- Local MSSQL database
- .NET 9.0 SDK
- Node.js for React app

#### 8.2 Production Environment (Future)
- Azure App Service or containerized deployment
- Azure SQL Database
- Azure CDN for static assets
- Application Gateway for load balancing

### 9. Technology Stack

**Backend**
- .NET 9.0
- ASP.NET Core Web API
- Entity Framework Core 9.0
- MSSQL Server
- Swagger/OpenAPI

**Frontend**
- React 18
- Material-UI (MUI)
- Axios
- React Router
- Recharts (for future price charts)

**Development Tools**
- Visual Studio / VS Code
- Git
- Postman / Swagger UI
- MSSQL Management Studio
