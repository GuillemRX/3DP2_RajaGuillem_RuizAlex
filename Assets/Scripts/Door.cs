using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform doorTransform;

    private Vector3 _originalPosition;

    [SerializeField]
    private Vector3 _finalPosition;

    private void Awake()
    {
        _originalPosition = doorTransform.position;
    }

    public void OpenDoor(){
        doorTransform.DOMove(_finalPosition, 1f);
    }

    public void CloseDoor(){
        doorTransform.DOMove(_originalPosition, 1f);
    }
}