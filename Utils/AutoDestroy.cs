using UnityEngine;

/// <summary> Destroy a GameObject after a certain amount of time </summary>
public class AutoDestroy : MonoBehaviour
{
    [Tooltip("Destroy the GameObject when lifetime expires (in seconds)")][SerializeField] private float lifetime = 1f;

    private void Start()
    {
        Invoke(nameof(Destroy), lifetime);
    }

    private void Destroy()
    {
#if UNITY_EDITOR
        DestroyImmediate(gameObject);
#else
        Destroy(gameObject);
#endif
    }

    private void OnDestroy()
    {
        CancelInvoke(nameof(Destroy)); // prevent any zombie call
    }
}