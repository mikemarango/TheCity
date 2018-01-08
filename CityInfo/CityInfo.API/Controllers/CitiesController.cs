using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Data;
using Microsoft.AspNetCore.Mvc;
using CityInfo.API.Services.CityService;
using CityInfo.API.Models.DTOs;

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
            var results = new List<CityNoAttractionsDto>();
            foreach (var city in cities)
            {
                results.Add(new CityNoAttractionsDto
                {
                    Id = city.Id,
                    Description = city.Description,
                    Name = city.Name
                });
            }
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
                var cityResult = new CityDto()
                {
                    Id = city.Id,
                    Name = city.Name,
                    Description = city.Description
                };
                foreach (var attraction in city.Attractions)
                {
                    cityResult.Attractions.Add(
                        new AttractionDto()
                        {
                            Id = attraction.Id,
                            Name = attraction.Name,
                            Description = attraction.Description
                        });
                }

                return Ok(cityResult);
            }

            var cityNoAttractionResult = new CityNoAttractionsDto()
            {
                Id = city.Id,
                Description = city.Description,
                Name = city.Name
            };

            return Ok(cityNoAttractionResult);
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
