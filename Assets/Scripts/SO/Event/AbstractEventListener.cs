using UnityEngine;
using UnityEngine.Events;

public class AbstractEventListener<T> : MonoBehaviour {
    public AbstractEvent<T> @event;
    public UnityEvent<T> response;

    public void OnRaised(T data) {
        response.Invoke(data);
    }
    private void OnEnable() {
        @event.AddListener(this);
    }
    private void OnDisable() {
        @event.RemoveListener(this);
    }
}