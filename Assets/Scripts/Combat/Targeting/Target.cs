using System;
using UnityEngine;

public class Target : MonoBehaviour
{
    public event Action<Target> OnDestroyEvent;

    private void OnDestroy()
    {
        OnDestroyEvent?.Invoke(this);
    }
}
