using Npgsql;

namespace Discount.API.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrationDatabase<TContext>(this IHost host, int? retry = 0)
        {
            int retryFroAvailability = retry.Value;
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<TContext>>();
                try
                {
                    logger.LogInformation("Migrating postgresql database.");
                    using var connection = new NpgsqlConnection
                        (configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
                    connection.Open();
                    using var command = new NpgsqlCommand
                    {
                        Connection = connection
                    };
                    command.CommandText = "DROP TABLE IF EXISTS Coupon";
                    command.ExecuteNonQuery();

                    command.CommandText = "CREATE TABLE Coupon (" +
                        "Id SERIAL PRIMARY KEY, " +
                        "ProductName VARCHAR(24) NOT NULL, " +
                        "Description TEXT, " +
                        "Amount INT)";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO Coupon (ProductName,Description,Amount) VALUES(" +
                        "'IPhone X', 'IPhone Description', 150)";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO Coupon (ProductName,Description,Amount) VALUES(" +
                        "'Samsung 10', 'Samsung Description', 100)";
                    command.ExecuteNonQuery();

                    logger.LogInformation("Migrating postgresql database.");
                }
                catch (NpgsqlException ex)
                {
                    logger.LogError(ex, "An error occurred while migration the postgresql database.");
                    if (retryFroAvailability < 50)
                    {
                        retryFroAvailability++;
                        System.Threading.Thread.Sleep(2000);
                        MigrationDatabase<TContext>(host, retryFroAvailability);
                    }
                }
            }
            return host;
        }
    }
}
