using CoffeeShop_API.DATA;
using CoffeeShop_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShop_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderRepository orderRepository;

        public OrderController(OrderRepository orderRepository) 
        {
            this.orderRepository = orderRepository;
        }

        #region GetAllOrders
        [HttpGet]
        public IActionResult GetAllOrders()
        {
            var orders = orderRepository.GetAllOrders();
            return Ok(orders);
        }
        #endregion

        #region DeleteOrder
        [HttpDelete]
        public IActionResult DeleteOrder(int OrderID)
        {
            bool orders = orderRepository.DeleteOrder(OrderID);
            return Ok(orders);
        }
        #endregion

        #region InsertOrder
        [HttpPost]
        public IActionResult InsertOrder(OrderModel orderModel)
        {
            var orders = orderRepository.InsertOrder(orderModel);
            return Ok(orders);
        }
        #endregion

        #region UpdateOrder
        [HttpPut]
        public IActionResult UpdateOrder(OrderModel orderModel)
        {
            var orders = orderRepository.UpdateOrder(orderModel);
            return Ok(orders);
        }
        #endregion

        #region GetOrderByPK
        [HttpGet("{OrderID:int}")]
        public IActionResult GetOrderByPK(int OrderID) 
        {
            OrderModel order = orderRepository.GetOrderByPK(OrderID);
            return Ok(order);
        }
        #endregion
    }
}
