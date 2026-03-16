using UserService.Domain.Events;

namespace UserService.Domain.Interfaces.Publishers;

public interface IUserEventPublisher
{
    public Task PublishUserCreated(UserCreatedEvent userCreatedEvent);
    public Task PublishUserUpdated(UserUpdatedEvent userUpdatedEvent);
    public Task PublishUserDeleted(Guid userId);
}