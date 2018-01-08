using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Data;
using Microsoft.AspNetCore.Mvc;
using CityInfo.API.Services.CityService;
using CityInfo.API.Models.DTOs;
using AutoMapper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class CitiesController : Controller
    {
        public ICityRepository CityRepository { get; }

        public CitiesController(ICityRepository cityRepository)
        {
            CityRepository = cityRepository;
        }

        // GET: api/cities
        [HttpGet]
        public IActionResult Get()
        {
            var cities = CityRepository.GetCities();
            var results = Mapper.Map<IEnumerable<CityNoAttractionsDto>>(cities);
            return Ok(results);
        }

        // GET api/cities/5
        [HttpGet("{id}")]
        public IActionResult Get(int id, bool includeAttractions = false)
        {
            var city = CityRepository.GetCity(id, includeAttractions);

            if (city == null)
                return NotFound();

            if (includeAttractions)
            {
                var cityResult = Mapper.Map<CityDto>(city);
                return Ok(cityResult);
            }

            var cityNoAttractionsResult = Mapper.Map<CityNoAttractionsDto>(city);
            return Ok(cityNoAttractionsResult);
        }

        // POST api/cities
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/cities/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/cities/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
