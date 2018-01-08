using CityInfo.API.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Services.CityService
{
    public interface ICityRepository
    {
        IEnumerable<City> GetCities();
        City GetCity(int cityId, bool includeAttractions);
        IEnumerable<Attraction> GetAttractions(int cityId);
        Attraction GetAttraction(int cityId, int attractionId);
        bool CityExists(int cityId);
        void CreateAttraction(int cityId, Attraction attraction);
        bool Save();
    }
}
