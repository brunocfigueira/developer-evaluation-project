using Ambev.DeveloperEvaluation.Application;
using Ambev.DeveloperEvaluation.Application.Checkout.OrderConfirmed;
using Ambev.DeveloperEvaluation.Application.Checkout.SaleConfirmed;
using Ambev.DeveloperEvaluation.Common.HealthChecks;
using Ambev.DeveloperEvaluation.Common.Logging;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.IoC;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.WebApi.Mappings;
using Ambev.DeveloperEvaluation.WebApi.Middleware;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Rebus.Transport.InMem;
using Serilog;

namespace Ambev.DeveloperEvaluation.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            Log.Information("Starting web application");

            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            builder.AddDefaultLogging();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.AddBasicHealthChecks();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<DefaultContext>(options =>
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("Ambev.DeveloperEvaluation.ORM")
                )
            );

            builder.Services.AddJwtAuthentication(builder.Configuration);

            builder.RegisterDependencies();

            builder.Services.AddAutoMapper(
                typeof(Program).Assembly,
                typeof(ApplicationLayer).Assembly
            );

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(
                    typeof(ApplicationLayer).Assembly,
                    typeof(Program).Assembly
                );
            });

            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            builder.Services.AddWebApiMappings();
            builder.Services.ConfigureRebus();

            var app = builder.Build();
            app.UseMiddleware<ValidationExceptionMiddleware>();
            app.UseMiddleware<BusinessRuleExceptionMiddleware>();
            app.UseMiddleware<ResourceNotFoundExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseBasicHealthChecks();

            app.MapControllers();

            app.Run();

            var rebus = app.Services.GetRequiredService<IBusStarter>();
            rebus.Start();
        }
        catch (AggregateException ex)
        {
            foreach (var inner in ex.InnerExceptions)
            {
                Console.WriteLine(inner.GetType().Name + ": " + inner.Message);
                Console.WriteLine(inner.StackTrace);
            }
            throw;
        }
        catch (InvalidOperationException ex)
        {

            Console.WriteLine(ex.GetType().Name + ": " + ex.Message);
            Console.WriteLine(ex.StackTrace);
            throw;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}

public static class ServiceCollectionExtensions
{
    public static void AddWebApiMappings(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<ProductMappingProfile>();
            cfg.AddProfile<UserMappingProfile>();
            cfg.AddProfile<SaleMappingProfile>();
            cfg.AddProfile<CartMappingProfile>();
        });
    }

    public static void ConfigureRebus(this IServiceCollection services)
    {
        const string ORDER_QUEUE = "order-confirmation-queue";
        const string SALE_QUEUE = "sale-confirmation-queue";

        var orderNetwork = new InMemNetwork();
        var saleNetwork = new InMemNetwork();

        // Bus primário para pedidos
        services.AddRebus(
            (configure, provider) => configure
                .Transport(t => t.UseInMemoryTransport(orderNetwork, ORDER_QUEUE))
                .Routing(r => r.TypeBased().Map<OrderConfirmedMessage>(ORDER_QUEUE))
                .Options(o => o.SetNumberOfWorkers(1)),
            key: "order-confirmation",
            isDefaultBus: true
        );
        services.AddRebusHandler<OrderConfirmedHandler>();

        // Bus secundário para vendas
        services.AddRebus(
            (configure, provider) => configure
                .Transport(t => t.UseInMemoryTransport(saleNetwork, SALE_QUEUE))
                .Routing(r => r.TypeBased().Map<SaleConfirmedMessage>(SALE_QUEUE))
                .Options(o => o.SetNumberOfWorkers(1)),
            key: "sale-confirmation",
            isDefaultBus: false
        );              
        services.AddRebusHandler<SaleConfirmedHandler>();
    }
}
