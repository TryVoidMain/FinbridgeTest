using CalculationSite.Services;
using Serilog;

namespace CalculationSite
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host
                .UseSerilog((context, configuration) => configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .WriteTo.Console());

            var services = builder.Services;

            // Add services to the container.
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddScoped<ICalculationService, CalculationService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();
            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");
            app.MapControllers();

            Log.Information("App configured succesfully");

            app.Run();
        }
    }
}