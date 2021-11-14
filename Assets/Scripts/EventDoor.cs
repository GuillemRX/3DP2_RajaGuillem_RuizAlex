using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class EventDoor : Door
{
    [SerializeField]
    string doorId;

     void OnEnable(){
        EventManager.StartListening("button_pressed", EventOpenDoorExecuted);
        EventManager.StartListening("button_released", EventCloseDoorExecuted);
    }

    void OnDisable(){
        EventManager.StopListening("button_pressed", EventOpenDoorExecuted);
        EventManager.StopListening("button_released", EventCloseDoorExecuted);
    }

    
    void EventOpenDoorExecuted(Dictionary<string,object> args){
        
        if((string)args["id"] == doorId){
            OpenDoor();
        }
        
    }

    void EventCloseDoorExecuted(Dictionary<string,object> args){
        
        if((string)args["id"] == doorId){
            CloseDoor();
        }
        
    }
}
