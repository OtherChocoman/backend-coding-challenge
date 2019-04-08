using BackendChallenge.Application;
using BackendChallenge.Bootstrap;
using BackendChallenge.Domain;
using BackendChallenge.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BackendChallenge
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddTransient<SuggestionsService>();

            var cityList = CityListFactory.CreateCityList(this._configuration["CitiesFileLocation"], this._configuration["RegionCodesFileLocation"]);
            services.AddTransient<ICityRepository, CityRepository>(serviceProvider => new CityRepository(cityList));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
