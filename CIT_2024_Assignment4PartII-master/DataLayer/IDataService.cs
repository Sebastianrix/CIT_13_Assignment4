using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer;
public interface IDataService
{
    IList<Category> GetCategories();

    Category? GetCategory(int id);

    Category? CreateCategory(string name, string description);

    bool DeleteCategory(int id);

    bool UpdateCategory(int id, string name, string description);

    //ProductWithCategoryName? GetProduct(int id);

    //IList<ProductWithCategoryName>? GetProductByCategory(int id);

    //IList<ProductWithCategoryName>? GetProductByName(string name);

    Order? GetOrder(int id);

    IList<Order> GetOrders();

    IList<OrderDetails>? GetOrderDetailsByOrderId(int id);

    IList<OrderDetails>? GetOrderDetailsByProductId(int id);

}
