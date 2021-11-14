using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class WeightedButton : MonoBehaviour
{
    [SerializeField]
    private weight companionWeight;

    [SerializeField]
    private string doorId;

    private enum weight{
        medium,
        big,
    }


    void OnTriggerEnter(Collider other) {
        PickableObject objectState = other.gameObject.GetComponent<PickableObject>();
        
        if(other.gameObject.tag == "Companion" && objectState && !objectState.isBeingHolded()){
            EventManager.TriggerEvent(Events.Instance.onButtonPressed, new Dictionary<string, object>(){
                {"id",doorId}
            });
        }
            
    }
}
