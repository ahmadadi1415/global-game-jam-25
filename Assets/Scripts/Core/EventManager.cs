using System;
using System.Collections.Generic;

public static class EventManager
{
    private static readonly Dictionary<Type, Delegate> events = new();

    public static void Subscribe<T>(Action<T> listener) where T : struct
    {
        Type eventType = typeof(T);
        if (!events.ContainsKey(eventType))
        {
            events[eventType] = listener;
        }
        else
        {
            events[eventType] = Delegate.Combine(events[eventType], listener);
        }
    }

    public static void Unsubscribe<T>(Action<T> listener) where T : struct
    {
        Type eventType = typeof(T);
        if (events.ContainsKey(eventType))
        {
            events[eventType] = Delegate.Remove(events[eventType], listener);
        }
    }

    public static void Publish<T>(T eventData) where T : struct
    {
        Type eventType = typeof(T);

        if (events.TryGetValue(eventType, out Delegate delegates))
        {
            if (delegates is Action<T> typedDelegates)
            {
                typedDelegates.Invoke(eventData);
            }
        }
    }
}