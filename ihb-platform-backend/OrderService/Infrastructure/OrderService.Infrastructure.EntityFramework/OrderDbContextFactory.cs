using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using OrderService.Infrastructure.EntityFramework.Contexts;

namespace OrderService.Infrastructure.EntityFramework
{
    /// <summary>
    /// Фабрика контекста БД, используемая на этапе разработки (миграции и т.д.).
    /// Читает строку подключения из переменных окружения и создает <see cref="OrderDbContext"/>.
    /// </summary>
    public class OrderDbContextFactory : IDesignTimeDbContextFactory<OrderDbContext>
    {
        /// <summary>
        /// Создаёт экземпляр <see cref="OrderDbContext"/>.
        /// </summary>
        /// <param name="args">Аргументы командной строки (не используются).</param>
        /// <returns>Конфигурированный экземпляр <see cref="OrderDbContext"/>.</returns>
        /// <exception cref="InvalidOperationException">Если строка подключения не найдена в переменных окружения.</exception>
        public OrderDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetConnectionString("orderdbconnection");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "Установите переменную окружения ConnectionStrings__orderdbconnection."
                );
            }

            var optionsBuilder = new DbContextOptionsBuilder<OrderDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new OrderDbContext(optionsBuilder.Options);
        }
    }
}