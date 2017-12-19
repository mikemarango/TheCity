using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Models.DTOs
{
    public class CityDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumberOfAttractions => Attractions.Count;

        public ICollection<AttractionDto> Attractions { get; set; }
    }
}
