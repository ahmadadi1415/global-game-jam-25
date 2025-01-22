using System;
using System.Collections.Generic;

public static class EventManager
{
    private static readonly Dictionary<Type, Action<object>> events = new();

    public static void Subscribe<T>(Action<T> listener) where T : struct
    {
        Type eventType = typeof(T);
        if (!events.ContainsKey(eventType))
        {
            events[eventType] = delegate { };
        }
        events[eventType] += _ => listener((T)_);
    }

    public static void Unsubscribe<T>(Action<T> listener) where T : struct
    {
        Type eventType = typeof(T);
        if (events.ContainsKey(eventType))
        {
            events[eventType] -= _ => listener((T)_);
        }
    }

    public static void Publish<T>(T eventData) where T : struct
    {
        var eventType = typeof(T);

        if (events.TryGetValue(eventType, out var action))
        {
            action?.Invoke(eventData);
        }
    }
}