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
    public class CitiesController : Controller
    {
        // GET: api/cities
        [HttpGet]
        public IActionResult Get()
        {
            return base.Ok(CityData.Current.Cities);
        }

        // GET api/cities/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (CityData.Current.Cities.FirstOrDefault(c => c.Id == id) == null)
            {
                return NotFound();
            }
            return base.Ok(CityData.Current.Cities.FirstOrDefault(c => c.Id == id));
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
