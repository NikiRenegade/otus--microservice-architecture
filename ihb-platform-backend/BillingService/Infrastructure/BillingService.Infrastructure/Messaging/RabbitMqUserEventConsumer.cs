using BillingService.Domain.DTOs;
using BillingService.Domain.Interfaces.Services;
using Shared.RabbitMq.Interfaces;

namespace BillingService.Infrastructure.Messaging;

public class RabbitMqUserEventConsumer : IRabbitMqConsumer
{
    private readonly IEventConsumer _consumer;
    private readonly IAccountService _accountService;
    private const string ExchangeName = "user-events";

    public RabbitMqUserEventConsumer(IEventConsumer consumer, IAccountService accountService)
    {
        _consumer = consumer;
        _accountService = accountService;
    }

    public async Task StartAsync()
    {
        await _consumer.SubscribeAsync<AccountCreateDto>("user", "user.created", ExchangeName,
            async @event => { await _accountService.AddAsync(@event); });

        await _consumer.SubscribeAsync<AccountChangeEmailDto>("user", "user.emailchange", ExchangeName,
            async @event => { await _accountService.UpdateEmailAsync(@event.UserId, @event.UserEmail); });

        await _consumer.SubscribeAsync<Guid>("user", "user.deleted", ExchangeName,
            async @event => { await _accountService.DeleteAsync(@event); });
    }
}