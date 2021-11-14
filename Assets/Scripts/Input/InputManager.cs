using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;
using Utilities.Singleton;

namespace Input
{
    public class InputManager : SingletonMonoBehaviour<InputManager>
    {
        private void OnLook(InputValue value)
        {
            EventManager.TriggerEvent(Events.Instance.inputActions.onLook, new Dictionary<string, object>
            {
                {"value", value.Get<Vector2>()}
            });
        }

        private void OnMove(InputValue value)
        {
            EventManager.TriggerEvent(Events.Instance.inputActions.onMove, new Dictionary<string, object>
            {
                {"value", value.Get<Vector2>()}
            });
        }

        private void OnJump(InputValue value)
        {
            EventManager.TriggerEvent(Events.Instance.inputActions.onJump, new Dictionary<string, object>
            {
                {"value", value.isPressed}
            });
        }
        
        private void OnRightClick(InputValue value)
        {
            EventManager.TriggerEvent(Events.Instance.inputActions.onRightClick, new Dictionary<string, object>
            {
                {"value", value.isPressed}
            });
        }
        
        private void OnLeftClick(InputValue value)
        {
            EventManager.TriggerEvent(Events.Instance.inputActions.onLeftClick, new Dictionary<string, object>
            {
                {"value", value.isPressed}
            });
        }
        
        private void OnScroll(InputValue value)
        {
            EventManager.TriggerEvent(Events.Instance.inputActions.onScroll, new Dictionary<string, object>
            {
                {"value", value.Get<float>()}
            });
        }

        private void OnRespawn(InputValue value)
        {
            EventManager.TriggerEvent("player_spawned", null);
        }
    }
}