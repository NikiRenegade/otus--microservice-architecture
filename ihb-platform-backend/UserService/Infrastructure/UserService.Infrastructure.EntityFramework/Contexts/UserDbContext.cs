using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;
using UserService.Infrastructure.EntityFramework.Configurations;

namespace UserService.Infrastructure.EntityFramework.Contexts
{
    public class UserDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        /// <summary>
        /// Контекст базы данных для сущности <see cref="User"/>.
        /// </summary>
        /// <param name="options">Опции контекста.</param>
        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Применяет конфигурации модели (включая <see cref="UserConfiguration"/>).
        /// </summary>
        /// <param name="builder">Builder модели EF.</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new UserConfiguration());
        }
    }
}
