using InventoryService.Infrastructure.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace InventoryService.Infrastructure.EntityFramework;

public class InventoryDbContextFactory: IDesignTimeDbContextFactory<InventoryDbContext>
{
    /// <summary>
    /// Создаёт экземпляр <see cref="InventoryDbContext"/>.
    /// </summary>
    /// <param name="args">Аргументы командной строки (не используются).</param>
    /// <returns>Конфигурированный экземпляр <see cref="InventoryDbContext"/>.</returns>
    /// <exception cref="InvalidOperationException">Если строка подключения не найдена в переменных окружения.</exception>
    public InventoryDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("inventorydbconnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Установите переменную окружения ConnectionStrings__inventorydbconnection."
            );
        }

        var optionsBuilder = new DbContextOptionsBuilder<InventoryDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new InventoryDbContext(optionsBuilder.Options);
    }
}