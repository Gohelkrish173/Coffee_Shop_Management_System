using CoffeeShop_API.DATA;
using CoffeeShop_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShop_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductRepository productRepository;

        public ProductController(ProductRepository productRepository) 
        {
            this.productRepository = productRepository;
        }

        #region GetAllProduct
        [HttpGet]
        public IActionResult GetAllProduct()
        {
            var products = productRepository.GetAllProduct();
            return Ok(products);
        }
        #endregion

        #region DeleteProduct
        [HttpDelete("{ProductID:int}")]
        public IActionResult DeleteProduct(int ProductID)
        {
            bool responce = productRepository.DeleteProduct(ProductID);
            return Ok(responce);
        }
        #endregion

        #region InsertProduct
        [HttpPost]
        public IActionResult InsertProduct(ProductModel productModel)
        {
            bool responce = productRepository.InsertProduct(productModel);
            return Ok(responce);
        }
        #endregion

        #region UpdateProduct
        [HttpPut]
        public IActionResult UpdateProduct(ProductModel productModel)
        {
            bool responce = productRepository.UpdateProduct(productModel);
            return Ok(responce);
        }
        #endregion

        #region GetProductByPK
        [HttpGet("{ProductID:int}")]
        public IActionResult GetProductByPK(int ProductID)
        {
            ProductModel pmodel = productRepository.GetProductByPK(ProductID);
            return Ok(pmodel);
        }
        #endregion
    }
}
