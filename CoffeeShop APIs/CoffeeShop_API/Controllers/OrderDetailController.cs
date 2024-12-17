using CoffeeShop_API.DATA;
using CoffeeShop_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShop_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly OrderDetailRepository orderDetailRepository;

        public OrderDetailController(OrderDetailRepository orderDetailRepository)
        {
            this.orderDetailRepository = orderDetailRepository;
        }

        #region GetAllOrderDetail
        [HttpGet]
        public IActionResult GetAllOrderDetail() 
        {
            var orderDetail = orderDetailRepository.GetAllOrderDetail();
            return Ok(orderDetail);
        }
        #endregion

        #region DeleteOrderDetail
        [HttpDelete("{OrderDetailID:int}")]
        public IActionResult DeleteOrderDetail(int OrderDetailID)
        {
            bool orderDetail = orderDetailRepository.DeleteOrderDetail(OrderDetailID);
            return Ok(orderDetail);
        }
        #endregion

        #region InsertOrderDetail
        [HttpPost]
        public IActionResult InsertOrderDetail(OrderDetailModel orderDetailModel)
        {
            bool orderDetail = orderDetailRepository.InsertOrderDetail(orderDetailModel);
            return Ok(orderDetail);
        }
        #endregion

        #region UpdateOrderDetail
        [HttpPut]
        public IActionResult UpdateOrderDetail(OrderDetailModel orderDetailModel)
        {
            bool orderDetail = orderDetailRepository.UpdateOrderDetail(orderDetailModel);
            return Ok(orderDetail);
        }
        #endregion

        #region GetOrderByPK
        [HttpGet("{OrderDetailID:int}")]
        public IActionResult GetOrderDetailByPK(int OrderDetailID)
        {
            OrderDetailModel OD = orderDetailRepository.GetOrderDetailByPK(OrderDetailID);
            return Ok(OD);
        }
        #endregion
    }
}
