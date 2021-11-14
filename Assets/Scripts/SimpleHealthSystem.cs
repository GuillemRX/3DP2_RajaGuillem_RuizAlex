using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class SimpleHealthSystem : MonoBehaviour
{
    bool isAlive;

    [SerializeField]
    bool canReespawn;

    Vector3 checkpoint_position;

   
    public void Kill(){
        if(isAlive){
            isAlive = false;
            EventManager.TriggerEvent("player_killed", new Dictionary<string, object>(){});
        }
        
    }

    public void SpawnOnLastCheckpoint(){
        Debug.Log("SPAWNED!");
        gameObject.transform.position = checkpoint_position;
        isAlive = true;
    }

    public void logggg(){
        Debug.Log("WORKS");
    }

    public void SetCheckpoint(Vector3 _checkpoint){
        Debug.Log("CEHCKPOINT SET");
        checkpoint_position = _checkpoint;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "dead-zone"){
            Kill();
        }

    }
}
