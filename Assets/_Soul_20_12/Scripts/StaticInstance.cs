using UnityEngine;

/// <summary>
/// A static instance is similar to a singleton, but instead of destroying any new
/// instances, it overrides the current instance. This is handy for resetting the state
/// and saves you doing it manually
/// </summary>
public abstract class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour {
    public static T Ins { get; private set; }
    protected virtual void Awake() => Ins = this as T;

    protected virtual void OnApplicationQuit() {
        Ins = null;
        Destroy(gameObject);
    }
}
