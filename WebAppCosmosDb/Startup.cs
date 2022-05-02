using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppCosmosDb.Services;

namespace WebAppCosmosDb
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
            services.AddControllersWithViews();
            services.AddSingleton<ICosmosDbService>(
                InitialiseCosmosClientAsync(Configuration.GetSection("CosmosDb")).GetAwaiter().GetResult());

        }
        static async Task<CosmosDbService> InitialiseCosmosClientAsync(IConfigurationSection configSection)
        {
            string databaseName = configSection.GetSection("databaseName").Value;
            string containerName = configSection.GetSection("containerName").Value;
            string account = configSection.GetSection("Account").Value;
            string key = configSection.GetSection("Key").Value;

            CosmosClient cosmosClient = new CosmosClient(account, key);
            CosmosDbService cosmosDbService = new CosmosDbService(cosmosClient, databaseName, containerName);

            DatabaseResponse database = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseName);

            await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");

            return cosmosDbService;

        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Items}/{action=Index}/{id?}");
            });
        }
    }
}