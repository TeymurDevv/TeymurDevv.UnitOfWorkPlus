# UnitOfWorkPlus - Usage Guide

## 📌 Introduction
UnitOfWorkPlus is a **fully automated** and **dependency injection (DI) friendly** implementation of the **Unit of Work + Repository Pattern**. It allows you to **manage repositories dynamically** without manually registering them.

## 🚀 Features
✔ **No manual repository registration required**  
✔ **Automatic detection of repositories**  
✔ **Direct repository access via `_unitOfWork.ProductRepository`**  
✔ **Works seamlessly with Dependency Injection (DI)**  
✔ **Supports dynamic access using `_unitOfWork["Product"]`**  

---

## 📌 1️⃣ Installation
1. Add `UnitOfWorkPlus` package to your project:
   ```sh
   dotnet add package TeymurDevv.UnitOfWorkPlus
   ```

---

## 📌 2️⃣ Setup in `Program.cs`
Register **UnitOfWork and DbContext** in **Dependency Injection (DI)**.

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using YourNamespace.Data; // Your ApplicationDbContext
using TeymurDevv.UnitOfWorkPlus;

var builder = WebApplication.CreateBuilder(args);

// Register UnitOfWork and DbContext
builder.Services.AddUnitOfWork<ApplicationDbContext>();

var app = builder.Build();
app.Run();
```

---

## 📌 3️⃣ Inject `IUnitOfWork` and Use in a Service
Use `IUnitOfWork` in services to access repositories dynamically.

```csharp
public class ProductService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task AddProduct(string name, decimal price)
    {
        var product = new Product { Name = name, Price = price };
        await _unitOfWork.Repository<Product>().Create(product);
        await _unitOfWork.SaveChangesAsync();
    }
}
```

---

## 📌 4️⃣ Inject `IUnitOfWork` in a Controller
Use `IUnitOfWork` in an API controller to perform CRUD operations.

```csharp
[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] Product product)
    {
        await _unitOfWork.Repository<Product>().Create(product);
        await _unitOfWork.SaveChangesAsync();
        return Ok("Product added successfully.");
    }

    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetProducts()
    {
        return await _unitOfWork.Repository<Product>().GetAll();
    }
}
```

---

## 📌 5️⃣ Dynamic Repository Access
You can dynamically access repositories by entity name.

```csharp
var productRepo = (IRepository<Product>)_unitOfWork["Product"];
await productRepo.Create(new Product { Name = "Smartphone", Price = 799.99M });
await _unitOfWork.SaveChangesAsync();
```

---

## 🎯 Summary
✔ **Automatic repository detection**  
✔ **No need for manual repository registration**  
✔ **Seamless integration with ASP.NET Core**  
✔ **Direct repository access via `_unitOfWork.Repository<Product>()`**  
✔ **Dynamic repository access via `_unitOfWork["Product"]`**  

🚀 **Now your UnitOfWork system is fully automated and production-ready!** 🔥

