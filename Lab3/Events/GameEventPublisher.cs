namespace Lab3.Events;
public sealed class GameEventPublisher {
    private readonly Dictionary<GameEventType, List<IGameEventListener>> listeners = new();
    public void Subscribe(GameEventType type, IGameEventListener listener) {
        if (!listeners.TryGetValue(type, out var list))
            listeners[type] = list = new();
        list.Add(listener);
    }
    public void Unsubscribe(GameEventType type, IGameEventListener listener) {
        if (listeners.TryGetValue(type, out var list))
            list.Remove(listener);
    }
    public void Publish(GameEvent gameEvent) {
        if (listeners.TryGetValue(gameEvent.Type, out var list))
            foreach (var listener in list.ToArray())
                listener.OnEvent(gameEvent);
    }
}