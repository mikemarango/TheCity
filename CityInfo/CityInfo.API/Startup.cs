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
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog.Extensions.Logging;

namespace CityInfo.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddXmlDataContractSerializerFormatters();

#if DEBUG
            services.AddTransient<IMailService, LocalEmail>();
#else
            services.AddTransient<IMailService, CloudEmail>();
#endif

            services.AddDbContext<CityContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ICityRepository, CityRepository>();
        }

            // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory logger)
        {
            logger.AddConsole();
            logger.AddDebug();
            logger.AddNLog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }

            app.UseStatusCodePages();

            Mapper.Initialize(config =>
            {
                config.CreateMap<City, CityNoAttractionsDto>();
                config.CreateMap<City, CityDto>();
                config.CreateMap<Attraction, AttractionDto>();
                config.CreateMap<AttractionCreateDto, Attraction>();
            });

            app.UseMvc();
        }
    }
}
