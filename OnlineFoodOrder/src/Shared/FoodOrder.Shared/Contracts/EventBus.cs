using Microsoft.Extensions.Logging;

namespace FoodOrder.Shared.Contracts;

// ── Interface ─────────────────────────────────────────────────────────────────
public interface IEventBus
{
    Task PublishAsync<T>(T @event) where T : Events.IntegrationEvent;
}

// ── In-Memory (swap with RabbitMQ / Azure Service Bus in production) ──────────
public class InMemoryEventBus : IEventBus
{
    private readonly ILogger<InMemoryEventBus> _logger;
    private static readonly Dictionary<Type, List<Func<object, Task>>> _subs = new();

    public InMemoryEventBus(ILogger<InMemoryEventBus> logger) => _logger = logger;

    public static void Subscribe<T>(Func<T, Task> handler) where T : Events.IntegrationEvent
    {
        var t = typeof(T);
        if (!_subs.ContainsKey(t)) _subs[t] = new();
        _subs[t].Add(e => handler((T)e));
    }

    public async Task PublishAsync<T>(T @event) where T : Events.IntegrationEvent
    {
        _logger.LogInformation("[EventBus] {Event} published", typeof(T).Name);
        if (_subs.TryGetValue(typeof(T), out var handlers))
            foreach (var h in handlers) await h(@event);
    }
}

// ── API Response wrapper ──────────────────────────────────────────────────────
public class ApiResponse<T>
{
    public bool    Success { get; set; }
    public string? Message { get; set; }
    public T?      Data    { get; set; }

    public static ApiResponse<T> Ok(T data, string? msg = null)
        => new() { Success = true, Data = data, Message = msg };

    public static ApiResponse<T> Fail(string msg)
        => new() { Success = false, Message = msg };
}
