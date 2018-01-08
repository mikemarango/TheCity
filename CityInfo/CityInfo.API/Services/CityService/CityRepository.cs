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

        public IEnumerable<City> GetCities()
        {
            return Context.Cities.OrderBy(c => c.Name).ToList();
        }

        public City GetCity(int cityId, bool includeAttractions)
        {
            if (includeAttractions)
                return Context.Cities.Include(c => c.Attractions).Where(c => c.Id == cityId).FirstOrDefault();

            return Context.Cities.Where(c => c.Id == cityId).FirstOrDefault();
        }

        public IEnumerable<Attraction> GetAttractions(int cityId)
        {
            return Context.Attractions.Where(a => a.CityId == cityId).ToList();
        }

        public Attraction GetAttraction(int cityId, int attractionId)
        {
            return Context.Attractions.Where(a => a.CityId == cityId && a.Id == attractionId).FirstOrDefault();
        }

        public bool CityExists(int cityId) => 
            Context.Cities.Any(c => c.Id == cityId);

        public void CreateAttraction(int cityId, Attraction attraction)
        {
            var city = GetCity(cityId, false);
            city.Attractions.Add(attraction);
        }

        public bool Save() => (Context.SaveChanges() >= 0);
    }
}
