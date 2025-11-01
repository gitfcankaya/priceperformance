# PricePerformance - Global Price Comparison Platform

A comprehensive e-commerce price comparison platform that tracks product prices across multiple platforms and countries, helping users find the best deals worldwide.

## 🌟 Features

- **Multi-Platform Price Tracking**: Compare prices from Amazon, eBay, AliExpress, Walmart, Best Buy, and Turkish platforms (Trendyol, Hepsiburada, N11)
- **Global Coverage**: Track prices across 10+ countries with currency conversion
- **Price History**: View historical price data and trends
- **Best Deal Finder**: Intelligent algorithms to identify the best prices
- **AI-Powered Content**: Integration with OpenAI and Google Gemini for unique product descriptions
- **Modern UI**: Responsive React frontend with Material-UI
- **RESTful API**: Well-documented .NET Core backend

## 🏗️ Architecture

### Tech Stack

**Backend:**
- .NET 9.0 (ASP.NET Core Web API)
- Entity Framework Core 9.0
- MSSQL Server
- Swagger/OpenAPI

**Frontend:**
- React 18 with Vite
- Material-UI (MUI)
- Axios for API calls
- React Router for navigation

**Tools:**
- HtmlAgilityPack for web scraping
- Newtonsoft.Json for JSON handling

## 📋 Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Node.js 18+](https://nodejs.org/)
- [MSSQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or MSSQL LocalDB

## 🚀 Getting Started

### Backend Setup

1. **Clone the repository**
```bash
git clone https://github.com/gitfcankaya/priceperformance.git
cd priceperformance
```

2. **Update database connection string**
Edit `src/PricePerformance.API/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PricePerformanceDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

3. **Create and seed the database**
```bash
cd src/PricePerformance.Infrastructure
dotnet ef migrations add InitialCreate --startup-project ../PricePerformance.API
dotnet ef database update --startup-project ../PricePerformance.API
```

4. **Run the API**
```bash
cd ../PricePerformance.API
dotnet run
```

The API will be available at `http://localhost:5000` and Swagger UI at `http://localhost:5000/swagger`

### Frontend Setup

1. **Navigate to the client directory**
```bash
cd ../../client
```

2. **Install dependencies**
```bash
npm install
```

3. **Configure API URL (optional)**
Edit `client/.env`:
```
VITE_API_URL=http://localhost:5000/api
```

4. **Run the development server**
```bash
npm run dev
```

The frontend will be available at `http://localhost:5173`

## 📚 Project Structure

```
priceperformance/
├── src/
│   ├── PricePerformance.API/          # Web API project
│   │   ├── Controllers/               # API controllers
│   │   ├── Program.cs                 # Application entry point
│   │   └── appsettings.json          # Configuration
│   ├── PricePerformance.Core/         # Domain layer
│   │   ├── Entities/                  # Domain models
│   │   └── Interfaces/                # Service contracts
│   └── PricePerformance.Infrastructure/ # Data access layer
│       ├── Data/                      # DbContext and migrations
│       ├── Repositories/              # Repository implementations
│       └── Services/                  # Service implementations
├── tests/
│   └── PricePerformance.Tests/        # Unit tests
├── client/                            # React frontend
│   ├── src/
│   │   ├── components/               # Reusable components
│   │   ├── pages/                    # Page components
│   │   ├── services/                 # API services
│   │   └── App.jsx                   # Root component
│   └── package.json
├── docs/                              # Documentation
│   ├── PRD.md                        # Product Requirements
│   ├── SRS.md                        # Software Requirements
│   ├── SDS.md                        # Software Design
│   └── TODO.md                       # Task list
└── README.md
```

## 🔌 API Endpoints

### Products
- `GET /api/products` - List all products
- `GET /api/products/{id}` - Get product details
- `GET /api/products/search?query={q}` - Search products

### Categories
- `GET /api/categories` - List all categories
- `GET /api/categories/{id}` - Get category details
- `GET /api/categories/root` - Get root categories

### Price Comparison
- `GET /api/pricecomparison/best-price/{productId}` - Get best price
- `GET /api/pricecomparison/best-prices-category/{categoryId}` - Get best prices for category
- `GET /api/pricecomparison/price-history/{productId}` - Get price history

### Platforms & Countries
- `GET /api/platforms` - List all platforms
- `GET /api/countries` - List all countries

## 🗄️ Database Schema

The system uses a normalized relational database with the following main entities:

- **Countries**: Country information and currencies
- **Platforms**: E-commerce platform details
- **Categories**: Hierarchical product categories
- **Products**: Product information and specifications
- **ProductPrices**: Price data with platform, country, and timestamp
- **ProductReviews**: User reviews and ratings
- **ProductSpecifications**: Product technical specifications

See `docs/SDS.md` for detailed database design.

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## 📄 License

This project is licensed under the MIT License.

## 🔮 Future Enhancements

- User authentication and accounts
- Price alerts and notifications
- Mobile native apps (iOS/Android)
- Browser extension
- Machine learning for price predictions
- Affiliate program integration

## 📞 Contact

For questions or support, please open an issue on GitHub.
