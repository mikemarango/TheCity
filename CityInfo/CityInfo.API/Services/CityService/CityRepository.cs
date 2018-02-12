using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Data;
using CityInfo.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Services.CityService
{
    public class CityRepository : ICityRepository
    {
        public CityContext Context { get; }

        public CityRepository(CityContext context)
        {
            Context = context;
        }

        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            return await Context.Cities.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<City> GetCityAsync(int cityId, bool includeAttractions)
        {
            if (includeAttractions)
                return await Context.Cities.Include(c => c.Attractions).Where(c => c.Id == cityId).FirstOrDefaultAsync();

            return await Context.Cities.Where(c => c.Id == cityId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Attraction>> GetAttractionsAsync(int cityId)
        {
            return await Context.Attractions.Where(a => a.CityId == cityId).ToListAsync();
        }

        public async Task<Attraction> GetAttractionAsync(int cityId, int attractionId)
        {
            return await Context.Attractions.Where(a => a.CityId == cityId && a.Id == attractionId).FirstOrDefaultAsync();
        }

        public async Task CreateAttractionAsync(int cityId, Attraction attraction)
        {
            var city = await GetCityAsync(cityId, false);
            city.Attractions.Add(attraction);
        }

        public async Task DeleteAttractionAsync(Attraction attraction)
        {
            Context.Attractions.Remove(attraction);
            await Context.SaveChangesAsync();
        }
        public async Task<bool> SaveAsync() => (await Context.SaveChangesAsync() >= 0);

        public async Task<bool> CityExistsAsync(int cityId)
        {
            return await Context.Cities.AnyAsync(c => c.Id == cityId);
        }

    }
}
