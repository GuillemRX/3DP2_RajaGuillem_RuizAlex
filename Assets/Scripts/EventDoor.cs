using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class EventDoor : Door
{
    [SerializeField]
    string doorId;

     void OnEnable(){
        EventManager.StartListening(Events.Instance.onButtonPressed, EventExecuted);
    }

    void OnDisable(){
        EventManager.StopListening(Events.Instance.onButtonPressed, EventExecuted);
    }

    
    void EventExecuted(Dictionary<string,object> args){
        if((string)args["id"] == doorId){
            OpenDoor();
        }
        
    }
}
