using CoffeeShop_API.DATA;
using CoffeeShop_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShop_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerRepository customerRepository;

        public CustomerController(CustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        #region GetAllCustomer
        [HttpGet]
        public IActionResult GetAllCustomer()
        {
            var customer = customerRepository.GetAllCustomer();
            return Ok(customer);
        }
        #endregion

        #region DeleteCustomer
        [HttpDelete("{CustomerID:int}")]
        public IActionResult DeleteCustomer(int CustomerID)
        {
            bool responce = customerRepository.DeleteCustomer(CustomerID);
            return Ok(responce);
        }
        #endregion

        #region InsertCustomer
        [HttpPost]
        public IActionResult InsertCustomer(CustomerModel customerModel)
        {
            bool responce = customerRepository.InsertCustomer(customerModel);
            return Ok(responce);
        }
        #endregion

        #region UpdateCustomer
        [HttpPut]
        public IActionResult UpdateCustomer(CustomerModel customerModel)
        {
            bool customer = customerRepository.UpdateCustomer(customerModel);
            return Ok(customer);
        }
        #endregion

        #region GetCustomerByPK
        [HttpGet("{CustomerID:int}")]
        public IActionResult GetCustomerByPK(int CustomerID)
        {
            CustomerModel customerModel = customerRepository.GetCustomerByPK(CustomerID);
            return Ok(customerModel);
        }
        #endregion
    }
}
