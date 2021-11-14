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
        if(companionWeight == weight.big && other.transform.localScale.x < 1.2f) return;
        if(other.gameObject.tag == "Companion" && objectState && !objectState.isBeingHolded()){
            EventManager.TriggerEvent("button_pressed", new Dictionary<string, object>(){
                {"id",doorId}
            });
        }
            
    }

    void OnTriggerExit(Collider other) {
        if(other.gameObject.tag == "Companion" ){
            EventManager.TriggerEvent("button_released", new Dictionary<string, object>(){
                {"id",doorId}
            });
        }
            
    }
}
