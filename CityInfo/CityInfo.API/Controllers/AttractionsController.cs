using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Data;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class AttractionsController : Controller
    {
        // GET: api/attractions
        [HttpGet("{cityId}/attractions")]
        public IActionResult Get(int cityId)
        {
            var city = CityData.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            return Ok(city.Attractions);
        }

        // GET api/attractions/5
        [HttpGet("{cityId}/attractions/{id}")]
        public IActionResult Get(int cityId, int id)
        {
            var city = CityData.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            var attraction = city.Attractions.FirstOrDefault(a => a.Id == id);
            if (attraction == null)
            {
                return NotFound();
            }
            return Ok(attraction);
        }

        // POST api/attractions
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/attractions/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/attractions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
