using CityInfo.API.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Services.CityService
{
    public interface ICityRepository
    {
        Task<IEnumerable<City>> GetCitiesAsync();
        Task<City> GetCityAsync(int cityId, bool includeAttractions);
        Task<IEnumerable<Attraction>> GetAttractionsAsync(int cityId);
        Task<Attraction> GetAttractionAsync(int cityId, int attractionId);
        Task<bool> CityExistsAsync(int cityId);
        Task CreateAttractionAsync(int cityId, Attraction attraction);
        Task<bool> SaveAsync();
        Task DeleteAttractionAsync(Attraction attraction);
    }
}
