using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class CollisionAudio : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        EventManager.TriggerEvent(Events.Instance.onObjectCollision, new Dictionary<string, object>
        {
            {"source", gameObject}
        });
    }

    private void OnTriggerEnter(Collider other)
    {
        EventManager.TriggerEvent(Events.Instance.onObjectCollision, new Dictionary<string, object>
        {
            {"source", gameObject}
        });
    }
}