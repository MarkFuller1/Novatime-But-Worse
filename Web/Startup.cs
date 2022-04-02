using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Time.Interfaces;
using Time.Repositories;
using Time.Services;



namespace Time
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

            services.AddTransient<IHoursService, HoursService>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Time", Version = "v1" });
            });

            services.AddDbContext<EmployeeTimesContext>(options =>
            options
                .UseNpgsql(Configuration.GetConnectionString("EmployeeTimeContext"))
                .UseSnakeCaseNamingConvention()
                .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
                .EnableSensitiveDataLogging()
            );

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddCors();
            /*                (c =>
                        {
                            c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
                            c.AddPolicy("Access-Control-Allow-Methods", options => options.AllowAnyMethod());
                            c.AddPolicy("Access-Control-Request-Headers", options => options.AllowAnyHeader());
                        });
            */
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Time v1");
                    c.RoutePrefix = "";
                });
            }

            // disable https
            //app.UseHttpsRedirection();

            app.UseRouting();

            enableCors(app);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                enableCors(app);
                endpoints.MapControllers();
                enableCors(app);
            });

            enableCors(app);
        }

        private IApplicationBuilder enableCors(IApplicationBuilder app)
        {
            app.UseCors(x => x
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .SetIsOriginAllowed(origin => true) // allow any origin
                                                      //.WithOrigins("https://localhost:44351")); // Allow only this origin can also have multiple origins separated with comma
                  .AllowCredentials()); // allow credentials
            return app;
        }
    }
}
