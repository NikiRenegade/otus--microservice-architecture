using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables; // <- вот это обязательно
using UserService.Infrastructure.EntityFramework.Contexts;

namespace UserService.Infrastructure.EntityFramework
{
    /// <summary>
    /// Фабрика контекста БД, используемая на этапе разработки (миграции и т.д.).
    /// Читает строку подключения из переменных окружения и создает <see cref="UserDbContext"/>.
    /// </summary>
    public class UserDbContextFactory : IDesignTimeDbContextFactory<UserDbContext>
    {
        /// <summary>
        /// Создаёт экземпляр <see cref="UserDbContext"/>.
        /// </summary>
        /// <param name="args">Аргументы командной строки (не используются).</param>
        /// <returns>Конфигурированный экземпляр <see cref="UserDbContext"/>.</returns>
        /// <exception cref="InvalidOperationException">Если строка подключения не найдена в переменных окружения.</exception>
        public UserDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetConnectionString("userdbconnection");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "Установите переменную окружения ConnectionStrings__userdbconnection."
                );
            }

            var optionsBuilder = new DbContextOptionsBuilder<UserDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new UserDbContext(optionsBuilder.Options);
        }
    }
}