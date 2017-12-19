using CityInfo.API.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Data
{
    public class CityData
    {
        public static CityData Current { get; } = new CityData();
        public List<CityDto> Cities { get; set; }
        public CityData()
        {
            Cities = new List<CityDto>()
            {
                new CityDto
                {
                    Id = 1,
                    Name = "New York City",
                    Description = "The one with that big park.",
                    Attractions = new List<AttractionDto>
                    {
                        new AttractionDto()
                        {
                            Id = 1,
                            Name = "Central Park",
                            Description = "The most visited urban park in the world!"
                        },
                        new AttractionDto()
                        {
                            Id = 2,
                            Name = "Empire State Building",
                            Description = "A 102-story skyscrapper located in Midtown Manhatan."
                        }
                    }
                },
                new CityDto
                {
                    Id = 2,
                    Name = "Antwerp",
                    Description = "The one with the cathedral that was never really finished.",
                    Attractions = new List<AttractionDto>
                    {
                        new AttractionDto()
                        {
                            Id = 3,
                            Name = "Cathedral of Our Lady",
                            Description = "A Gothic style cathedral, conceived by architects Jan and Pieter Appelmans."
                        },
                        new AttractionDto()
                        {
                            Id = 4,
                            Name = "Antwerp Central Station",
                            Description = "The finest example of railway architecture in Belgium."
                        }
                    }
                },
                new CityDto
                {
                    Id = 3,
                    Name = "Paris",
                    Description = "The one with that big tower.",
                    Attractions = new List<AttractionDto>
                    {
                        new AttractionDto()
                        {
                            Id = 5,
                            Name = "Eiffel Tower",
                            Description = "A wrought iron lattice tower on the Champ de Mars, named after engineer Gustave Eiffel."
                        },
                        new AttractionDto()
                        {
                            Id = 6,
                            Name = "The Louvre",
                            Description = "The world's largest museum."
                        }
                    }
                }
            };
        }
    }
}
