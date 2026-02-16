using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces.Repositories;
using UserService.Infrastructure.EntityFramework.Contexts;

namespace UserService.Infrastructure.Repositories
{
    /// <summary>
    /// Конкретная реализация репозитория пользователей, использующая EF Core и <see cref="UserManager{TUser}"/>.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _context;
        private readonly UserManager<User> _userManager;

        /// <summary>
        /// Конструктор репозитория.
        /// </summary>
        /// <param name="context">Контекст БД.</param>
        /// <param name="userManager">Менеджер работы с Identity-пользователями.</param>
        public UserRepository(UserDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
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

        /// <summary>
        /// Создаёт пользователя через <see cref="UserManager{TUser}"/>.
        /// </summary>
        /// <param name="user">Сущность пользователя.</param>
        /// <param name="password">Пароль в открытом виде.</param>
        /// <returns>Созданный пользователь или <c>null</c>, если создание не удалось.</returns>
        public async Task<User?> AddAsync(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
                return _userManager.Users.FirstOrDefault(u => u.Email == user.Email);
            return null;
        }
    }
}