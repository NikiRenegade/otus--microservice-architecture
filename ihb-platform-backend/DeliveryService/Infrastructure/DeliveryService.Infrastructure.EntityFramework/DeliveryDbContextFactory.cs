using DeliveryService.Infrastructure.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DeliveryService.Infrastructure.EntityFramework;

public class DeliveryDbContextFactory: IDesignTimeDbContextFactory<DeliveryDbContext>
{
    /// <summary>
    /// Создаёт экземпляр <see cref="DeliveryDbContext"/>.
    /// </summary>
    /// <param name="args">Аргументы командной строки (не используются).</param>
    /// <returns>Конфигурированный экземпляр <see cref="DeliveryDbContext"/>.</returns>
    /// <exception cref="InvalidOperationException">Если строка подключения не найдена в переменных окружения.</exception>
    public DeliveryDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("deliverydbconnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Установите переменную окружения ConnectionStrings__deliverybconnection."
            );
        }

        var optionsBuilder = new DbContextOptionsBuilder<DeliveryDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new DeliveryDbContext(optionsBuilder.Options);
    }
}