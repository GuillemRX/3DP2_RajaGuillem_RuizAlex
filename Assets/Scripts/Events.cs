using System;
using UnityEngine;
using Utilities.Singleton;

[CreateAssetMenu(fileName = "Events", menuName = "Scriptable Objects/Events")]
public class Events : SingletonScriptableObject<Events>
{
    public string onTeleportObject;
    public string onObjectCollision;
    public string onButtonPressed;
    public string onButtonReleased;
    public InputActions inputActions;
    public PlayerActions playerActions;
    

    [Serializable]
    public struct InputActions
    {
        public string onLook;
        public string onMove;
        public string onJump;
        public string onLeftClick;
        public string onRightClick;
        public string onScroll;
    }
    
    [Serializable]
    public struct PlayerActions
    {
        public string onPreviewBluePortal;
        public string onPreviewOrangePortal;
        public string onPlaceBluePortal;
        public string onPlaceOrangePortal;
        public string onGravityGunGrab;
        public string onGravityGunThrow;
        public string onGravityGunDrop;
    }
}