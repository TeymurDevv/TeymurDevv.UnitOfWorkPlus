# TeymurDevv.UnitOfWorkPlus

## 📌 Introduction
TeymurDevv.UnitOfWorkPlus is a **lightweight, generic Unit of Work and Repository pattern** for **Entity Framework Core**. This package simplifies database operations by providing a structured **Unit of Work** and **Generic Repository** implementation, allowing you to easily manage transactions and repositories.

---

## 🚀 Features
✅ **Generic Repository** with built-in support for filtering, pagination, and tracking options.  
✅ **Unit of Work** pattern to manage database transactions efficiently.  
✅ **Automatic Repository Registration** for clean dependency injection.  
✅ **Works with Any DbContext** – supports all Entity Framework Core providers.  
✅ **NuGet-Compatible** for easy integration into any .NET project.  

---

## 📦 Installation
Install via NuGet:
```sh
 dotnet add package TeymurDevv.UnitOfWorkPlus
```

OR manually add it to your **.csproj** file:
```xml
<PackageReference Include="TeymurDevv.UnitOfWorkPlus" Version="1.0.0" />
```

---

## ⚙️ Configuration
### **1️⃣ Register Services in `Program.cs` (or `Startup.cs`)**
In **.NET 6+**, modify `Program.cs`:
```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TeymurDevv.UnitOfWorkPlus;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer("YourConnectionString"));

// Register UnitOfWork
builder.Services.AddUnitOfWork<MyDbContext>();

var app = builder.Build();
```

For **.NET 5 and earlier**, add this in `Startup.cs`:
```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddDbContext<MyDbContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
    
    services.AddUnitOfWork<MyDbContext>();
}
```

---

## 📌 Usage

### **2️⃣ Inject `IUnitOfWork` and Use Repositories**
Once registered, you can **inject `IUnitOfWork`** in your services and access repositories dynamically.

```csharp
public class CategoryService
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<Category>> GetCategories()
    {
        return await _unitOfWork.Repository<Category>().GetAll();
    }

    public async Task AddCategory(Category category)
    {
        await _unitOfWork.Repository<Category>().Create(category);
        await _unitOfWork.SaveChangesAsync();
    }
}
```

---

## 🎯 Unit of Work API
### `IUnitOfWork` Methods
| Method | Description |
|--------|-------------|
| `BeginTransactionAsync(CancellationToken cancellationToken = default)` | Begins a new database transaction asynchronously. |
| `CommitTransactionAsync(CancellationToken cancellationToken = default)` | Commits the current database transaction asynchronously. |
| `RollbackTransactionAsync(CancellationToken cancellationToken = default)` | Rolls back the current database transaction asynchronously. |
| `SaveChangesAsync(CancellationToken cancellationToken = default)` | Saves all changes to the database asynchronously. |

---

## 📌 Repository API
### `IRepository<T>` Methods
| Method | Description |
|--------|-------------|
| `GetEntity(Expression<Func<T, bool>> predicate = null, bool AsnoTracking = false, int skip = 0, int take = 0, params Func<IQueryable<T>, IQueryable<T>>[] includes)` | Retrieves a single entity with optional filters, includes, and tracking settings. |
| `GetAll(Expression<Func<T, bool>> predicate = null, bool AsnoTracking = false, int skip = 0, int take = 0, params Func<IQueryable<T>, IQueryable<T>>[] includes)` | Retrieves a list of entities with optional filters, pagination, and includes. |
| `Create(T entity)` | Adds a new entity to the database. |
| `Update(T entity)` | Updates an existing entity. |
| `Delete(T entity)` | Deletes an entity from the database. |
| `IsExists(Expression<Func<T, bool>> predicate = null)` | Checks if an entity exists based on a condition. |
| `GetQuery(Expression<Func<T, bool>> predicate = null, bool AsnoTracking = false, params Func<IQueryable<T>, IQueryable<T>>[] includes)` | Returns an IQueryable<T> allowing for custom queries. |

---

## 📌 Example Queries
### **Retrieve a Single Entity with Filters**
```csharp
var category = await _unitOfWork.Repository<Category>()
    .GetEntity(c => c.Name == "Technology");
```

### **Retrieve All with Includes & Pagination**
```csharp
var categories = await _unitOfWork.Repository<Category>()
    .GetAll(predicate: c => c.IsActive, take: 10, includes: q => q.Include(c => c.Products));
```

### **Update an Entity**
```csharp
var category = await _unitOfWork.Repository<Category>().GetEntity(c => c.Id == 1);
category.Name = "Science";
await _unitOfWork.Repository<Category>().Update(category);
await _unitOfWork.SaveChangesAsync();
```

### **Delete an Entity**
```csharp
var category = await _unitOfWork.Repository<Category>().GetEntity(c => c.Id == 1);
await _unitOfWork.Repository<Category>().Delete(category);
await _unitOfWork.SaveChangesAsync();
```

---

## 📌 Contributing
We welcome contributions! If you'd like to improve this library, feel free to fork the repository and submit a pull request.

---

## 📜 License
This project is licensed under the **MIT License**.

---

## 🛠 Support & Contact
For issues and feature requests, please create an issue on [GitHub](https://github.com/your-repo-url).

**Happy Coding! 🚀**

