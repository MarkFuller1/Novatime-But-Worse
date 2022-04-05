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
        private const string AllowedOrigins = "AllowAll";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public string AllowAllCorsPolicy { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCors(options =>
            {
                options.AddPolicy(AllowedOrigins,
                    builder => builder
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin()
                        .AllowCredentials());
            });

            services.AddTransient<IHoursService, HoursService>();
            services.AddTransient<IExcelService, ExcelService>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Time", Version = "v1" });
            });

            services.AddDbContext<EmployeeTimesContext>(options =>
            options
                .UseNpgsql(Configuration.GetConnectionString("EmployeeTimeContext"))
                .UseSnakeCaseNamingConvention()
                .UseLoggerFactory(LoggerFactory.Create(builder => { builder.AddConsole(); builder.AddDebug(); }))
                .EnableSensitiveDataLogging()
            );

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllCorsPolicy", builder =>
                {
                    builder
                    .SetIsOriginAllowed(x => _ = true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
                });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
            });

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
            app.UseCors(AllowedOrigins);


            app.UseAuthorization();
            enableCors(app);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

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
