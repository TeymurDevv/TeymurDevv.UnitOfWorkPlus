# TeymurDevv.UnitOfWorkPlus - Usage Guide

## Introduction
**TeymurDevv.UnitOfWorkPlus** is a powerful .NET package that provides an automated **Unit of Work** and **Repository Pattern** implementation. This package allows developers to:

- Define custom repositories **without** manually registering them.
- Use **IUnitOfWork** to access repositories dynamically as **properties**.
- Reduce boilerplate code while maintaining a clean and maintainable architecture.

This guide will walk you through the installation, setup, and usage of **TeymurDevv.UnitOfWorkPlus** in your .NET project.

---

## 📌 Installation

To install the package, run the following command in your .NET project:

```sh
 dotnet add package TeymurDevv.UnitOfWorkPlus
```

Or install via NuGet Package Manager in Visual Studio.

---

## 📌 Package Setup

### **1. Define Your DbContext**
Ensure your project has a `DbContext` to manage database interactions.

```csharp
using Microsoft.EntityFrameworkCore;

namespace YourProject.Data
{
    public class MyDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }
    }
}
```

---

### **2. Configure the Package in Program.cs**

Register **TeymurDevv.UnitOfWorkPlus** services in `Program.cs`.

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TeymurDevv.UnitOfWorkPlus.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Register DbContext
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer("YourConnectionString"));

// Register UnitOfWork and Repositories
builder.Services.AddUnitOfWork<MyDbContext>(typeof(MyDbContext).Assembly);

var app = builder.Build();
app.Run();
```

---

## 📌 Implementing Custom Repositories

### **1. Create a Model Class**
Define an entity class that represents a database table.

```csharp
using System.ComponentModel.DataAnnotations;

namespace YourProject.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
```

---

### **2. Create a Custom Repository**
Your repository should implement `IRepository<T>` from the package.

```csharp
using YourProject.Models;
using TeymurDevv.UnitOfWorkPlus.Repositories;

namespace YourProject.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product?> GetByNameAsync(string name);
    }

    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(DbContext context) : base(context) { }

        public async Task<Product?> GetByNameAsync(string name)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.Name == name);
        }
    }
}
```

---

## 📌 Using UnitOfWork in Your Application

### **1. Injecting UnitOfWork in a Controller**

```csharp
using Microsoft.AspNetCore.Mvc;
using TeymurDevv.UnitOfWorkPlus.UnitOfWork;
using YourProject.Repositories;

namespace YourProject.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _unitOfWork.GetRepository<IProductRepository>().GetAllAsync();
            return Ok(products);
        }
    }
}
```

---

## 📌 Key Features
- **Automatic Repository Discovery**: No need to manually register repositories.
- **Unit of Work Pattern**: Ensures transactions and proper resource management.
- **Dynamic Repository Access**: Access repositories as properties inside `IUnitOfWork`.

---

## 📌 Additional Configuration
### **Enabling Logging for Debugging**
If you want to log repository activity, configure logging inside `Program.cs`:

```csharp
builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
});
```

---

## 📌 Conclusion
With **TeymurDevv.UnitOfWorkPlus**, developers can manage repositories effortlessly, ensuring a clean and maintainable architecture. By following this guide, you now have:

✅ Automatic **repository** discovery and registration
✅ Clean **Unit of Work** pattern integration
✅ **Minimal configuration** needed to get started

🚀 Now you are ready to build scalable applications with **TeymurDevv.UnitOfWorkPlus**! 🚀

