using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CityInfo.API.Data;
using CityInfo.API.Models.DTOs;
using CityInfo.API.Models.Entities;
using CityInfo.API.Services.CityService;
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
        public ICityRepository Repository { get; }
        public IMailService Email { get; }
        public ILogger<AttractionsController> Logger { get; }

        public AttractionsController(ICityRepository repository, IMailService email, ILogger<AttractionsController> logger)
        {
            Repository = repository;
            Email = email;
            Logger = logger;
        }
        // GET: api/attractions
        [HttpGet("{cityId}/attractions")]
        public async Task<IActionResult> Get(int cityId)
        {
            try
            {
                if (await Repository.CityExistsAsync(cityId) == false)
                {
                    Logger.LogInformation($"City with id {cityId} doesn't exist.");
                    return NotFound();
                }

                var cityAttractions = await Repository.GetAttractionsAsync(cityId);
                var results = Mapper.Map<IEnumerable<AttractionDto>>(cityAttractions);
                return Ok(results);

            }
            catch (Exception ex)
            {
                Logger.LogCritical($"None existent City with id {cityId}", ex);
                return StatusCode(500, "An error occured while handling your request.");
            }
        }

        // GET api/cities/attractions/5
        [HttpGet("{cityId}/attractions/{id}", Name = "GetAttraction")]
        public async Task<IActionResult> Get(int cityId, int id)
        {

            if (await Repository.CityExistsAsync(cityId) == false)
            {
                Logger.LogInformation($"City with id {cityId} doesn't exist.");
                return NotFound();
            }

            var attraction = await Repository.GetAttractionAsync(cityId, id);

            if (attraction == null)
                return NotFound();

            var result = Mapper.Map<AttractionDto>(attraction);
            return Ok(result);

        }

        // POST api/cities/attractions
        [HttpPost("{cityId}/attractions")]
        public async Task<IActionResult> Post(int cityId, [FromBody]AttractionCreateDto attraction)
        {
            if (attraction == null)
                return BadRequest();

            if (attraction.Description == attraction.Name)
                ModelState.AddModelError("Description", "The description should be different from the name.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await Repository.CityExistsAsync(cityId)) return NotFound();

            var finalAttraction = Mapper.Map<Attraction>(attraction);

            await Repository.CreateAttractionAsync(cityId, finalAttraction);

            if (!await Repository.SaveAsync())
                return StatusCode(500, "A problem occured while processing your request!");

            var createdAttraction = Mapper.Map<AttractionDto>(finalAttraction);

            return CreatedAtRoute("GetAttraction", new { cityId, id = createdAttraction.Id }, createdAttraction);
        }

        // PUT api/cities/attractions/5 (Full update)
        [HttpPut("{cityId}/attractions/{id}")]
        public async Task<IActionResult> Put(int cityId, int id,  [FromBody]AttractionUpdateDto attraction)
        {
            if (attraction == null)
                return BadRequest();

            if (attraction.Description == attraction.Name)
                ModelState.AddModelError("Description", "The description should be different from the name.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await Repository.CityExistsAsync(cityId))
                return NotFound();

            var cityAttraction = await Repository.GetAttractionAsync(cityId, id);

            if (cityAttraction == null)
                return NotFound();

            Mapper.Map(attraction, cityAttraction);

            if (!await Repository.SaveAsync())
                return StatusCode(500, "An error occured while handling your request.");

            return NoContent();
        }

        [HttpPatch("{cityId}/attractions/{id}")]
        public async Task<IActionResult> Patch(int cityId, int id, [FromBody]JsonPatchDocument<AttractionUpdateDto> document)
        {
            if (document == null)
                return BadRequest();

            if (!await Repository.CityExistsAsync(cityId))
                return NotFound();

            var cityAttraction = await Repository.GetAttractionAsync(cityId, id);

            if (cityAttraction == null)
                return NotFound();

            var attractionPatch = Mapper.Map<AttractionUpdateDto>(cityAttraction);

            document.ApplyTo(attractionPatch, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (attractionPatch.Description == attractionPatch.Name)
                ModelState.AddModelError("Description", "The description must be different from name");

            TryValidateModel(attractionPatch);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Mapper.Map(attractionPatch, cityAttraction);

            if (!await Repository.SaveAsync())
                return StatusCode(500, "An error occured while processing your request.");

            return NoContent();
        }


        // DELETE api/cities/attractions/5
        [HttpDelete("{cityId}/attractions/{id}")]
        public async Task<IActionResult> Delete(int cityId, int id)
        {

            if (!await Repository.CityExistsAsync(cityId))
                return NotFound();

            var attraction = await Repository.GetAttractionAsync(cityId, id);
            if (attraction == null)
                return NotFound();

            await Repository.DeleteAttractionAsync(attraction);

            if (!await Repository.SaveAsync())
                return StatusCode(500, "An error occured while processing your request.");

            await Email.SendEmailAsync("Attraction for city was deleted.",
                $"Attraction {attraction.Name} with id {attraction.Id} was deleted.");

            return NoContent();
        }
    }
}
