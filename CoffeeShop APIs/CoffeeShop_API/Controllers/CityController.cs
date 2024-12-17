using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CoffeeShop_API.Models;
using CoffeeShop_API.DATA;

namespace CoffeeShop_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly CityRepository _cityRepository;

        public CityController(CityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }

        #region GetAllCities
        [HttpGet]
        public IActionResult Index()
        {
            var cities = _cityRepository.GetAllCities();

            return Ok(cities);
        }
        #endregion

        #region InsertCity
        [HttpPost]
        public IActionResult InsertCity(CityModel cmodel)
        {
            bool Reflected = _cityRepository.InsertCity(cmodel);
            return Ok(Reflected);
        }
        #endregion

        #region UpdateCity
        [HttpPut]
        public IActionResult UpdateCity(CityModel cityModel)
        {
            bool Reflected = _cityRepository.UpdateCity(cityModel);
            return Ok(Reflected);
        }
        #endregion

        #region DeleteCity
        [HttpDelete("{CityID:int}")]
        public IActionResult DeleteCity(int CityID)
        {
            bool Reflected = _cityRepository.DeleteCity(CityID);
            return Ok(Reflected);
        }
        #endregion

        #region GetCityByPK
        [HttpGet("{CityID:int}")]
        public IActionResult GetCityByPK(int CityID) 
        {
            CityModel cmodel = _cityRepository.GetCityByPK(CityID);
            return Ok(cmodel);
        }
        #endregion
    }
}
