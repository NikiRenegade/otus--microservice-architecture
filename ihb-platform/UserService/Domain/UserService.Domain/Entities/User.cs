using Microsoft.AspNetCore.Identity;
namespace UserService.Domain.Entities;


/// <summary>
/// Модель пользователя, расширяющая <see cref="IdentityUser{TKey}"/> с ключом типа <see cref="Guid"/>.
/// Содержит дополнительные поля для ФИО и времени создания.
/// </summary>
public class User : IdentityUser<Guid>
{
    /// <summary>
    /// Имя пользователя.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Фамилия пользователя.
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Время создания записи пользователя.
    /// </summary>
    public DateTime CreatedAt { get; set; }


    /// <summary>
    /// Конструктор для создания пользователя с явным указанием основных полей.
    /// </summary>
    /// <param name="email">Email пользователя.</param>
    /// <param name="userName">Логин/имя пользователя.</param>
    /// <param name="firstName">Имя.</param>
    /// <param name="lastName">Фамилия.</param>
    public User(string email, string userName, string firstName, string lastName)
    {
        Email = email;
        UserName = userName;
        FirstName = firstName;
        LastName = lastName;
    }

    /// <summary>
    /// Пустой конструктор
    /// </summary>
    public User()
    {
    }
    public bool CheckPassword(string password, IPasswordHasher<User> hasher)
    {
        return hasher.VerifyHashedPassword(this, PasswordHash, password) == PasswordVerificationResult.Success;
    }

    public void ChangePassword(string currentPassword, string newPassword, IPasswordHasher<User> hasher)
    {
        if (!CheckPassword(currentPassword, hasher))
            throw new InvalidOperationException("Неверный текущий пароль.");

        PasswordHash = hasher.HashPassword(this, newPassword);
    }

    // Метод для внешней аутентификации (Google, etc.)
    public static User CreateOrUpdateFromExternalProvider(string email, string userName, string firstName, string lastName)
    {
        return new User(email, userName, firstName, lastName);
    }
}