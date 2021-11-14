using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionButton : MonoBehaviour
{
  
    [SerializeField]
    private GameObject companionPrefab;

    [SerializeField]
    private float spawnHeight;
    private Vector3 spawnPosition;

    void Start(){
        spawnPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y+spawnHeight, gameObject.transform.position.z - 1.75f);
    }

    void OnTriggerEnter(Collider other) {
        PickableObject objectState = other.gameObject.GetComponent<PickableObject>();
        
        if(other.gameObject.tag == "Player") 
            Instantiate(companionPrefab, spawnPosition, Quaternion.identity);

        if(other.gameObject.tag == "Companion" && objectState && !objectState.isBeingHolded()) 
            Instantiate(companionPrefab, spawnPosition, Quaternion.identity);
    }
}
