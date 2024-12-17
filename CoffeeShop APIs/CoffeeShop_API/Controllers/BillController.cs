using CoffeeShop_API.DATA;
using CoffeeShop_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShop_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : ControllerBase
    {
        private readonly BillRepository _billRepository;

        public BillController(BillRepository billRepository)
        {
            _billRepository = billRepository;
        }

        #region GetAllBill
        [HttpGet]
        public IActionResult GetAllBill()
        {
            var Bills = _billRepository.GetAllBill();
            return Ok(Bills);
        }
        #endregion

        #region DeleteBill
        [HttpDelete("{BillID:int}")]
        public IActionResult DeleteBill(int BillID)
        {
            var Bills = _billRepository.DeleteBill(BillID);
            return Ok(Bills);
        }
        #endregion

        #region InsertBill
        [HttpPost]
        public IActionResult InsertBill(BillModel billModel)
        {
            var Bills = _billRepository.InsertBill(billModel);
            return Ok(Bills);
        }
        #endregion

        #region UpdateBill
        [HttpPut]
        public IActionResult UpdateBill(BillModel billModel)
        {
            var Bills = _billRepository.UpdateBill(billModel);
            return Ok(Bills);
        }
        #endregion

        #region GetByPkBill
        [HttpGet("{BillID:int}")]
        public IActionResult GetByPKBill(int BillID) 
        {
            BillModel bill = _billRepository.GetBillByPK(BillID);
            return Ok(bill);
        }
        #endregion
    }
}
