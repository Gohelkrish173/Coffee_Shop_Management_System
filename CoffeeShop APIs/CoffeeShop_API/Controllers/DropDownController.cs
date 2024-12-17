using CoffeeShop_API.DATA;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShop_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DropDownController : ControllerBase
    {
        private readonly DropDownRepository _dropDownRepository;

        public DropDownController(DropDownRepository dropDownRepository)
        {
            _dropDownRepository = dropDownRepository;
        }

        #region UserDropDown
        [HttpGet("UserDropDown")]
        public IActionResult UserDropDown()
        {
            var UDD = _dropDownRepository.LoadUserDropDown();
            return Ok(UDD);
        }
        #endregion

        #region ProductDropDown
        [HttpGet("ProductDropDown")]
        public IActionResult ProductDropDown()
        {
            var PDD = _dropDownRepository.LoadProductDropDown();
            return Ok(PDD);
        }
        #endregion

        #region CustomerDropDown
        [HttpGet("CustomerDropDown")]
        public IActionResult CustomerDropDown()
        {
            var CDD = _dropDownRepository.LoadCustomerDropDown();
            return Ok(CDD);
        }
        #endregion

        #region OrderDropDown
        [HttpGet("OrderDropDown")]
        public IActionResult OrderDronDown()
        {
            var ODD = _dropDownRepository.LoadOrderDropDown();
            return Ok(ODD);
        }
        #endregion

        #region StateDropDown
        [HttpGet("StateDropDown/{CountryID:int}")]
        public IActionResult StateDropDown(int CountryID)
        {
            var SDD = _dropDownRepository.GetStateDropDownByCountry(CountryID);
            return Ok(SDD);
        }
        #endregion

        #region CountryDropDown
        [HttpGet("CountryDropDown")]
        public IActionResult CountryDropDown() 
        {
            var CODD = _dropDownRepository.GetCountryDropDown();
            return Ok(CODD);
        }
        #endregion
    }
}
