using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using Player;

public class DeadScreenManager : MonoBehaviour
{

    [SerializeField]
    GameObject deadScreen;

    void Start(){
        deadScreen.SetActive(false);
    }

    void OnEnable(){
        EventManager.StartListening("player_killed", EventPlayerDied);
        EventManager.StartListening("player_spawned", EventPlayerSpawn);
    }

    void OnDisable(){
        EventManager.StopListening("player_killed", EventPlayerDied);
        EventManager.StopListening("player_spawned", EventPlayerSpawn);
    }

    void EventPlayerDied(Dictionary<string,object> args){
        deadScreen.SetActive(true);

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        FPSCameraController cam = player.GetComponentInChildren<FPSCameraController>();
        cam.enabled = false;

        FPSCharacterController characterController = player.GetComponentInChildren<FPSCharacterController>();
        characterController.enabled = false;
        
    }

    void EventPlayerSpawn(Dictionary<string,object> args){
        if(deadScreen.activeSelf){
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponentInChildren<SimpleHealthSystem>().SpawnOnLastCheckpoint();

            deadScreen.SetActive(false);
            
            FPSCameraController cam = player.GetComponentInChildren<FPSCameraController>();
            cam.enabled = true;

            FPSCharacterController characterController = player.GetComponentInChildren<FPSCharacterController>();
            characterController.enabled = true;

            
        }
        
    }



}
