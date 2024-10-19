using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataLayer;

public class DataService
{
    public IList<Category> GetCategories()
    {
        var db = new NorthwindContext();
        return db.Categories.ToList();
    }
    public Category GetCategory(int requested_ID)
    {
        var db = new NorthwindContext();

        var myCategory = db.Categories.FirstOrDefault(c => c.Id == requested_ID);
        return myCategory;
    }
    public Category CreateCategory(string categoryName, string categoryDescription)
    {
        var db = new NorthwindContext();
        int id = db.Categories.Max(x => x.Id) + 1;
        var myCategory = new Category
        {
            Id = id,
            Name = categoryName,
            Description = categoryDescription
        };

        db.Categories.Add(myCategory);

        db.SaveChanges();

        return myCategory;
    }
    public bool DeleteCategory(int requested_ID)
    {
        var db = new NorthwindContext();
        var myCategory = db.Categories.FirstOrDefault(c => c.Id == requested_ID);
        if (myCategory == null) { return false; }
        else
        {
            db.Categories.Remove(myCategory);
            db.SaveChanges();

            return true;
        }
    }
    public bool UpdateCategory(int requested_ID, string categoryName, string categoryDescription)
    {
        var db = new NorthwindContext();
        var myCategory = db.Categories.FirstOrDefault(c => c.Id == requested_ID);
        if (myCategory == null) { return false; }
        else
        {
            myCategory.Id = requested_ID;
            myCategory.Name = categoryName;
            myCategory.Description = categoryDescription;

            db.SaveChanges();

            return true;
        }
    }
    public Product GetProduct(int requested_ID)
    {
        using var db = new NorthwindContext();
        var myProduct = db.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == requested_ID);
        
        myProduct.CategoryName = myProduct.Category.Name;

        return myProduct;
    }

    public IList<Product> GetProductByCategory(int requested_ID)
    {
        using var db = new NorthwindContext();
        var products = db.Products.Include(p => p.Category)
                                  .Where(p => p.CategoryId == requested_ID)
                                  .OrderBy(p => p.Id) // Ensure consistent order
                                  .ToList();

        // Populate CategoryName manually
        foreach (var product in products)
        {
            if (product.Category != null)
            {
                product.CategoryName = product.Category.Name;
            }
        }

        return products;
    }
    
    public IList<Product> GetProductByName(string requested_Name)
    {
        using var db = new NorthwindContext();
        var products = db.Products.Include(p => p.Category)
                                  .Where(p => p.Name.Contains(requested_Name))
                                  .OrderBy(p => p.Id) // Ensure consistent order
                                  .ToList();

        // Populate CategoryName manually
        foreach (var product in products)
        {
            if (product.Category != null)
            {
                product.CategoryName = product.Category.Name;
            }
        }

        return products;
    }

    public Order GetOrder(int requested_ID)
    {
        using var db = new NorthwindContext();
        var myOrder = db.Orders
                        .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Product)
                        .ThenInclude(p => p.Category)
                        .FirstOrDefault(o => o.Id == requested_ID);
        return myOrder;
    }


    public List<OrderDetails> GetOrderDetailsByOrderId(int orderId)
    {
        using var db = new NorthwindContext();
        return db.OrderDetails
                 .Include(od => od.Product)
                 .Where(od => od.OrderId == orderId)
                 .ToList();
    }

    public List<OrderDetails> GetOrderDetailsByProductId(int productId)
    {
        using var db = new NorthwindContext();
        return db.OrderDetails
                 .Include(od => od.Order)
                 .Where(od => od.ProductId == productId)
                 .OrderBy(od => od.OrderId)
                 .ToList();
    }

    public List<Order> GetOrders()
    {
        using var db = new NorthwindContext();
        return db.Orders.Include(o => o.OrderDetails).ToList();
    }
    //public Product CreateProduct(string productName, double unitPrice, string quantityPerUnit, int unitsInStock, int categoryID)
    //{
    //    using var db = new NorthwindContext();
    //    int id = db.Products.Max(x => x.Id) + 1;
    //    var myProduct = new Product
    //    {
    //        Id = id,
    //        Name = productName,
    //        UnitPrice = unitPrice,
    //        QuantityPerUnit = quantityPerUnit,
    //        UnitsInStock = unitsInStock,
    //        CategoryId = categoryID
    //    };

    //    db.Products.Add(myProduct);

    //    db.SaveChanges();

    //    return myProduct;
    //}
    //public bool DeleteProduct(int requested_ID)
    //{
    //    using var db = new NorthwindContext();
    //    var myProduct = db.Products.FirstOrDefault(p => p.Id == requested_ID);
    //    if (myProduct == null) { return false; }
    //    else
    //    {
    //        db.Products.Remove(myProduct);
    //        db.SaveChanges();

    //        return true;
    //    }
    //}
    //public bool UpdateProduct(int requested_ID, string productName, double unitPrice, string quantityPerUnit, int unitsInStock, int categoryID)
    //{
    //    using var db = new NorthwindContext();
    //    var myProduct = db.Products.FirstOrDefault(p => p.Id == requested_ID);
    //    if (myProduct == null) { return false; }
    //    else
    //    {
    //        myProduct.Id = requested_ID;
    //        myProduct.Name = productName;
    //        myProduct.UnitPrice = unitPrice;
    //        myProduct.QuantityPerUnit = quantityPerUnit;
    //        myProduct.UnitsInStock = unitsInStock;
    //        myProduct.CategoryId = categoryID;

    //        db.SaveChanges();

    //        return true;
    //    }
    //}


}







