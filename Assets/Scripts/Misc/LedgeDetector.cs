using System;
using UnityEngine;

public class LedgeDetector : MonoBehaviour
{
    public Action<Vector3, Vector3> OnLedgeDetect;

    private void OnTriggerEnter(Collider other)
    {
        // as soon as a ledge is detected, an event will be invoked that'll carry the forward vec. of the ledge and the closest point
        // on the ledge collider's outside to the ledge detector itself:
        OnLedgeDetect?.Invoke(other.transform.forward, other.ClosestPointOnBounds(transform.position));
    }
}
