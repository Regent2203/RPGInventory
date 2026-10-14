## Project Description
A simple demo backend application implementing a craft, shop, and inventory system for a typical RPG game.

The solution is divided into 4 subprojects based on Clean Architecture:
1. **RPGInventory.Domain**
   - Contains core game entities and configuration constants.
2. **RPGInventory.Application**
   - Contains business logic services and DTOs.
3. **RPGInventory.Infrastructure**
   - Contains database relations (EF Core, SQLite), migrations, and seed logic.
4. **RPGInventory.WebApi**
   - Contains exposed RESTful HTTP API controllers and launch settings.

---

## Quick Start
1. Clone or download the project from GitHub.
2. Open the solution in Visual Studio and run it. Alternatively, if you have Docker installed, open cmd/PowerShell inside the **RPGInventory** directory (where the **docker-compose.yml** file is located) and run:
```bash
docker-compose up --build -d
```
3. Open Swagger UI at: [http://localhost:5000/swagger/index.html](http://localhost:5000/swagger/index.html) (The application uses port 5000 for both local and Docker environments).

---

## Testing the API
Now you can try out the API methods via Swagger UI:

1. Use `POST /api/Players/register` to register a player. Copy the generated player ID from the response body to use in subsequent requests (there is no authorization system in this demo project).
2. Use `GET /api/Metadata/AsText` to see all available items and craft recipes in a human-readable text format. (Use `GET /api/Metadata` to retrieve raw JSON for client integration.)
3. Use `GET /api/Players/{id}/profile` to get player's profile and inventory info.
4. Use `POST /api/Shop/buy`, `POST /api/Shop/sell`, and `POST /api/Craft` to modify the inventory state.
5. Use `GET /api/Players/{id}/history` to get logs of all previous transactions and inventory modifications.