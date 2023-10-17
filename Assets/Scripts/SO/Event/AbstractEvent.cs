using System.Collections.Generic;
using UnityEngine;

public class AbstractEvent<T> : ScriptableObject {
    private readonly List<AbstractEventListener<T>> listeners = new();

    public void Raise(T data) {
        for (int i = listeners.Count - 1; i >= 0; --i) {
            listeners[i].OnRaised(data);
        }
    }

    public void AddListener(AbstractEventListener<T> listener) {
        if(!listeners.Contains(listener)) {
            listeners.Add(listener);
        }
    }
    public void RemoveListener(AbstractEventListener<T> listener) {
        listeners.Remove(listener);
    }
}