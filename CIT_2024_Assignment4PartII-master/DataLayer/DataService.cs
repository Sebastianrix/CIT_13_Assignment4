using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer
{
    public class DataService : IDataService
    {
        public readonly NorthwindContext _dbContext;


        public DataService(NorthwindContext? dbContext = null)
        {
            if (dbContext == null)
            {
                var options = new DbContextOptionsBuilder<NorthwindContext>()
                    .UseNpgsql("Host=localhost;Database=Northwind;Username=postgres;Password=yourpassword") // Adjust this as needed
                    .Options;

                _dbContext = new NorthwindContext(options);
            }
            else
            {
                _dbContext = dbContext;
            }
        }

        public IList<Category> GetCategories()
        {
            return _dbContext.Categories.ToList();
        }

        public Category? GetCategory(int requestedId)
        {
            return _dbContext.Categories.FirstOrDefault(c => c.Id == requestedId);
        }

        public Category CreateCategory(string categoryName, string categoryDescription)
        {
            int id = _dbContext.Categories.Max(x => x.Id) + 1;
            var category = new Category
            {
                Id = id,
                Name = categoryName,
                Description = categoryDescription
            };

            _dbContext.Categories.Add(category);
            _dbContext.SaveChanges();

            return category;
        }

        public bool DeleteCategory(int requestedId)
        {
            var category = _dbContext.Categories.FirstOrDefault(c => c.Id == requestedId);
            if (category == null) return false;

            _dbContext.Categories.Remove(category);
            _dbContext.SaveChanges();

            return true;
        }

        public bool UpdateCategory(int id, string name, string description)
        {
            var category = _dbContext.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null) return false;

            category.Name = name;
            category.Description = description;

            _dbContext.SaveChanges();

            return true;
        }


        public Product? GetProduct(int requestedId)
        {
            var product = _dbContext.Products.Include(p => p.Category)
                                             .FirstOrDefault(p => p.Id == requestedId);
            if (product?.Category != null)
            {
                product.CategoryName = product.Category.Name;
            }

            return product;
        }

        public IList<Product> GetProductByCategory(int requestedId)
        {
            var products = _dbContext.Products.Include(p => p.Category)
                                              .Where(p => p.CategoryId == requestedId)
                                              .OrderBy(p => p.Id)
                                              .ToList();

            foreach (var product in products)
            {
                if (product.Category != null)
                {
                    product.CategoryName = product.Category.Name;
                }
            }

            return products;
        }

        public IList<Product> GetProductByName(string requestedName)
        {
            var products = _dbContext.Products.Include(p => p.Category)
                                              .Where(p => p.Name.Contains(requestedName))
                                              .OrderBy(p => p.Id)
                                              .ToList();

            foreach (var product in products)
            {
                if (product.Category != null)
                {
                    product.CategoryName = product.Category.Name;
                }
            }

            return products;
        }

        public Order? GetOrder(int requestedId)
        {
            return _dbContext.Orders
                             .Include(o => o.OrderDetails)
                             .ThenInclude(od => od.Product)
                             .ThenInclude(p => p.Category)
                             .FirstOrDefault(o => o.Id == requestedId);
        }

        public IList<OrderDetails>? GetOrderDetailsByOrderId(int orderId)
        {
            return _dbContext.OrderDetails
                             .Include(od => od.Product)
                             .Where(od => od.OrderId == orderId)
                             .ToList();
        }

        public IList<OrderDetails>? GetOrderDetailsByProductId(int productId)
        {
            return _dbContext.OrderDetails
                             .Include(od => od.Order)
                             .Where(od => od.ProductId == productId)
                             .OrderBy(od => od.OrderId)
                             .ToList();
        }

        public IList<Order> GetOrders()
        {
            return _dbContext.Orders.Include(o => o.OrderDetails).ToList();
        }
    }
}
