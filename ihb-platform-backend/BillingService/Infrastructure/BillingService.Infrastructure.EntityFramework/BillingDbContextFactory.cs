using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using BillingService.Infrastructure.EntityFramework.Contexts;

namespace BillingService.Infrastructure.EntityFramework
{
    /// <summary>
    /// Фабрика контекста БД, используемая на этапе разработки (миграции и т.д.).
    /// Читает строку подключения из переменных окружения и создает <see cref="BillingDbContext"/>.
    /// </summary>
    public class BillingDbContextFactory : IDesignTimeDbContextFactory<BillingDbContext>
    {
        /// <summary>
        /// Создаёт экземпляр <see cref="BillingDbContext"/>.
        /// </summary>
        /// <param name="args">Аргументы командной строки (не используются).</param>
        /// <returns>Конфигурированный экземпляр <see cref="BillingDbContext"/>.</returns>
        /// <exception cref="InvalidOperationException">Если строка подключения не найдена в переменных окружения.</exception>
        public BillingDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetConnectionString("billingdbconnection");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "Установите переменную окружения ConnectionStrings__billingdbconnection."
                );
            }

            var optionsBuilder = new DbContextOptionsBuilder<BillingDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new BillingDbContext(optionsBuilder.Options);
        }
    }
}