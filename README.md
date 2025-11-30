# URL Shortener Service

A high-performance RESTful API for URL shortening with analytics tracking, built with ASP.NET Core 8, PostgreSQL, and Redis.

## 🚀 Setup Instructions

### Prerequisites
- Docker & Docker Compose

### Running the Application
1. Clone the repository.
2. Run the following command in the root directory:
   ```bash
   docker compose up --build
   ```
3. The API will be available at `http://localhost:8080`.
4. Swagger Documentation: `http://localhost:8080/swagger`.

## 🏗 Design Decisions

### Architecture
- **ASP.NET Core Web API**: Chosen for its high performance, cross-platform capabilities, and robust ecosystem.
- **PostgreSQL**: Used as the primary persistent storage for URLs and analytics data due to its reliability and powerful querying capabilities.
- **Redis**: Implemented as a caching layer for URL lookups to meet the <150ms latency requirement for redirects. It is also used for rate limiting.
- **Entity Framework Core**: Used for ORM to simplify database interactions and migrations.

### Database Schema
- **Urls Table**: Stores the mapping between short codes and long URLs. Indexed on `ShortCode` for fast lookups.
- **Clicks Table**: Stores analytics data for each redirect. Indexed on `ShortCode` and `ClickedAt` to optimize aggregation queries for analytics.

### Analytics
- **Tracking**: Click events are recorded synchronously for simplicity as per requirements, but the architecture allows for easy migration to an async background queue (e.g., using `System.Threading.Channels` or a message broker like RabbitMQ) for higher scale.
- **Aggregation**: The analytics endpoint aggregates data on-the-fly. For very high traffic, pre-aggregation (e.g., incrementing counters in Redis or a separate stats table) would be a better approach.

### Rate Limiting
- Implemented using `AspNetCoreRateLimit` with Redis backing.
- configured to limit requests to 100 per minute per IP to prevent abuse.

## ⚖️ Trade-offs

- **Synchronous Analytics**: To save development time and complexity, analytics are written to the DB during the redirect request. This adds a small latency overhead. In a production system with millions of clicks, this should be offloaded to a background worker.
- **Short Code Generation**: A simple hash-based approach (MD5) is used. In a distributed system, a pre-generated pool of tokens or a distributed ID generator (like Snowflake) would be preferred to avoid collisions and lock contention.
- **Database Normalization**: The schema is normalized. For read-heavy analytics, a denormalized view or a time-series database (like TimescaleDB) might perform better.

## 🔮 Future Improvements

- **Async Analytics**: Move click tracking to a background service.
- **User Accounts**: Add authentication and user-specific URL management.
- **Custom Domains**: Allow users to use their own domains.
- **Advanced Analytics**: Geolocation, device type, and referrer tracking.
- **Horizontal Scaling**: Deploy behind a load balancer with multiple API replicas.

## 🤖 GenAI Usage
See [GENAI_USAGE.md](GENAI_USAGE.md) for details.
