using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour
{

    private bool _isBeingHolded;

    // Start is called before the first frame update
    void Start()
    {
        _isBeingHolded = false;
    }

    public void startHolding(){
        _isBeingHolded = true;
        Debug.Log(_isBeingHolded);
        Debug.Log(gameObject.transform.localScale.x);

    }

    public void stopHolding(){
        _isBeingHolded = false;
        Debug.Log(_isBeingHolded);
    }

    public bool isBeingHolded(){
        return _isBeingHolded;
    }
}
