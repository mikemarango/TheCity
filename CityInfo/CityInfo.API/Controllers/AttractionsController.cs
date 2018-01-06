using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Data;
using CityInfo.API.Models.DTOs;
using CityInfo.API.Services.EmailService;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class AttractionsController : Controller
    {
        public IMailService Email { get; }
        public ILogger<AttractionsController> Logger { get; }

        public AttractionsController(IMailService email, ILogger<AttractionsController> logger)
        {
            Email = email;
            Logger = logger;
        }
        // GET: api/attractions
        [HttpGet("{cityId}/attractions")]
        public IActionResult Get(int cityId)
        {
            try
            {
                var city = CityData.Current.Cities.FirstOrDefault(c => c.Id == cityId);
                if (city == null)
                {
                    Logger.LogInformation($"City with id {cityId} doesn't exist.");
                    return NotFound();
                }
                return Ok(city.Attractions);

            }
            catch (Exception ex)
            {
                Logger.LogCritical($"None existent City with id {cityId}", ex);
                return StatusCode(500, "An error occured while handling your request.");
            }
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

        [HttpPatch("{cityId}/attractions/{id}")]
        public IActionResult Patch(int cityId, int id, [FromBody]JsonPatchDocument<AttractionUpdateDto> document)
        {
            if (document == null)
                return BadRequest();

            var city = CityData.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
                return NotFound();

            var attractionDto = city.Attractions.FirstOrDefault(a => a.Id == id);

            if (attractionDto == null)
                return NotFound();

            var attractionToPatch = new AttractionUpdateDto()
            {
                Name = attractionDto.Name,
                Description = attractionDto.Description
            };

            document.ApplyTo(attractionToPatch, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (attractionToPatch.Description == attractionToPatch.Name)
                ModelState.AddModelError("Description", "The description must be different from name");

            TryValidateModel(attractionToPatch);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            attractionDto.Name = attractionToPatch.Name;
            attractionDto.Description = attractionToPatch.Description;

            return NoContent();
        }


        // DELETE api/cities/attractions/5
        [HttpDelete("{cityId}/attractions/{id}")]
        public IActionResult Delete(int cityId, int id)
        {
            var city = CityData.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
                return NotFound();
            var attractionFromStore = city.Attractions.FirstOrDefault(a => a.Id == id);
            if (attractionFromStore == null)
                return NotFound();

            city.Attractions.Remove(attractionFromStore);
            Email.Send("Attraction for city was deleted.",
                $"Attraction {attractionFromStore.Name} with id {attractionFromStore.Id} was deleted.");

            return NoContent();
        }
    }
}
