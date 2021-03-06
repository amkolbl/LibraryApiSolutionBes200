using AutoMapper;
using LibraryApi.AutomapperProfiles;
using LibraryApi.Controllers;
using LibraryApi.Domain;
using LibraryApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using RabbitMqUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LibraryApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRabbit(Configuration);
            services.AddScoped<IProcessReservations, RabbitMqReservationProcessor>();
            services.AddScoped<ILookupReservations, EfReservationsLookup>();
            services.AddScoped<IReservationCommands, EfReservationsLookup>();

            var configForProducts = new ProductInfoOptions();

            services.Configure<ProductInfoOptions>(Configuration.GetSection(ProductInfoOptions.SectionName));


            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = Configuration.GetConnectionString("redis");
            });
            services.AddScoped<ILookupOnCallDevelopers, RedisOnCallDeveloperLookup>();

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.IgnoreNullValues = true;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { 
                    Title = "Library Api",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Name = "Jeff Gonzalez",
                        Email = "jeff@hypertheory.com"
                    },
                    Description = "This is a demo api for the BES 100 Course"
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddTransient<ILookupServerStatus, WillsHeatlhCheckServerStatus>();

            services.AddDbContext<LibraryDataContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Library"));
            });

            var mapperConfig = new MapperConfiguration(options =>
            {
                options.AddProfile(new BooksProfile());
                options.AddProfile(new AutomapperReservationProfiles());
            });

            services.AddScoped<ILookupBooks, EfSqlBooksData>();
            services.AddScoped<IBookCommands, EfSqlBooksData>();

            var mapper = mapperConfig.CreateMapper();

            services.AddSingleton<IMapper>(mapper);
            services.AddSingleton<MapperConfiguration>(mapperConfig);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
               
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LibraryApi v1"));

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
