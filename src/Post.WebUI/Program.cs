using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Post.Infrastructure.Identity;
using Post.Infrastructure.Persistence;

using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

namespace Post.WebUI
{
    public class Program
    {
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                       .ConfigureLogging(logging =>
                       {
                           logging.ClearProviders();
                       })
                       .UseSerilog()
                       .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }

        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                         .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                         .Enrich.FromLogContext()
                         .WriteTo.Console()
                         .CreateLogger();

            try
            {

                var host = CreateHostBuilder(args).Build();

                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;

                    try
                    {
                        var context = services.GetRequiredService<ApplicationDbContext>();

                        if (context.Database.IsSqlServer())
                        {
                            await context.Database.MigrateAsync();
                            Log.Logger = new LoggerConfiguration()
                                         .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                                         .Enrich.FromLogContext()
                                         .WriteTo.Console()
                                         .WriteTo.MSSqlServer(context.Database.GetConnectionString(), new MSSqlServerSinkOptions() { TableName = "Log", AutoCreateSqlTable = true })
                                         .CreateLogger();
                        }

                        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                        await ApplicationDbContextSeed.SeedDefaultUserAsync(userManager, roleManager);
                        await ApplicationDbContextSeed.SeedSampleDataAsync(context);
                    }
                    catch (Exception ex)
                    {
                        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

                        logger.LogError(ex, "An error occurred while migrating or seeding the database.");

                        throw;
                    }
                }

                await host.RunAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}