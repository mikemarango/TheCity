using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Data;
using CityInfo.API.Models.DTOs;
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

        // GET api/cities/attractions/5
        [HttpGet("{cityId}/attractions/{id}", Name = "GetAttraction")]
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

        // POST api/cities/attractions
        [HttpPost("{cityId}/attractions")]
        public IActionResult Post(int cityId, [FromBody]AttractionCreateDto attraction)
        {
            if (attraction == null)
                return BadRequest();

            if (attraction.Description == attraction.Name)
                ModelState.AddModelError("Description", "The description should be different from the name.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var city = CityData.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
                return NotFound();

            var maxAttractionId = 
                CityData.Current.Cities.SelectMany(c => c.Attractions).Max(a => a.Id);

            var finalAttraction = new AttractionDto()
            {
                Id = ++maxAttractionId,
                Name = attraction.Name,
                Description = attraction.Description
            };

            city.Attractions.Add(finalAttraction);

            return CreatedAtRoute("GetAttraction", new { cityId, id = finalAttraction.Id }, finalAttraction);
        }

        // PUT api/cities/attractions/5 (Full update)
        [HttpPut("{cityId}/attractions/{id}")]
        public IActionResult Put(int cityId, int id,  [FromBody]AttractionUpdateDto attraction)
        {
            if (attraction == null)
                return BadRequest();

            if (attraction.Description == attraction.Name)
                ModelState.AddModelError("Description", "The description should be different from the name.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var city = CityData.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
                return NotFound();

            var attractionDto = city.Attractions.FirstOrDefault(a => a.Id == id);

            if (attractionDto == null)
                return NotFound();

            attractionDto.Name = attraction.Name;
            attractionDto.Description = attraction.Description;

            return NoContent();
        }

        // DELETE api/cities/attractions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
