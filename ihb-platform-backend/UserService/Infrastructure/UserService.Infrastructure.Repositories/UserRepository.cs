using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces.Repositories;
using UserService.Infrastructure.EntityFramework.Contexts;

namespace UserService.Infrastructure.Repositories
{
    /// <summary>
    /// Конкретная реализация репозитория пользователей, использующая EF Core,
    /// <see cref="UserManager{TUser}" /> и 
    /// <see cref="SignInManager{TUser}" />.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _context;

        /// <summary>
        /// Конструктор репозитория.
        /// </summary>
        /// <param name="context">Контекст БД.</param>
        /// <param name="userManager">Менеджер работы с Identity-пользователями.</param>
        /// <param name="signInManager">Менеджер работы с Identity-пользователями для попытки входа в систему.</param>
        public UserRepository(UserDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Возвращает всех пользователей из базы.
        /// </summary>
        /// <returns>Перечисление пользователей или <c>null</c>.</returns>
        public async Task<IEnumerable<User>?> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        /// <summary>
        /// Возвращает пользователя по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        /// <returns>Пользователь или <c>null</c>.</returns>
        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        /// <summary>
        /// Обновляет поля существующего пользователя и сохраняет изменения.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        /// <param name="entity">Сущность с новыми значениями полей.</param>
        /// <returns><c>true</c>, если обновление выполнено; иначе <c>false</c>.</returns>
        public async Task<bool> UpdateAsync(Guid id, User entity)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);
            if (existingUser == null)
                return false;

            existingUser.FirstName = entity.FirstName;
            existingUser.LastName = entity.LastName;
            existingUser.Email = entity.Email;
            existingUser.UserName = entity.UserName;

            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Удаляет пользователя из базы.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        /// <returns><c>true</c>, если удаление успешно; иначе <c>false</c>.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);
            if (existingUser == null)
                return false;
            _context.Users.Remove(existingUser);
            await _context.SaveChangesAsync();
            return true;
        }
        /// <summary>
        /// Поиск пользователя по Email.
        /// </summary>
        /// <param name="email">Email для поиска.</param>
        /// <returns>Пользователь или <c>null</c>.</returns>
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}