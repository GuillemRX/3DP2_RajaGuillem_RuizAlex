using System;
using UnityEngine;
using Utilities.Singleton;

[CreateAssetMenu(fileName = "Events", menuName = "Scriptable Objects/Events")]
public class Events : SingletonScriptableObject<Events>
{
    public InputActions inputActions;
    public string onTeleportObject;

    public string onButtonPressed;
    

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
}