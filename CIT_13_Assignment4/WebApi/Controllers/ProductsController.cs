using DataLayer;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IDataService _dataService;

        public ProductsController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            var product = _dataService.GetProduct(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpGet("category/{categoryId}")]
        public IActionResult GetProductsByCategory(int categoryId)
        {
            var products = _dataService.GetProductByCategory(categoryId);

            if (products == null || !products.Any())
            {
                return NotFound(new List<Product>());
            }

            return Ok(products);
        }

        [HttpGet]
        public IActionResult GetProductsByName([FromQuery] string name)
        {
            var products = _dataService.GetProductByName(name);

            if (products == null || !products.Any())
            {
                return NotFound(new List<Product>());
            }

            return Ok(products);
        }

    }
}
