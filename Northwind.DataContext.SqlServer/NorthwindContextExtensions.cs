using Microsoft.Data.SqlClient; // För SqlConnectionStringBuilder
using Microsoft.Extensions.DependencyInjection; // För IServiceCollection
using Microsoft.EntityFrameworkCore; // För AddDbContext, UseSqlServer 
using Northwind.EntityModels;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Northwind.DataContext.SqlServer
{
    public static class NorthwindContextExtensions
    {
        /// <summary>
        /// Extension method för att lägga till NorthwindDatabaseContext i dependency injection
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="connectionString">Lägg till för att override default</param>
        /// <returns>En IServiceColelction som kan användas för att lägga till mer tjänster</returns>
        public static IServiceCollection AddNorthwindContext(
            this IServiceCollection services, 
            string? connectionString = null) // type that extends
        {
            if (connectionString is null)
            {
                SqlConnectionStringBuilder builder = new();

                builder.DataSource = "(localdb)\\MSSQLLocalDB"; 
                builder.InitialCatalog = "NorthwindDatabase";
                builder.IntegratedSecurity = true;
                builder.MultipleActiveResultSets = true;

                builder.ConnectTimeout = 3;

                builder.IntegratedSecurity = true;

                connectionString = builder.ConnectionString;
            }
            services.AddDbContext<NorthwindDatabaseContext>(options =>
            {
                options.UseSqlServer(connectionString);

                options.LogTo(NorthwindContextLogger.WriteLine,
                new[] { Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.CommandExecuting });
            },
            //Registrera NorthwindDatabaseContext med service lifetime transient för att undvika problem me
            contextLifetime: ServiceLifetime.Transient,
            optionsLifetime: ServiceLifetime.Transient);

            return services;
        }
    }
}
