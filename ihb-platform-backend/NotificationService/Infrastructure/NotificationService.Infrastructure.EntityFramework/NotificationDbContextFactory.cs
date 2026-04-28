using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using NotificationService.Infrastructure.EntityFramework.Contexts;

namespace NotificationService.Infrastructure.EntityFramework
{
    /// <summary>
    /// Фабрика контекста БД, используемая на этапе разработки (миграции и т.д.).
    /// Читает строку подключения из переменных окружения и создает <see cref="NotificationDbContext"/>.
    /// </summary>
    public class NotificationDbContextFactory : IDesignTimeDbContextFactory<NotificationDbContext>
    {
        /// <summary>
        /// Создаёт экземпляр <see cref="NotificationDbContext"/>.
        /// </summary>
        /// <param name="args">Аргументы командной строки (не используются).</param>
        /// <returns>Конфигурированный экземпляр <see cref="NotificationDbContext"/>.</returns>
        /// <exception cref="InvalidOperationException">Если строка подключения не найдена в переменных окружения.</exception>
        public NotificationDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetConnectionString("notificationdbconnection");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "Установите переменную окружения ConnectionStrings__notificationgbconnection."
                );
            }

            var optionsBuilder = new DbContextOptionsBuilder<NotificationDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new NotificationDbContext(optionsBuilder.Options);
        }
    }
}